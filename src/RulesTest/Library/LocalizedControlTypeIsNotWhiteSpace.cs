// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class LocalizedControlTypeIsNotWhiteSpace
    {
        private Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.LocalizedControlTypeIsNotWhiteSpace();

        [TestMethod]
        public void TestLocalizedControlTypeNull()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = null;
            Assert.IsFalse(this.Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void TestLocalizedControlTypeEmpty()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = "";
            Assert.IsFalse(this.Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void TestLocalizedControlTypeNotEmpty()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = " ";
            Assert.IsTrue(this.Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void TestLocalizedControlTypeWithOnlyWhiteSpace()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = "   ";
            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void TestLocalizedControlTypeWithVisibleCharacters()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = "hello world!";
            Assert.IsTrue(Rule.PassesTest(e));
        }
    } // class
} // LocalizedControlTypespace
