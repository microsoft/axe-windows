﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class LocalizedLandmarkTypeExcludesPrivateUnicodeCharacters
    {
        private static Rules.IRule Rule = new Rules.Library.LocalizedLandmarkTypeExcludesPrivateUnicodeCharacters();

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
