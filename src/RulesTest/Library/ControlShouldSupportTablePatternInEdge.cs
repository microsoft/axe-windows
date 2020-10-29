// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.RulesTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EvaluationCode = Axe.Windows.Rules.EvaluationCode;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class ControlShouldSupportTablePatternInEdge
    {
        private Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.ControlShouldSupportTablePatternInEdge();

        [TestMethod]
        public void HasTablePattern_Pass()
        {
            var e = new MockA11yElement();
            e.Patterns.Add(new A11yPattern(e, PatternType.UIA_TablePatternId));

            Assert.AreEqual(EvaluationCode.Pass, Rule.Evaluate(e));
        }

        [TestMethod]
        public void NoTablePattern_Warning()
        {
            var e = new MockA11yElement();

            Assert.AreEqual(EvaluationCode.Warning, Rule.Evaluate(e));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNull_Exception()
        {
            Rule.Evaluate(null);
        }

        [TestMethod]
        public void CheckPropertyIdIsSet()
        {
            Assert.AreEqual(PropertyType.UIA_IsTablePatternAvailablePropertyId, this.Rule.Info.PropertyID);
        }

        [TestMethod]
        public void ConditionMatchesExpectedElement()
        {
            int[] expectedTypes = { ControlType.Calendar, ControlType.Table };
            var unexpectedTypes = ControlType.All.Difference(expectedTypes);
            
            var e = new MockA11yElement();
            e.Framework = Framework.Edge;

            foreach (var type in expectedTypes)
            {
                e.ControlTypeId = type;
                Assert.IsTrue(Rule.Condition.Matches(e));
            }

            foreach (var type in unexpectedTypes)
            {
                e.ControlTypeId = type;
                Assert.IsFalse(Rule.Condition.Matches(e));
            }

            e.ControlTypeId = expectedTypes[0];
            e.Framework = null;

            Assert.IsFalse(Rule.Condition.Matches(e));
        }
    } // class
} // namespace
