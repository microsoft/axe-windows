// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class ChromiumComponentsShouldUseWebScannerTests
    {
        private static readonly Rules.IRule Rule = new Rules.Library.ChromiumComponentsShouldUseWebScanner();
        private Mock<IA11yElement> _elementMock;
        private Mock<IA11yElement> _elementParentMock;

        [TestInitialize]
        public void BeforeEach()
        {
            _elementMock = new Mock<IA11yElement>(MockBehavior.Strict);
            _elementParentMock = new Mock<IA11yElement>(MockBehavior.Strict);
        }

        [TestMethod]
        public void Exclusionary_ReturnsTrue()
        {
            Assert.IsTrue(Rule.IsExclusionRule);
        }

        [TestMethod]
        public void Condition_FrameworkIsNotChrome_ReturnsFalse()
        {
            IEnumerable<string> nonChromeValues = Extensions.GetFrameworkIds().Append("NotChrome").Except(new string[] { FrameworkId.Chrome });

            foreach (string nonChromeValue in nonChromeValues)
            {
                _elementMock.Setup(m => m.Framework)
                    .Returns(nonChromeValue)
                    .Verifiable();

                Assert.IsFalse(Rule.Condition.Matches(_elementMock.Object));
            }

            _elementMock.Verify(m => m.Framework, Times.Exactly(nonChromeValues.Count()));
        }

        [TestMethod]
        public void Condition_FrameworkIsChrome_ReturnsTrue()
        {
            _elementMock.Setup(m => m.Framework).Returns(FrameworkId.Chrome).Verifiable();

            Assert.IsTrue(Rule.Condition.Matches(_elementMock.Object));

            _elementMock.Verify(m => m.Framework, Times.Once());
        }

        [TestMethod]
        public void PassesTest_ReturnsFalse()
        {
            Assert.IsFalse(Rule.PassesTest(_elementMock.Object));
        }


        [TestMethod]
        public void IncludeInResults_ParentFrameworkIsNotChrome_ReturnsTrue()
        {
            string[] nonChromeValues = { FrameworkId.DirectUI, FrameworkId.InternetExplorer, FrameworkId.WinForm, FrameworkId.WPF, FrameworkId.XAML, FrameworkId.Win32, FrameworkId.Edge, "NotChrome" };

            foreach (string nonChromeValue in nonChromeValues)
            {
                _elementMock.Setup(m => m.Parent).Returns(_elementParentMock.Object).Verifiable();
                _elementParentMock.Setup(m => m.Framework).Returns(nonChromeValue).Verifiable();

                Assert.IsTrue(Rule.IncludeInResults(_elementMock.Object));
            }

            _elementMock.Verify(m => m.Parent, Times.Exactly(nonChromeValues.Length * 2));
            _elementParentMock.Verify(m => m.Framework, Times.Exactly(nonChromeValues.Length));
        }

        [TestMethod]
        public void IncludeInResults_ParentFrameworkIsChrome_ReturnsFalse()
        {
            _elementMock.Setup(m => m.Parent).Returns(_elementParentMock.Object).Verifiable();
            _elementParentMock.Setup(m => m.Framework).Returns(FrameworkId.Chrome).Verifiable();

            Assert.IsFalse(Rule.IncludeInResults(_elementMock.Object));

            _elementMock.Verify(m => m.Parent, Times.Exactly(2));
            _elementParentMock.Verify(m => m.Framework, Times.Once());
        }
    }
}
