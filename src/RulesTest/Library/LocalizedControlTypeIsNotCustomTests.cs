// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class LocalizedControlTypeIsNotCustomTests
    {
        private static readonly Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.LocalizedControlTypeIsNotCustom();

        private MockA11yElement CreateElementExpectedToMatchCondition()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_CustomControlTypeId;
            e.LocalizedControlType = "non-empty string";
            e.IsKeyboardFocusable = true;

            return e;
        }

        [TestMethod]
        public void ExpectedElementMatches()
        {
            var e = CreateElementExpectedToMatchCondition();

            Assert.IsTrue(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void ElementNotCustomControlType()
        {
            var e = CreateElementExpectedToMatchCondition();

            int[] custom = { ControlType.Custom };
            foreach (var ct in ControlType.All.
                Except(custom))
            {
                e.ControlTypeId = ct;
                Assert.IsFalse(Rule.Condition.Matches(e));
            }
        }

        [TestMethod]
        public void ElementNotFocusable()
        {
            var e = CreateElementExpectedToMatchCondition();
            e.IsKeyboardFocusable = false;

            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void ElementLocalizedControlTypeEmpty()
        {
            var e = CreateElementExpectedToMatchCondition();
            e.LocalizedControlType = string.Empty;

            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void ElementLocalizedControlTypeNull()
        {
            var e = CreateElementExpectedToMatchCondition();
            e.LocalizedControlType = null;

            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void ElementIsDataGridDetailsPresenter()
        {
            var parent = new MockA11yElement();
            parent.ControlTypeId = ControlType.DataItem;

            var e = CreateElementExpectedToMatchCondition();
            e.ClassName = "DataGridDetailsPresenter";
            e.Parent = parent;

            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void ElementIsWPFDataGridCell()
        {
            var e = CreateElementExpectedToMatchCondition();
            e.ClassName = "DataGridCell";
            e.Framework = Core.Enums.FrameworkId.WPF;

            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void LocalizedControlTypeIsCustom()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = "custom";

            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void LocalizedControlTypeIsNotCustomTest()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = "not custom";

            Assert.IsTrue(Rule.PassesTest(e));
        }
    } // class
} // LocalizedControlTypespace
