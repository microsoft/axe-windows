// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class NameExcludesPrivateUnicodeCharactersTests
    {
        private static readonly Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.NameExcludesPrivateUnicodeCharacters();

        [TestMethod]
        public void NameExcludesPrivateUnicodeCharacters_Pass()
        {
            var e = new MockA11yElement();
            e.Name = "Hello";

            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void NameExcludesPrivateUnicodeCharacters_Fail()
        {
            var e = new MockA11yElement();
            e.Name = "Hello \uE000";

            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NameExcludesPrivateUnicodeCharacters_NullElement_ThrowsArgumentNullException()
        {
            Rule.PassesTest(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameExcludesPrivateUnicodeCharacters_NameNull_ThrowsArgumentException()
        {
            var e = new MockA11yElement();

            Rule.PassesTest(e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameExcludesPrivateUnicodeCharacters_NameEmpty_ThrowsArgumentException()
        {
            var e = new MockA11yElement();
            e.Name = string.Empty;

            Rule.PassesTest(e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameExcludesPrivateUnicodeCharacters_NameWhiteSpace_ThrowsArgumentException()
        {
            var e = new MockA11yElement();
            e.Name = " ";

            Rule.PassesTest(e);
        }
    } // class
} // namespace
