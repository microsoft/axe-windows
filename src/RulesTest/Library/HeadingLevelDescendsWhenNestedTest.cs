// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class HeadingLevelDescendsWhenNestedTest
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.HeadingLevelDescendsWhenNested();

        [TestMethod]
        public void TestHeadingLevelLowerBoundTrue()
        {
            using (var e = new MockA11yElement())
            {
                e.HeadingLevel = HeadingLevelType.HeadingLevel1;

                Assert.IsTrue(Rule.Condition.Matches(e));
            } // using
        }

        [TestMethod]
        public void TestHeadingLevelUpperBoundTrue()
        {
            using (var e = new MockA11yElement())
            {
                e.HeadingLevel = HeadingLevelType.HeadingLevel9;

                Assert.IsTrue(Rule.Condition.Matches(e));
            } // using
        }

        [TestMethod]
        public void TestHeadingLevelOutsideLowerBoundFalse()
        {
            using (var e = new MockA11yElement())
            {
                e.HeadingLevel = HeadingLevelType.HeadingLevel1 - 1;

                Assert.IsFalse(Rule.Condition.Matches(e));
            } // using
        }

        [TestMethod]
        public void TestHeadingLevelOutsideUpperBoundFalse()
        {
            using (var e = new MockA11yElement())
            {
                e.HeadingLevel = HeadingLevelType.HeadingLevel9 + 1;

                Assert.IsFalse(Rule.Condition.Matches(e));
            } // using
        }

        [TestMethod]
        public void TestHeadingLevelDescendsWhenNestedPass()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                e.HeadingLevel = HeadingLevelType.HeadingLevel2;
                parent.HeadingLevel = HeadingLevelType.HeadingLevel1;
                e.Parent = parent;

                Assert.IsTrue(Rule.PassesTest(e));
            } // using
        }

        [TestMethod]
        public void TestHeadingLevelDescendsWhenNestedError()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                e.HeadingLevel = HeadingLevelType.HeadingLevel8;
                parent.HeadingLevel = HeadingLevelType.HeadingLevel9;
                e.Parent = parent;

                Assert.IsFalse(Rule.PassesTest(e));
            } // using
        }
    } // class
} // namespace
