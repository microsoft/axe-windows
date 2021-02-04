// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class LocalizedControlTypeIsNotCustomWPFGridCell
    {
        private Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.LocalizedControlTypeIsNotCustomWPFGridCell();

        private MockA11yElement CreateElementExpectedToMatchCondition()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_CustomControlTypeId;
            e.LocalizedControlType = "non-empty string";
            e.IsKeyboardFocusable = true;
            e.Framework = Core.Enums.FrameworkId.WPF;
            e.ClassName = "DataGridCell";

            return e;
        }

        [TestMethod]
        public void ExpectedElementMatches()
        {
            var e = CreateElementExpectedToMatchCondition();

            Assert.IsTrue(this.Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void ElementNotCustomControlType()
        {
            var e = CreateElementExpectedToMatchCondition();

            int[] custom = { ControlType.Custom };
            foreach (var ct in ControlType.All.Except(custom))
            {
                e.ControlTypeId = ct;
                Assert.IsFalse(this.Rule.Condition.Matches(e));
            }
        }

        [TestMethod]
        public void ElementNotFocusable()
        {
            var e = CreateElementExpectedToMatchCondition();
            e.IsKeyboardFocusable = false;

            Assert.IsFalse(this.Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void ElementLocalizedControlTypeEmpty()
        {
            var e = CreateElementExpectedToMatchCondition();
            e.LocalizedControlType = string.Empty;

            Assert.IsFalse(this.Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void ElementLocalizedControlTypeNull()
        {
            var e = CreateElementExpectedToMatchCondition();
            e.LocalizedControlType = null;

            Assert.IsFalse(this.Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void ElementClassNameDoesNotMatch()
        {
            var e = CreateElementExpectedToMatchCondition();
            e.ClassName = string.Empty;

            Assert.IsFalse(this.Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void ElementFrameworkDoesNotMatch()
        {
            var e = CreateElementExpectedToMatchCondition();
            e.Framework = string.Empty;

            Assert.IsFalse(this.Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void LocalizedControlTypeIsCustom()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = "custom";

            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void LocalizedControlTypeIsNotCustom()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = "not custom";

            Assert.IsTrue(Rule.PassesTest(e));
        }
    } // class
} // LocalizedControlTypespace
