// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class LocalizedControlTypeIsNotEmptyTests
    {
        private static readonly Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.LocalizedControlTypeIsNotEmpty();

        [TestMethod]
        public void ConditionMismatch_IsKeyboardFocusableFalse_ReturnFalse()
        {
            var e = new MockA11yElement();
            e.IsKeyboardFocusable = false;

            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void IsKeyboardFocusableTrue_LocalizedControlTypeEmpty_ScanError()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = "";
            e.IsKeyboardFocusable = true;

            Assert.IsTrue(Rule.Condition.Matches(e));
            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void LocalizedControlTypeNotEmpty_ScanPass()
        {
            var e = new MockA11yElement();
            e.IsKeyboardFocusable = true;
            e.LocalizedControlType = "abc";

            Assert.IsTrue(Rule.PassesTest(e));
        }
    } // class
} // namespace
