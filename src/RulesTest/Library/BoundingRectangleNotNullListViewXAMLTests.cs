// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using static Axe.Windows.RulesTests.ControlType;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class BoundingRectangleNotNullListViewXAMLTests
    {
        private static readonly Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.BoundingRectangleNotNullListViewXAML();

        [TestMethod]
        public void FrameworkIssueLink_IsNotNull()
        {
            Assert.IsNotNull(Rule.Info.FrameworkIssueLink);
        }

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
            {
                // ElementGrouis.ListXAML is tested externally--keep it simple here
                Assert.IsFalse(Rule.Condition.Matches(e));

                // Make ListXAML return true
                e.Framework = "XAML";
                e.ControlTypeId = List;
                Assert.IsTrue(Rule.Condition.Matches(e));

                e.IsOffScreen = true;
                Assert.IsFalse(Rule.Condition.Matches(e));

                // Make ListXAML return false
                e.Framework = "Something else";
                Assert.IsFalse(Rule.Condition.Matches(e));
            } // using
        }
    } // class
} // namespace
