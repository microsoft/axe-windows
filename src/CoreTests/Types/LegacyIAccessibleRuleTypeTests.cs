// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Axe.Windows.CoreTests.Types
{
    [TestClass()]
    public class LegacyIAccessibleRuleTypeTests
    {
        [TestMethod()]
        [Timeout(1000)]
        public void CheckExists()
        {
            Assert.AreEqual(true, LegacyIAccessibleRoleType.GetInstance().Exists(LegacyIAccessibleRoleType.ROLE_SYSTEM_OUTLINEBUTTON));
            Assert.AreEqual(false, LegacyIAccessibleRoleType.GetInstance().Exists(0));
        }

        [TestMethod()]
        [Timeout(1000)]
        public void CheckGetNameById()
        {
            Assert.AreEqual("ROLE_SYSTEM_OUTLINEBUTTON(64)", LegacyIAccessibleRoleType.GetInstance().GetNameById(LegacyIAccessibleRoleType.ROLE_SYSTEM_OUTLINEBUTTON));
        }

        [TestMethod()]
        [Timeout(1000)]
        public void CheckHasExpectedValues()
        {
            var values = LegacyIAccessibleRoleType.GetInstance().Values;
            var minValue = LegacyIAccessibleRoleType.ROLE_SYSTEM_TITLEBAR;
            var maxValue = LegacyIAccessibleRoleType.ROLE_SYSTEM_OUTLINEBUTTON;

            // Adding 1 because the true count of objects is max + 1 - min.
            Assert.AreEqual(values.Count(), maxValue + 1 - minValue);

            for (int i = minValue; i <= maxValue; ++i)
            {
                Assert.IsTrue(values.Contains(i), $"{nameof(ControlType)} does not contain the expected value {i}");
            }
        }
    }
}
