// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    [TestCategory("Axe.Windows.Rules")]
    public class ControlShouldSupportSetInfoXAMLTest
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.ControlShouldSupportSetInfoXAML();

        [TestMethod]
        public void TestControlShouldSupportSetInfoXAMLNoPositionInSetPropertyFail()
        {
            var e = new MockA11yElement();
            e.SizeOfSet = 1;
            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void TestControlShouldSupportSetInfoXAMLNoSizeOfSetPropertyFail()
        {
            var e = new MockA11yElement();
            e.PositionInSet = 1;
            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void TestControlShouldSupportSetInfoXAMLBothNonExistFail()
        {
            var e = new MockA11yElement();
            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void TestControlShouldSupportSetInfoXAMLBothExistPass()
        {
            var e = new MockA11yElement();
            e.PositionInSet = 1;
            e.SizeOfSet = 1;
            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void TestControlShouldSupportSetInfoXAMLExpectedControlTypes()
        {
            int[] expectedControlTypes = { ControlType.ListItem, ControlType.TreeItem };
            IEnumerable<int> unexpectedControlTypes = ControlType.All.Difference(expectedControlTypes);

            var e = new MockA11yElement();
            e.Framework = FrameworkId.XAML;

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
        public void TestControlShouldSupportSetInfoXAMLExpectedPlatform()
        {
            string[] expectedFrameworks = { FrameworkId.XAML };
            string[] unexpectedFrameworks = { FrameworkId.DirectUI, FrameworkId.Edge, FrameworkId.InternetExplorer, FrameworkId.Win32, FrameworkId.WinForm, FrameworkId.WPF };

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
