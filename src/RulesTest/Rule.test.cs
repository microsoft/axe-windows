// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.RulesTests
{
    [RuleInfo]
    internal class ConcreteRule : Rules.Rule
    {
        public override bool PassesTest(IA11yElement element)
        {
            return true;
        }

        protected override Condition CreateCondition()
        {
            return Condition.True;
        }
    }

    [TestClass]
    public class Rule
    {
        private static readonly Rules.IRule ConcreteRule = new ConcreteRule();

        [TestMethod]
        public void RulesAreNotExclusionaryByDefault()
        {
            Assert.IsFalse(ConcreteRule.Exclusionary);
        }

        [TestMethod]
        public void RulesAreIncludedInResultsByDefault()
        {
            Assert.IsTrue(ConcreteRule.IncludeInResults(null));
        }
    } // class
} // namespace
