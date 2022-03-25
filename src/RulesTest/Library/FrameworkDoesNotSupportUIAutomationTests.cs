// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]

    public class FrameworkDoesNotSupportUIAutomationTests
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
                    .Returns(nonWin32Value)
                    .Verifiable();

                Assert.IsFalse(Rule.Condition.Matches(_elementMock.Object));
            }

            _elementMock.Verify(m => m.Framework, Times.Exactly(nonWin32Values.Length));
        }

        [TestMethod]
        public void Condition_FrameworkIsWin32_ControlTypeIsNotWindow_ReturnsFalse()
        {
            _elementMock.Setup(m => m.Framework).Returns(FrameworkId.Win32).Verifiable();
            _elementMock.Setup(m => m.ControlTypeId).Returns(Core.Types.ControlType.UIA_ButtonControlTypeId);

            Assert.IsFalse(Rule.Condition.Matches(_elementMock.Object));

            _elementMock.Verify(m => m.Framework, Times.Once());
            _elementMock.Verify(m => m.ControlTypeId, Times.Once());
        }

        [TestMethod]
        public void Condition_FrameworkIsWin32_ControlTypeIsWindow_ReturnsTrue()
        {
            _elementMock.Setup(m => m.Framework).Returns(FrameworkId.Win32).Verifiable();
            _elementMock.Setup(m => m.ControlTypeId).Returns(Core.Types.ControlType.UIA_WindowControlTypeId);

            Assert.IsTrue(Rule.Condition.Matches(_elementMock.Object));

            _elementMock.Verify(m => m.Framework, Times.Once());
            _elementMock.Verify(m => m.ControlTypeId, Times.Once());
        }

        [TestMethod]
        public void PassesTest_ClassNameIsNotKnownProblematic_ReturnsFalse()
        {
            string[] testClasses = { "Edit", "SunAwT" };

            foreach (string testClass in testClasses)
            {
                _elementMock.Setup(m => m.ClassName).Returns(testClass).Verifiable(); ;

                Assert.IsTrue(Rule.PassesTest(_elementMock.Object));
            }

            _elementMock.VerifyAll();
        }

        [TestMethod]
        public void PassesTest_ClassNameIsKnownProblematic_ReturnsTrue()
        {
            string[] testClasses = { "SunAwt", "SunAwtXyz" };

            foreach (string testClass in testClasses)
            {
                _elementMock.Setup(m => m.ClassName).Returns(testClass).Verifiable(); ;

                Assert.IsFalse(Rule.PassesTest(_elementMock.Object));
            }

            _elementMock.VerifyAll();
        }
    }
}
