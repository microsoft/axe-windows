// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Axe.Windows.RulesTest.ControlType;
using EvaluationCode = Axe.Windows.Rules.EvaluationCode;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class HelpTextExcludesPrivateUnicodeCharactersUnitTests
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.HelpTextExcludesPrivateUnicodeCharacters();

        [TestMethod]
        public void HelpTextExcludesPrivateUnicodeCharacters_Pass()
        {
            var e = new MockA11yElement();
            e.HelpText = "Hello";

            Assert.AreEqual(EvaluationCode.Pass, Rule.Evaluate(e));
        }

        [TestMethod]
        public void HelpTextExcludesPrivateUnicodeCharacters_Faile()
        {
            var e = new MockA11yElement();
            e.HelpText = "Hello \uE000";

            Assert.AreEqual(EvaluationCode.Error, Rule.Evaluate(e));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HelpTextExcludesPrivateUnicodeCharacters_NullElement_ThrowsArgumentNullException()
        {
            Rule.Evaluate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HelpTextExcludesPrivateUnicodeCharacters_HelpTextNull_ThrowsArgumentException()
        {
            var e = new MockA11yElement();

            Rule.Evaluate(e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HelpTextExcludesPrivateUnicodeCharacters_HelpTextEmpty_ThrowsArgumentException()
        {
            var e = new MockA11yElement();
            e.HelpText = string.Empty;

            Rule.Evaluate(e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HelpTextExcludesPrivateUnicodeCharacters_HelpTextWhiteSpace_ThrowsArgumentException()
        {
            var e = new MockA11yElement();
            e.HelpText = " ";

            Rule.Evaluate(e);
        }
    } // class
} // namespace
