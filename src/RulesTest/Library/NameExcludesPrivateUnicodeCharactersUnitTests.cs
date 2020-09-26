// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EvaluationCode = Axe.Windows.Rules.EvaluationCode;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class NameExcludesPrivateUnicodeCharactersUnitTests
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.NameExcludesPrivateUnicodeCharacters();

        [TestMethod]
        public void NameExcludesPrivateUnicodeCharacters_Pass()
        {
            var e = new MockA11yElement();
            e.Name = "Hello";

            Assert.AreEqual(EvaluationCode.Pass, Rule.Evaluate(e));
        }

        [TestMethod]
        public void NameExcludesPrivateUnicodeCharacters_Fail()
        {
            var e = new MockA11yElement();
            e.Name = "Hello \uE000";

            Assert.AreEqual(EvaluationCode.Error, Rule.Evaluate(e));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NameExcludesPrivateUnicodeCharacters_NullElement_ThrowsArgumentNullException()
        {
            Rule.Evaluate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameExcludesPrivateUnicodeCharacters_NameNull_ThrowsArgumentException()
        {
            var e = new MockA11yElement();

            Rule.Evaluate(e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameExcludesPrivateUnicodeCharacters_NameEmpty_ThrowsArgumentException()
        {
            var e = new MockA11yElement();
            e.Name = string.Empty;

            Rule.Evaluate(e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameExcludesPrivateUnicodeCharacters_NameWhiteSpace_ThrowsArgumentException()
        {
            var e = new MockA11yElement();
            e.Name = " ";

            Rule.Evaluate(e);
        }
    } // class
} // namespace
