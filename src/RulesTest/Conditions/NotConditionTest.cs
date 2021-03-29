// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.RulesTests.Conditions
{
    [TestClass]
    public class NotConditionTest
    {
        [TestMethod]
        public void TestNotTrue()
        {
            var test = new NotCondition(Condition.True);
            Assert.IsFalse(test.Matches(null));
        }

        [TestMethod]
        public void TestNotFalse()
        {
            var test = new NotCondition(Condition.False);
            Assert.IsTrue(test.Matches(null));
        }
    } // class
} // namespace
