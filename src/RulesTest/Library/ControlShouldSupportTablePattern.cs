// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class ControlShouldSupportTablePattern
    {
        private Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.ControlShouldSupportTablePattern();

        [TestMethod]
        public void HasTablePattern_Pass()
        {
            var e = new MockA11yElement();
            e.Patterns.Add(new A11yPattern(e, PatternType.UIA_TablePatternId));

            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void NoTablePattern_Error()
        {
            var e = new MockA11yElement();

            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNull_Exception()
        {
            Rule.PassesTest(null);
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
        }
    } // class
} // namespace
