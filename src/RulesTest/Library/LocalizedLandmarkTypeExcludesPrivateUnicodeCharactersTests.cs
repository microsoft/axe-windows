// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class LocalizedLandmarkTypeExcludesPrivateUnicodeCharactersTests
    {
        private static readonly Rules.IRule Rule = new Rules.Library.LocalizedLandmarkTypeExcludesPrivateUnicodeCharacters();

        [TestMethod]
        public void LocalizedLandmarkTypeExcludesPrivateUnicodeCharacters_Pass()
        {
            var e = new MockA11yElement();
            e.LocalizedLandmarkType = "Hello";

            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void LocalizedLandmarkTypeExcludesPrivateUnicodeCharacters_Fail()
        {
            var e = new MockA11yElement();
            e.LocalizedLandmarkType = "Hello \uE000";

            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LocalizedLandmarkTypeExcludesPrivateUnicodeCharacters_NullElement_ThrowsArgumentNullException()
        {
            Rule.PassesTest(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LocalizedLandmarkTypeExcludesPrivateUnicodeCharacters_TypeNull_ThrowsArgumentNullException()
        {
            var e = new MockA11yElement();

            Rule.PassesTest(e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LocalizedLandmarkTypeExcludesPrivateUnicodeCharacters_TypeEmpty_ThrowsArgumentException()
        {
            var e = new MockA11yElement();
            e.LocalizedLandmarkType = string.Empty;

            Rule.PassesTest(e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LocalizedLandmarkTypeExcludesPrivateUnicodeCharacters_TypeWhiteSpace_ThrowsArgumentException()
        {
            var e = new MockA11yElement();
            e.LocalizedLandmarkType = " ";

            Rule.PassesTest(e);
        }
    } // class
} // namespace
