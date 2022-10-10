// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Axe.Windows.AutomationTests
{
    [TestClass]
    public class ScanToolsBuilderTests
    {
        private Mock<IAxeWindowsActions> _actionsMock;
        private Mock<IDPIAwareness> _dpiAwarenessMock;
        private Mock<IOutputFileHelper> _outputFileHelperMock;
        private Mock<IScanResultsAssembler> _resultsAssemblerMock;
        private Mock<ITargetElementLocator> _targetElementLocatorMock;
        private Mock<IFactory> _factoryMock;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _actionsMock = new Mock<IAxeWindowsActions>(MockBehavior.Strict);
            _dpiAwarenessMock = new Mock<IDPIAwareness>(MockBehavior.Strict);
            _outputFileHelperMock = new Mock<IOutputFileHelper>(MockBehavior.Strict);
            _resultsAssemblerMock = new Mock<IScanResultsAssembler>(MockBehavior.Strict);
            _targetElementLocatorMock = new Mock<ITargetElementLocator>(MockBehavior.Strict);
            _factoryMock = new Mock<IFactory>(MockBehavior.Strict);

            _factoryMock.Setup(x => x.CreateAxeWindowsActions()).Returns(_actionsMock.Object);
            _factoryMock.Setup(x => x.CreateResultsAssembler()).Returns(_resultsAssemblerMock.Object);
            _factoryMock.Setup(x => x.CreateTargetElementLocator()).Returns(_targetElementLocatorMock.Object);
        }

        private void VerifyAllMocks()
        {
            _actionsMock.VerifyAll();
            _dpiAwarenessMock.VerifyAll();
            _outputFileHelperMock.VerifyAll();
            _resultsAssemblerMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
            _factoryMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Build_WithOutputDirectory_CreatesExpectedObject()
        {
            const string expectedPath = @"c:\_TestPath";

            _factoryMock.Setup(x => x.CreateDPIAwareness()).Returns(_dpiAwarenessMock.Object);

            string tempString = null;
            _factoryMock.Setup(x => x.CreateOutputFileHelper(expectedPath))
                .Callback<string>(s => tempString = s)
                .Returns(_outputFileHelperMock.Object);

            _outputFileHelperMock.Setup(x => x.GetNewA11yTestFilePath(It.IsAny<Func<string, string>>()))
                .Returns(() => tempString);

            var builder = new ScanToolsBuilder(_factoryMock.Object);

            var scanTools = builder
                .WithOutputDirectory(expectedPath)
                .Build();

            Assert.IsNotNull(scanTools);
            Assert.IsNotNull(scanTools.Actions);
            Assert.IsNotNull(scanTools.DpiAwareness);
            Assert.IsNotNull(scanTools.OutputFileHelper);
            Assert.IsNotNull(scanTools.ResultsAssembler);
            Assert.IsNotNull(scanTools.TargetElementLocator);

            Assert.AreEqual(expectedPath, scanTools.OutputFileHelper.GetNewA11yTestFilePath((value) => value));

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Build_WithDPIAwareness_CreatesExpectedObject()
        {
            _factoryMock.Setup(x => x.CreateOutputFileHelper(null))
                .Returns(_outputFileHelperMock.Object);

            var builder = new ScanToolsBuilder(_factoryMock.Object);

            var scanTools = builder
                .WithDPIAwareness(_dpiAwarenessMock.Object)
                .Build();

            Assert.IsNotNull(scanTools);
            Assert.IsNotNull(scanTools.Actions);
            Assert.IsNotNull(scanTools.DpiAwareness);
            Assert.IsNotNull(scanTools.OutputFileHelper);
            Assert.IsNotNull(scanTools.ResultsAssembler);
            Assert.IsNotNull(scanTools.TargetElementLocator);

            Assert.AreSame(_dpiAwarenessMock.Object, scanTools.DpiAwareness);

            VerifyAllMocks();
        }
    } // class
} // namespace
