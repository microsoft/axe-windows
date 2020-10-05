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
    public class ControlShouldSupportSetInfoWPFTest
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.ControlShouldSupportSetInfoWPF();

        [TestMethod]
        public void TestControlShouldSupportSetInfoWPFNoPositionInSetPropertyFail()
        {
            var e = new MockA11yElement();
            e.SizeOfSet = 1;
            Assert.AreEqual(EvaluationCode.Warning, Rule.Evaluate(e));
        }

        [TestMethod]
        public void TestControlShouldSupportSetInfoWPFNoSizeOfSetPropertyFail()
        {
            var e = new MockA11yElement();
            e.PositionInSet = 1;
            Assert.AreEqual(EvaluationCode.Warning, Rule.Evaluate(e));
        }

        [TestMethod]
        public void TestControlShouldSupportSetInfoWPFBothNonExistFail()
        {
            var e = new MockA11yElement();
            Assert.AreEqual(EvaluationCode.Warning, Rule.Evaluate(e));
        }

        [TestMethod]
        public void TestControlShouldSupportSetInfoWPFBothExistPass()
        {
            var e = new MockA11yElement();
            e.PositionInSet = 1;
            e.SizeOfSet = 1;
            Assert.AreEqual(EvaluationCode.Pass, Rule.Evaluate(e));
        }

        [TestMethod]
        public void TestControlShouldSupportSetInfoWPFExpectedControlTypes()
        {
            int[] expectedControlTypes = { ControlType.ListItem, ControlType.TreeItem };
            IEnumerable<int> unexpectedControlTypes = ControlType.All.Difference(expectedControlTypes);

            var e = new MockA11yElement();
            e.Framework = Framework.WPF;

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
            string[] expectedFrameworks = { Framework.WPF };
            string[] unexpectedFrameworks = { Framework.DirectUI, Framework.Edge, Framework.InternetExplorer, Framework.Win32, Framework.WinForm, Framework.XAML };

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
