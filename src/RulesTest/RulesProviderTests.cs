// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Enums;
using Axe.Windows.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data;
using System.Linq;

namespace Axe.Windows.RulesTests
{
    [TestClass]
    public class RulesProviderTests
    {
        private Mock<IRule> _exclusionRuleMock;
        private Mock<IRule> _includedRuleMock;
        private Mock<IRuleFactory> _factoryMock;

        [TestInitialize]
        public void BeforeEach()
        {
            _exclusionRuleMock = new Mock<IRule>(MockBehavior.Strict);
            _exclusionRuleMock.Setup(r => r.Exclusionary).Returns(true);
            _includedRuleMock = new Mock<IRule>(MockBehavior.Strict);
            _includedRuleMock.Setup(r => r.Exclusionary).Returns(false);
            _factoryMock = new Mock<IRuleFactory>(MockBehavior.Strict);
            _factoryMock.Setup(o => o.CreateRule(It.IsAny<RuleId>())).Returns(_includedRuleMock.Object);
            _factoryMock.Setup(o => o.CreateRule(RuleId.NameNotNull)).Returns(_exclusionRuleMock.Object);
        }

        [TestMethod]
        public void RulesAreCreatedOnlyOnce()
        {
            var provider = new RuleProvider(_factoryMock.Object);
            for (int i = 0; i < 5; ++i)
            {
                provider.GetRule(RuleId.NameNotNull);
                _factoryMock.Verify(o => o.CreateRule(RuleId.NameNotNull), Times.Once());
            }
        }

        [TestMethod]
        public void RulesAreCreatedOnlyOnceForAll()
        {
            var provider = new RuleProvider(_factoryMock.Object);
            for (int i = 0; i < 5; ++i)
            {
                provider.All.GetEnumerator();
                _factoryMock.Verify(o => o.CreateRule(RuleId.NameNotNull), Times.Once());
            }
        }

        [TestMethod]
        public void ExclusionRulesContainsOnlyExclusionRules()
        {
            var provider = new RuleProvider(_factoryMock.Object);
            var actualExclusionRules = provider.ExclusionRules;
            Assert.AreEqual(1, actualExclusionRules.Count());
            Assert.AreEqual(_exclusionRuleMock.Object, actualExclusionRules.FirstOrDefault());
        }

        [TestMethod]
        public void InclusionRulesContainsOnlyInclusionRules()
        {
            var provider = new RuleProvider(_factoryMock.Object);
            var actualIncludedRules = provider.IncludedRules;
            Assert.AreEqual(provider.All.Count() - 1, actualIncludedRules.Count());
            Assert.AreEqual(_includedRuleMock.Object, actualIncludedRules.FirstOrDefault());
        }

        [TestMethod]
        public void AllRulesContainsExclusionAndInclusionRules()
        {
            var provider = new RuleProvider(_factoryMock.Object);
            var actualAllRules = provider.All;
            var excludedResult = actualAllRules.Where(r => r.Exclusionary);
            Assert.AreEqual(1, excludedResult.Count());
            Assert.AreEqual(_exclusionRuleMock.Object, excludedResult.FirstOrDefault());
            var includedResult = actualAllRules.Where(r => !r.Exclusionary);
            Assert.AreEqual(actualAllRules.Count() - 1, includedResult.Count());
            Assert.AreEqual(_includedRuleMock.Object, includedResult.FirstOrDefault());
        }
    } // class
} // namespace
