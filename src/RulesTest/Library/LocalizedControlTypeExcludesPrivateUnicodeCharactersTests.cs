// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class LocalizedControlTypeExcludesPrivateUnicodeCharactersTests
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.LocalizedControlTypeExcludesPrivateUnicodeCharacters();

        [TestMethod]
        public void LocalizedControlTypeExcludesPrivateUnicodeCharacters_Pass()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = "Hello";

            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void LocalizedControlTypeExcludesPrivateUnicodeCharacters_Faile()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = "Hello \uE000";

            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LocalizedControlTypeExcludesPrivateUnicodeCharacters_NullElement_ThrowsArgumentNullException()
        {
            Rule.PassesTest(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LocalizedControlTypeExcludesPrivateUnicodeCharacters_LocalizedControlTypeNull_ThrowsArgumentException()
        {
            var e = new MockA11yElement();

            Rule.PassesTest(e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LocalizedControlTypeExcludesPrivateUnicodeCharacters_LocalizedControlTypeEmpty_ThrowsArgumentException()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = string.Empty;

            Rule.PassesTest(e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LocalizedControlTypeExcludesPrivateUnicodeCharacters_LocalizedControlTypeWhiteSpace_ThrowsArgumentException()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = " ";

            Rule.PassesTest(e);
        }
    } // class
} // namespace
