// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class EdgeBrowserHasBeenDeprecatedTests
    {
        private static readonly Rules.IRule Rule = new Rules.Library.EdgeBrowserHasBeenDeprecated();
        private Mock<IA11yElement> _elementMock;

        [TestInitialize]
        public void BeforeEach()
        {
            _elementMock = new Mock<IA11yElement>(MockBehavior.Strict);
        }

        [TestMethod]
        public void FrameworkIssueLink_IsNotNull()
        {
            Assert.IsNotNull(Rule.Info.FrameworkIssueLink);
        }

        [TestMethod]
        public void Condition_FrameworkIsNotEdge_ReturnsFalse()
        {
            string[] nonWin32Values = { FrameworkId.DirectUI, FrameworkId.InternetExplorer, FrameworkId.WinForm, FrameworkId.WPF, FrameworkId.XAML, FrameworkId.Win32, "NotEdge" };

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
        public void Condition_FrameworkIsEdge_ReturnsTrue()
        {
            _elementMock.Setup(m => m.Framework).Returns(FrameworkId.Edge).Verifiable();

            Assert.IsTrue(Rule.Condition.Matches(_elementMock.Object));

            _elementMock.Verify(m => m.Framework, Times.Once());
        }

        [TestMethod]
        public void PassesTest_ReturnsFalse()
        {
            Assert.IsFalse(Rule.PassesTest(_elementMock.Object));
        }
    }
}
