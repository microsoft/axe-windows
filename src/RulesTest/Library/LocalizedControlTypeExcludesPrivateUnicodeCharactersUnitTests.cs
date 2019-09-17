// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Axe.Windows.RulesTest.ControlType;
using EvaluationCode = Axe.Windows.Rules.EvaluationCode;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class LocalizedControlTypeExcludesPrivateUnicodeCharactersUnitTests
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.LocalizedControlTypeExcludesPrivateUnicodeCharacters();

        [TestMethod]
        public void LocalizedControlTypeExcludesPrivateUnicodeCharacters_Pass()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = "Hello";

            Assert.AreEqual(EvaluationCode.Pass, Rule.Evaluate(e));
        }

        [TestMethod]
        public void LocalizedControlTypeExcludesPrivateUnicodeCharacters_Faile()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = "Hello \uE000";

            Assert.AreEqual(EvaluationCode.Error, Rule.Evaluate(e));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LocalizedControlTypeExcludesPrivateUnicodeCharacters_NullElement_ThrowsArgumentNullException()
        {
            Rule.Evaluate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LocalizedControlTypeExcludesPrivateUnicodeCharacters_LocalizedControlTypeNull_ThrowsArgumentException()
        {
            var e = new MockA11yElement();

            Rule.Evaluate(e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LocalizedControlTypeExcludesPrivateUnicodeCharacters_LocalizedControlTypeEmpty_ThrowsArgumentException()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = string.Empty;

            Rule.Evaluate(e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LocalizedControlTypeExcludesPrivateUnicodeCharacters_LocalizedControlTypeWhiteSpace_ThrowsArgumentException()
        {
            var e = new MockA11yElement();
            e.LocalizedControlType = " ";

            Rule.Evaluate(e);
        }
    } // class
} // namespace
