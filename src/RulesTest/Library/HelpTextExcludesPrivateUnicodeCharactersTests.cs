// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class HelpTextExcludesPrivateUnicodeCharactersTests
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.HelpTextExcludesPrivateUnicodeCharacters();

        [TestMethod]
        public void HelpTextExcludesPrivateUnicodeCharacters_Pass()
        {
            var e = new MockA11yElement();
            e.HelpText = "Hello";

            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void HelpTextExcludesPrivateUnicodeCharacters_Faile()
        {
            var e = new MockA11yElement();
            e.HelpText = "Hello \uE000";

            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HelpTextExcludesPrivateUnicodeCharacters_NullElement_ThrowsArgumentNullException()
        {
            Rule.PassesTest(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HelpTextExcludesPrivateUnicodeCharacters_HelpTextNull_ThrowsArgumentException()
        {
            var e = new MockA11yElement();

            Rule.PassesTest(e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HelpTextExcludesPrivateUnicodeCharacters_HelpTextEmpty_ThrowsArgumentException()
        {
            var e = new MockA11yElement();
            e.HelpText = string.Empty;

            Rule.PassesTest(e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HelpTextExcludesPrivateUnicodeCharacters_HelpTextWhiteSpace_ThrowsArgumentException()
        {
            var e = new MockA11yElement();
            e.HelpText = " ";

            Rule.PassesTest(e);
        }
    } // class
} // namespace
