// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]

    public class FrameworkDoesNotSupportUIAutomationUnitTests
    {
        private readonly static Rules.IRule Rule = new Rules.Library.FrameworkDoesNotSupportUIAutomation();
        private Mock<IA11yElement> _elementMock;

        [TestInitialize]
        public void BeforeEach()
        {
            _elementMock = new Mock<IA11yElement>(MockBehavior.Strict);
        }

        [TestMethod]
        public void Condition_FrameworkIsNotWin32_ReturnsFalse()
        {
            string[] nonWin32Values = { FrameworkId.DirectUI, FrameworkId.Edge, FrameworkId.InternetExplorer, FrameworkId.WinForm, FrameworkId.WPF, FrameworkId.XAML, "NotWin32" };

            foreach (string nonWin32Value in nonWin32Values)
            {
                _elementMock.Setup(m => m.Framework)
                    .Returns(FrameworkId.DirectUI)
                    .Verifiable();

                Assert.IsFalse(Rule.Condition.Matches(_elementMock.Object));
            }

            _elementMock.Verify(m => m.Framework, Times.Exactly(nonWin32Values.Length));
        }

        [TestMethod]
        public void Condition_FrameworkIsWin32_ReturnsTrue()
        {
            _elementMock.Setup(m => m.Framework).Returns(FrameworkId.Win32).Verifiable();

            Assert.IsTrue(Rule.Condition.Matches(_elementMock.Object));

            _elementMock.Verify(m => m.Framework, Times.Once());
        }

        [TestMethod]
        public void PassesTest_FrameworkIsWin32_ClassNameIsNotKnownBad_ReturnsFalse()
        {
            _elementMock.Setup(m => m.ClassName).Returns("Edit").Verifiable(); ;

            Assert.IsTrue(Rule.PassesTest(_elementMock.Object));

            _elementMock.VerifyAll();
        }

        [TestMethod]
        public void PassesTest_FrameworkIsWin32_ClassNameIsKnownBad_ReturnsTrue()
        {
            _elementMock.Setup(m => m.ClassName).Returns("SunAwtFrame").Verifiable(); ;

            Assert.IsFalse(Rule.PassesTest(_elementMock.Object));

            _elementMock.VerifyAll();
        }
    }
}
