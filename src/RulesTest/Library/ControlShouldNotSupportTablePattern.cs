// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using EvaluationCode = Axe.Windows.Rules.EvaluationCode;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class ControlShouldNotSupportTablePattern
    {
        private Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.ControlShouldNotSupportTablePattern();

        [TestMethod]
        public void NoTablePattern_Pass()
        {
            var e = new MockA11yElement();

            Assert.AreEqual(EvaluationCode.Pass, Rule.Evaluate(e));
        }

        [TestMethod]
        public void HasTablePattern_Error()
        {
            var e = new MockA11yElement();
            e.Patterns.Add(new A11yPattern(e, PatternType.UIA_TablePatternId));

            Assert.AreEqual(EvaluationCode.Error, Rule.Evaluate(e));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNull_Exception()
        {
            Rule.Evaluate(null);
        }

        [TestMethod]
        public void ConditionMatchesExpectedType()
        {
            int[] expectedTypes = { ControlType.List };
            var unexpectedTypes = ControlType.All.Except(expectedTypes);

            var e = new MockA11yElement();

            foreach (var type in expectedTypes)
            {
                e.ControlTypeId = type;
                Assert.IsTrue(Rule.Condition.Matches(e), $"expected ControlType: {type}");
            }

            foreach (var type in unexpectedTypes)
            {
                e.ControlTypeId = type;
                Assert.IsFalse(Rule.Condition.Matches(e), $"unexpected ControlType: {type}");
            }
        }

        [TestMethod]
        public void ConditionDoesNotMatchWin32List()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = ControlType.List;
            e.ClassName = "SysListView32";

            Assert.IsFalse(Rule.Condition.Matches(e));
        }
    } // class
} // namespace
