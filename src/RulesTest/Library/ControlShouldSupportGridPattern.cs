// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axe.Windows.Core.Bases;
using EvaluationCode = Axe.Windows.Rules.EvaluationCode;
using Axe.Windows.Core.Types;
using System.Diagnostics;
using UIAutomationClient;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class ControlShouldSupportGridPattern
    {
        private Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.ControlShouldSupportGridPattern();

        [TestMethod]
        public void HasGridPattern_Pass()
        {
            var e = new MockA11yElement();
            e.Patterns.Add(new A11yPattern(e, PatternType.UIA_GridPatternId));

            Assert.AreEqual(EvaluationCode.Pass, Rule.Evaluate(e));
        }

        [TestMethod]
        public void NoGridPattern_Error()
        {
            var e = new MockA11yElement();

            Assert.AreEqual(EvaluationCode.Error, Rule.Evaluate(e));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNull_Exception()
        {
            Rule.Evaluate(null);
        }

        /// <summary>
        /// Pass
        /// A Table supports Grid pattern
        /// </summary>
        [TestMethod]
        public void TableWithGridPattern_Pass()
        {
            var e = new MockA11yElement();

            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_TableControlTypeId;

            e.Patterns.Add(new Core.Bases.A11yPattern(e, PatternType.UIA_GridPatternId));

            Assert.IsTrue(this.Rule.Condition.Matches(e));
            Assert.AreEqual(EvaluationCode.Pass, this.Rule.Evaluate(e));

            Assert.AreEqual(PropertyType.UIA_IsGridPatternAvailablePropertyId, this.Rule.Info.PropertyID);
        }

        /// <summary>
        /// Fail
        /// A Table does not support Grid pattern
        /// </summary>
        [TestMethod]
        public void TableWithoutGridPattern_Error()
        {
            var e = new MockA11yElement();

            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_TableControlTypeId;

            Assert.IsTrue(this.Rule.Condition.Matches(e));
            Assert.AreEqual(EvaluationCode.Error, this.Rule.Evaluate(e));

            Assert.AreEqual(PropertyType.UIA_IsGridPatternAvailablePropertyId, this.Rule.Info.PropertyID);
        }

    } // class
} // namespace
