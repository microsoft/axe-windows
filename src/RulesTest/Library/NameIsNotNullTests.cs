// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using static Axe.Windows.RulesTests.ControlType;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class NameIsNotNullTests
    {
        private static readonly Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.NameIsNotNull();

        [TestMethod]
        public void TestNameNull()
        {
            var e = new MockA11yElement();
            e.Name = null;

            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void TestNameNotNull()
        {
            var e = new MockA11yElement();
            e.Name = "";

            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void TestNameIsNotNullProgressBar()
        {
            var e = new MockA11yElement();
            e.IsKeyboardFocusable = false;
            e.ControlTypeId = CheckBox;
            e.BoundingRectangle = new Rectangle(0, 0, 25, 25);
            Assert.IsFalse(Rule.Condition.Matches(e));

            e.ControlTypeId = ProgressBar;
            Assert.IsTrue(Rule.Condition.Matches(e));
        }
    } // class
} // namespace
