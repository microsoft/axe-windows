// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.RulesTests.Conditions
{
    [TestClass]
    public class ControlTypeConditionTests
    {
        [TestMethod]
        public void TestMatchingControlTypes()
        {
            using (var e = new MockA11yElement())
            {
                e.ControlTypeId = ControlType.Button;
                var test = new ControlTypeCondition(ControlType.Button);
                Assert.IsTrue(test.Matches(e));
            } // using
        }

        [TestMethod]
        public void TestNonMatchingControlTypes()
        {
            using (var e = new MockA11yElement())
            {
                e.ControlTypeId = ControlType.CheckBox;
                var test = new ControlTypeCondition(ControlType.Button);
                Assert.IsFalse(test.Matches(e));
            } // using
        }
    } // class
} // namespace
