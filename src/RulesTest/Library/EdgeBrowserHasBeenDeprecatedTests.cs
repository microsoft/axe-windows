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
    public class EdgeBrowserHasBeenDeprecatedTests
    {
        private static readonly Rules.IRule Rule = new Rules.Library.EdgeBrowserHasBeenDeprecated(excludeChromiumContent: false);
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
            IEnumerable<string> nonEdgeValues = Extensions.GetFrameworkIds().Append("NotEdge").Except(new string[] { FrameworkId.Edge });

            foreach (string nonEdgeValue in nonEdgeValues)
            {
                _elementMock.Setup(m => m.Framework)
                    .Returns(nonEdgeValue)
                    .Verifiable();

                Assert.IsFalse(Rule.Condition.Matches(_elementMock.Object));
            }

            _elementMock.Verify(m => m.Framework, Times.Exactly(nonEdgeValues.Count()));
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
