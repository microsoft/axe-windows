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
        private Mock<IA11yElement> _parentMock;

        [TestInitialize]
        public void BeforeEach()
        {
            _elementMock = new Mock<IA11yElement>(MockBehavior.Strict);
            _parentMock = new Mock<IA11yElement>(MockBehavior.Strict);
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
            _elementMock.Verify(m => m.Framework, Times.Exactly(nonChromeValues.Count()));
        }

        [TestMethod]
        public void Condition_FrameworkIsChrome_ControlTypeIsNotDocument_NoParent_ReturnsFalse()
        {
            IEnumerable<int> nonDocumentTypes = ControlType.All.Except(new int[] { ControlType.Document });
            _elementMock.Setup(m => m.Framework).Returns(FrameworkId.Chrome).Verifiable();
            _elementMock.Setup(m => m.Parent).Returns<IA11yElement>(null).Verifiable();

            foreach (int nonDocumentType in nonDocumentTypes)
            {
                _elementMock.Setup(m => m.ControlTypeId)
                    .Returns(nonDocumentType)
                    .Verifiable();

                Assert.IsFalse(Rule.Condition.Matches(_elementMock.Object));
            }

            _elementMock.Verify(m => m.Framework, Times.Exactly(nonDocumentTypes.Count()));
            _elementMock.Verify(m => m.ControlTypeId, Times.Exactly(nonDocumentTypes.Count()));
            _elementMock.Verify(m => m.Parent, Times.Exactly(nonDocumentTypes.Count()));
        }

        [TestMethod]
        public void Condition_FrameworkIsChrome_ParentControlTypeIsNotDocument_ReturnsFalse()
        {
            IEnumerable<int> nonDocumentTypes = ControlType.All.Except(new int[] { ControlType.Document });
            _elementMock.Setup(m => m.ControlTypeId).Returns(ControlType.Window).Verifiable();  // Arbitrary type
            _elementMock.Setup(m => m.Framework).Returns(FrameworkId.Chrome).Verifiable();
            _elementMock.Setup(m => m.Parent).Returns(_parentMock.Object).Verifiable();
            _parentMock.Setup(m => m.Parent).Returns<IA11yElement>(null).Verifiable();

            foreach (int nonDocumentType in nonDocumentTypes)
            {
                _parentMock.Setup(m => m.ControlTypeId)
                    .Returns(nonDocumentType)
                    .Verifiable();

                Assert.IsFalse(Rule.Condition.Matches(_elementMock.Object));
            }

            _elementMock.Verify(m => m.Framework, Times.Exactly(nonDocumentTypes.Count()));
            _elementMock.Verify(m => m.ControlTypeId, Times.Exactly(nonDocumentTypes.Count()));
            _elementMock.Verify(m => m.Parent, Times.Exactly(nonDocumentTypes.Count()));
            _parentMock.Verify(m => m.ControlTypeId, Times.Exactly(nonDocumentTypes.Count()));
            _parentMock.Verify(m => m.Parent, Times.Exactly(nonDocumentTypes.Count()));
        }

        [TestMethod]
        public void Condition_FrameworkIsChrome_ParentControlTypeIsDocument_ReturnsTrue()
        {
            _elementMock.Setup(m => m.Framework).Returns(FrameworkId.Chrome).Verifiable();
            _elementMock.Setup(m => m.ControlTypeId).Returns(ControlType.Window).Verifiable();  // Arbitrary value
            _elementMock.Setup(m => m.Parent).Returns(_parentMock.Object).Verifiable();
            _parentMock.Setup(m => m.ControlTypeId).Returns(ControlType.Document).Verifiable();

            Assert.IsTrue(Rule.Condition.Matches(_elementMock.Object));

            _elementMock.Verify(m => m.Framework, Times.Once());
            _elementMock.Verify(m => m.ControlTypeId, Times.Once());
            _elementMock.Verify(m => m.Parent, Times.Once());
            _parentMock.Verify(m => m.ControlTypeId, Times.Once());
        }

        [TestMethod]
        public void Condition_FrameworkIsChrome_ControlTypeIsDocument_ReturnsTrue()
        {
            _elementMock.Setup(m => m.Framework).Returns(FrameworkId.Chrome).Verifiable();
            _elementMock.Setup(m => m.ControlTypeId).Returns(ControlType.Document).Verifiable();

            Assert.IsTrue(Rule.Condition.Matches(_elementMock.Object));

            _elementMock.Verify(m => m.Framework, Times.Once());
            _elementMock.Verify(m => m.ControlTypeId, Times.Once());
        }

        [TestMethod]
        public void PassesTest_ReturnsFalse()
        {
            Assert.IsFalse(Rule.PassesTest(_elementMock.Object));
        }
    }
}
