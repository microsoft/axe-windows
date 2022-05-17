// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    [TestCategory("Axe.Windows.Rules")]
    public class ControlShouldNotSupportInvokePattern
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.ControlShouldNotSupportInvokePattern();

        private static readonly int[] ApplicableControlTypes =
        {
            Core.Types.ControlType.UIA_TabItemControlTypeId,
            Core.Types.ControlType.UIA_AppBarControlTypeId,
        };
        private static readonly IEnumerable<int> NonApplicableControlTypes = ControlType.All.Difference(ApplicableControlTypes);

        [TestMethod]
        public void Condition_ApplicableTypes_ReturnsTrue()
        {
            foreach (int controlType in ApplicableControlTypes)
            {
                var e = new MockA11yElement();
                e.ControlTypeId = controlType;
                e.Framework = FrameworkId.Win32;

                Assert.IsTrue(Rule.Condition.Matches(e));
            }
        }

        public void Condition_NonApplicableTypes_ReturnsFalse()
        {
            foreach (int controlType in NonApplicableControlTypes)
            {
                var e = new MockA11yElement();
                e.ControlTypeId = controlType;
                e.Framework = FrameworkId.Win32;

                Assert.IsFalse(Rule.Condition.Matches(e));
            }
        }
    }
}
