// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using static Axe.Windows.RulesTests.ControlType;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class NameIsEmptyButElementIsNotKeyboardFocusableTests
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.NameIsEmptyButElementIsNotKeyboardFocusable();

        [TestMethod]
        public void TestNameIsEmptyButElementIsNotKeyboardFocusablePass()
        {
            using (var e = new MockA11yElement())
            {
                e.Name = "Bob";
                Assert.IsTrue(Rule.PassesTest(e));
            } // using
        }

        [TestMethod]
        public void TestNameIsEmptyButElementIsNotKeyboardFocusableOpen()
        {
            using (var e = new MockA11yElement())
            {
                e.Name = "";
                Assert.IsFalse(Rule.PassesTest(e));
            } // using
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNameIsEmptyButElementIsNotKeyboardFocusableNullArgument()
        {
            Rule.PassesTest(null);
        }

        [TestMethod]
        public void TestNameIsEmptyButElementIsNotKeyboardFocusableProgressBar()
        {
            // progress bars are processed by NameIsNotEmpty

            var e = new MockA11yElement();
            e.ControlTypeId = CheckBox;
            e.Name = "Bob";
            e.BoundingRectangle = new Rectangle(0, 0, 25, 25);
            Assert.IsTrue(Rule.Condition.Matches(e));

            e.ControlTypeId = ProgressBar;
            Assert.IsFalse(Rule.Condition.Matches(e));
        }
    } // class
} // namespace
