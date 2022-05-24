// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using static Axe.Windows.RulesTests.ControlType;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class BoundingRectangleNotNullTextBlockXAMLTests
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.BoundingRectangleNotNullTextBlockXAML();

        [TestMethod]
        public void TestBoundingRectangleNotNullPass()
        {
            using (var e = new MockA11yElement())
            {
                e.BoundingRectangle = Rectangle.Empty;
                Assert.IsTrue(Rule.PassesTest(e));
            } // using
        }

        [TestMethod]
        public void TestBoundingRectangleNotNullFail()
        {
            using (var e = new MockA11yElement())
            {
                Assert.IsFalse(Rule.PassesTest(e));
            } // using
        }

        [TestMethod]
        public void Condition_MatchesExpectation()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                // ElementGrouis.HyperlinkInTextXAML is tested externally--keep it simple here
                Assert.IsFalse(Rule.Condition.Matches(e));

                // Make HyperlinkInTextXAML return true
                e.Framework = "XAML";
                e.ControlTypeId = Hyperlink;
                e.Parent = parent;
                parent.ControlTypeId = Text;
                Assert.IsTrue(Rule.Condition.Matches(e));

                e.IsOffScreen = true;
                Assert.IsFalse(Rule.Condition.Matches(e));

                // Make HyperlinkInTextXAML return false
                e.Framework = "Something else";
                Assert.IsFalse(Rule.Condition.Matches(e));
            } // using
        }
    } // class
} // namespace
