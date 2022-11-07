// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Axe.Windows.RulesTests.ControlType;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class LocalizedControlTypeIsReasonableTests
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.LocalizedControlTypeIsReasonable();

        [TestMethod]
        public void TestControlTypeIdIsAppBarAndLocalizedControlTypeIsAppBar()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_AppBarControlTypeId;
            e.LocalizedControlType = "app bar";

            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void TestControlTypeIdIsAppBarAndLocalizedControlTypeIsNotAppBar()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_AppBarControlTypeId;
            e.LocalizedControlType = "custom";

            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void TestControlTypeIdIsCustomControl()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_CustomControlTypeId;
            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void LocalizedControlTypeIsReasonable_True_MultipleOptions()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = Hyperlink;
            e.LocalizedControlType = "hyperlink";
            Assert.IsTrue(Rule.PassesTest(e));

            e.LocalizedControlType = "link";
            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void LocalizedControlTypeIsReasonable_True_CaseInsensitive()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = Hyperlink;
            e.LocalizedControlType = "Hyperlink";
            Assert.IsTrue(Rule.PassesTest(e));
        }
    } // class
} // LocalizedControlTypespace
