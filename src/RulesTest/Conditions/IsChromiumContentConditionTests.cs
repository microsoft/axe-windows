// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.RulesTests.Conditions
{
    [TestClass]
    public class IsChromiumContentConditionTests
    {
        private static readonly Condition Condition = new IsChromiumContentCondition();

        private Mock<IA11yElement> _elementMock;

        [TestInitialize]
        public void BeforeEach()
        {
            _elementMock = new Mock<IA11yElement>(MockBehavior.Strict);
        }

        [TestMethod]
        public void Matches_FrameworkIsNotChrome_ParentIsNull_ReturnsFalse()
        {
            _elementMock.Setup(m => m.Parent)
                .Returns(null as IA11yElement)
                .Verifiable();

            IEnumerable<string> nonChromeValues = Extensions.GetFrameworkIds().Append("NotChrome").Except(new string[] { FrameworkId.Chrome });
            foreach (string nonChromeValue in nonChromeValues)
            {
                _elementMock.Setup(m => m.Framework)
                    .Returns(nonChromeValue)
                    .Verifiable();

                Assert.IsFalse(Condition.Matches(_elementMock.Object));
            }

            _elementMock.Verify(m => m.Framework, Times.Exactly(nonChromeValues.Count()));
            _elementMock.Verify(m => m.Parent, Times.Exactly(nonChromeValues.Count()));
        }

        [TestMethod]
        public void Matches_FrameworkIsChrome_IsNotDocument_ParentIsNull_ReturnsFalse()
        {
            _elementMock.Setup(m => m.Framework)
                .Returns(FrameworkId.Chrome)
                .Verifiable();
            _elementMock.Setup(m => m.ControlTypeId)
                .Returns(ControlType.Pane)  // Arbitrary type
                .Verifiable();
            _elementMock.Setup(m => m.Parent)
                .Returns(null as IA11yElement)
                .Verifiable();

            Assert.IsFalse(Condition.Matches(_elementMock.Object));

            _elementMock.VerifyAll();
        }

        [TestMethod]
        public void Matches_FrameworkIsChrome_IsDocument_ReturnsTrue()
        {
            _elementMock.Setup(m => m.Framework)
                .Returns(FrameworkId.Chrome)
                .Verifiable();
            _elementMock.Setup(m => m.ControlTypeId)
                .Returns(ControlType.Document)
                .Verifiable();

            Assert.IsTrue(Condition.Matches(_elementMock.Object));

            _elementMock.VerifyAll();
        }

        public void Matches_FrameworkIsChrome_IsNotDocument_ParentIsChromeDocument_ReturnsTrue()
        {
            Mock<IA11yElement> parentMock = new Mock<IA11yElement>(MockBehavior.Strict);
            parentMock.Setup(m => m.Framework)
                .Returns(FrameworkId.Chrome)
                .Verifiable();
            parentMock.Setup(m => m.ControlTypeId)
                .Returns(ControlType.Document)
                .Verifiable();

            _elementMock.Setup(m => m.Framework)
                .Returns(FrameworkId.Chrome)
                .Verifiable();
            _elementMock.Setup(m => m.ControlTypeId)
                .Returns(ControlType.Edit)
                .Verifiable();
            _elementMock.Setup(m => m.Parent)
                .Returns(parentMock.Object)
                .Verifiable();

            Assert.IsTrue(Condition.Matches(_elementMock.Object));

            _elementMock.VerifyAll();
            parentMock.VerifyAll();
        }

        [TestMethod]
        public void Matches_FrameworkIsChrome_IsNotDocument_GrandparentIsChromeDocument_ReturnsTrue()
        {
            Mock<IA11yElement> grandparentMock = new Mock<IA11yElement>(MockBehavior.Strict);
            grandparentMock.Setup(m => m.Framework)
                .Returns(FrameworkId.Chrome)
                .Verifiable();
            grandparentMock.Setup(m => m.ControlTypeId)
                .Returns(ControlType.Document)
                .Verifiable();

            Mock<IA11yElement> parentMock = new Mock<IA11yElement>(MockBehavior.Strict);
            parentMock.Setup(m => m.Framework)
                .Returns(FrameworkId.Chrome)
                .Verifiable();
            parentMock.Setup(m => m.ControlTypeId)
                .Returns(ControlType.Window)  // Arbitrary type
                .Verifiable();
            parentMock.Setup(m => m.Parent)
                .Returns(grandparentMock.Object)
                .Verifiable();

            _elementMock.Setup(m => m.Framework)
                .Returns(FrameworkId.Chrome)
                .Verifiable();
            _elementMock.Setup(m => m.ControlTypeId)
                .Returns(ControlType.Edit)  // Arbitrary type
                .Verifiable();
            _elementMock.Setup(m => m.Parent)
                .Returns(parentMock.Object)
                .Verifiable();

            Assert.IsTrue(Condition.Matches(_elementMock.Object));

            _elementMock.VerifyAll();
            parentMock.VerifyAll();
            grandparentMock.VerifyAll();
        }

        [TestMethod]
        public void TestNotFalse()
        {
            var test = new NotCondition(Condition.False);
            Assert.IsTrue(test.Matches(null));
        }
    } // class
} // namespace
