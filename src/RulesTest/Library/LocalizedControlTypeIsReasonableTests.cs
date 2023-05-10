// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Rules.PropertyConditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Axe.Windows.RulesTests.ControlType;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class LocalizedControlTypeIsReasonableTests
    {
        private static readonly Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.LocalizedControlTypeIsReasonable();

        [TestMethod]
        [Timeout(1000)]
        public void Condition_IsNotEnglish_Returns_False()
        {
            SystemProperties.OverriddenISOLanguageName = "spa";

            bool matches = Rule.Condition.Matches(null);

            SystemProperties.OverriddenISOLanguageName = null;

            Assert.IsFalse(matches);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Condition_IsEnglish_Returns_True()
        {
            SystemProperties.OverriddenISOLanguageName = "eng";

            var e = new MockA11yElement();
            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_AppBarControlTypeId;
            e.LocalizedControlType = "app bar";

            bool matches = Rule.Condition.Matches(e);

            SystemProperties.OverriddenISOLanguageName = null;

            Assert.IsTrue(matches);
        }

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
