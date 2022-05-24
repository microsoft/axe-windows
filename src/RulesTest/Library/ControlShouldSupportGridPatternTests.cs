// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class ControlShouldSupportGridPatternTests
    {
        private Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.ControlShouldSupportGridPattern();

        [TestMethod]
        public void HasGridPattern_Pass()
        {
            var e = new MockA11yElement();
            e.Patterns.Add(new A11yPattern(e, PatternType.UIA_GridPatternId));

            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void NoGridPattern_Error()
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
            Assert.AreEqual(PropertyType.UIA_IsGridPatternAvailablePropertyId, this.Rule.Info.PropertyID);
        }
    } // class
} // namespace
