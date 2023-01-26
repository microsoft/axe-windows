// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    [TestCategory("Axe.Windows.Rules")]
    public class ControlShouldSupportSetInfoWPFTests
    {
        private static readonly Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.ControlShouldSupportSetInfoWPF();

        [TestMethod]
        public void FrameworkIssueLink_IsNotNull()
        {
            Assert.IsNotNull(Rule.Info.FrameworkIssueLink);
        }

        [TestMethod]
        public void TestControlShouldSupportSetInfoWPFNoPositionInSetPropertyFail()
        {
            var e = new MockA11yElement();
            e.SizeOfSet = 1;
            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void TestControlShouldSupportSetInfoWPFNoSizeOfSetPropertyFail()
        {
            var e = new MockA11yElement();
            e.PositionInSet = 1;
            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void TestControlShouldSupportSetInfoWPFBothNonExistFail()
        {
            var e = new MockA11yElement();
            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void TestControlShouldSupportSetInfoWPFBothExistPass()
        {
            var e = new MockA11yElement();
            e.PositionInSet = 1;
            e.SizeOfSet = 1;
            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void TestControlShouldSupportSetInfoWPFExpectedControlTypes()
        {
            int[] expectedControlTypes = { ControlType.ListItem, ControlType.TreeItem };
            IEnumerable<int> unexpectedControlTypes = ControlType.All.Difference(expectedControlTypes);

            var e = new MockA11yElement();
            e.Framework = FrameworkId.WPF;

            foreach (var ct in expectedControlTypes)
            {
                e.ControlTypeId = ct;
                Assert.IsTrue(Rule.Condition.Matches(e));
            }

            foreach (var ct in unexpectedControlTypes)
            {
                e.ControlTypeId = ct;
                Assert.IsFalse(Rule.Condition.Matches(e));
            }
        }

        [TestMethod]
        public void TestControlShouldSupportSetInfoWPFExpectedPlatform()
        {
            string[] expectedFrameworks = { FrameworkId.WPF };
            string[] unexpectedFrameworks = Extensions.GetFilteredFrameworkIds(new string[] { FrameworkId.WPF }, Array.Empty<string>());

            var e = new MockA11yElement();
            e.ControlTypeId = ControlType.ListItem;

            foreach (var framework in expectedFrameworks)
            {
                e.Framework = framework;
                Assert.IsTrue(Rule.Condition.Matches(e));
            }

            foreach (var framework in unexpectedFrameworks)
            {
                e.Framework = framework;
                Assert.IsFalse(Rule.Condition.Matches(e));
            }
        }
    }
}
