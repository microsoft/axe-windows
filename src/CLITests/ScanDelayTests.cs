// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using AxeWindowsCLI;
using AxeWindowsCLI.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Globalization;
using System.IO;

namespace AxeWindowsCLITests
{
    [TestClass]
    public class ScanDelayTests
    {
        private Mock<TextWriter> _writerMock;
        private Mock<Action> _oneSecondDelayMock;
        private Mock<IOptions> _optionsMock;
        private IScanDelay _testSubject;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _writerMock = new Mock<TextWriter>(MockBehavior.Strict);
            _oneSecondDelayMock = new Mock<Action>(MockBehavior.Strict);

            _testSubject = new ScanDelay(_writerMock.Object, _oneSecondDelayMock.Object);

            _optionsMock = new Mock<IOptions>(MockBehavior.Strict);
        }

        private void VerifyWriterAndOptionsMocks()
        {
            _writerMock.VerifyAll();
            _optionsMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Ctor_WriterIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(
                () => new ScanDelay(null, _oneSecondDelayMock.Object));
            Assert.AreEqual("writer", e.ParamName);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Ctor_OneSecondDelayIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(
                () => new ScanDelay(_writerMock.Object, null));
            Assert.AreEqual("oneSecondDelay", e.ParamName);
        }

        [TestMethod]
        [Timeout(1000)]
        public void DelayWithCountdown_OptionsIsNull_ThrowsArgrumentNullException()
        {
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(
                () => _testSubject.DelayWithCountdown(null));
            Assert.AreEqual("options", e.ParamName);
        }

        [TestMethod]
        [Timeout(1000)]
        public void DelayWithCountdown_DelayIsZero_NoOutputNoDelay()
        {
            _optionsMock.Setup(m => m.DelayInSeconds).Returns(0);

            _testSubject.DelayWithCountdown(_optionsMock.Object);

            VerifyWriterAndOptionsMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void DelayWithCountdown_DelayIsNotZero_VerbosityIsNotQuiet_HasDelayWithoutOutput()
        {
            const int delayInSeconds = 60;

            _optionsMock.Setup(m => m.DelayInSeconds).Returns(delayInSeconds);
            _optionsMock.Setup(m => m.VerbosityLevel).Returns(VerbosityLevel.Quiet);
            _oneSecondDelayMock.Setup(m => m.Invoke());

            _testSubject.DelayWithCountdown(_optionsMock.Object);

            _oneSecondDelayMock.Verify(m => m.Invoke(), Times.Exactly(delayInSeconds));
            VerifyWriterAndOptionsMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void DelayWithCountdown_DelayIsNotZero_VerbosityIsNotQuiet_HasDelayWithOutput()
        {
            const int delayInSeconds = 3;

            _optionsMock.Setup(m => m.DelayInSeconds).Returns(delayInSeconds);
            _optionsMock.Setup(m => m.VerbosityLevel).Returns(VerbosityLevel.Default);
            _oneSecondDelayMock.Setup(m => m.Invoke());

            WriteCall[] expectedCalls =
            {
                PackFormattedCall(DisplayStrings.ScanDelayHeaderMoreThanOneSecond, 3),
                PackFormattedCall(DisplayStrings.ScanDelayCountdownMoreThanOneSecondLeft, 3),
                PackFormattedCall(DisplayStrings.ScanDelayCountdownMoreThanOneSecondLeft, 2),
                PackFormattedCall(DisplayStrings.ScanDelayCountdownOneSecondLeft, 1),
                new WriteCall(DisplayStrings.ScanDelayTriggeringScan, WriteSource.WriteLineStringOnly),
            };
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);

            _testSubject.DelayWithCountdown(_optionsMock.Object);

            textWriterVerifier.VerifyAll();
            _oneSecondDelayMock.Verify(m => m.Invoke(), Times.Exactly(delayInSeconds));
            VerifyWriterAndOptionsMocks();
        }

        private static WriteCall PackFormattedCall(string formatString, int count)
        {
            string expectedString = string.Format(CultureInfo.CurrentCulture, formatString, count);
            return new WriteCall(expectedString, WriteSource.WriteLineStringOnly);
        }
    }
}
