// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using EvaluationCode = Axe.Windows.Rules.EvaluationCode;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    [TestCategory("Axe.Windows.Rules")]
    public class ControlShouldNotSupportTogglePattern
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.ControlShouldNotSupportTogglePattern();

        private static readonly int[] ExpectedControlTypes = { ControlType.RadioButton };
        private static readonly IEnumerable<int> UnexpectedControlTypes = ControlType.All.Difference(ExpectedControlTypes);

        [TestMethod]
        public void IsExpectedControlType_Condition_True()
        {
            var e = new MockA11yElement();

            foreach (var controlType in ExpectedControlTypes)
            {
                e.ControlTypeId = controlType;
                Assert.IsTrue(Rule.Condition.Matches(e));
            } // for each control type
        }

        [TestMethod]
        public void IsUnexpectedControlType_Condition_False()
        {
            var e = new MockA11yElement();

            foreach (var controlType in UnexpectedControlTypes)
            {
                e.ControlTypeId = controlType;
                Assert.IsFalse(Rule.Condition.Matches(e));
            } // for each control type
        }

        [TestMethod]
        public void HasTogglePattern_EvaluateReturnsWarning()
        {
            var e = new MockA11yElement();
            e.Patterns.Add(new Core.Bases.A11yPattern(e, PatternType.UIA_TogglePatternId));

            Assert.IsFalse(Rule.PassesTest(e));
            Assert.AreEqual(EvaluationCode.Warning, Rule.Info.ErrorCode);
        }

        [TestMethod]
        public void DoesNotToggleScrollPattern_EvaluateReturnsPass()
        {
            var e = new MockA11yElement();

            Assert.IsTrue(Rule.PassesTest(e));
        }
    } // class
} // namespace