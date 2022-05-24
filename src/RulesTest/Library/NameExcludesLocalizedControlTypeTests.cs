// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using static Axe.Windows.RulesTests.ControlType;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class NameExcludesLocalizedControlTypeTests
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.NameExcludesLocalizedControlType();

        [TestMethod]
        public void NameExcludesLocalizedControlType_Pass()
        {
            var e = new MockA11yElement();
            e.Name = "This is a button";
            e.LocalizedControlType = "Checkbox";

            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void NameExcludesLocalizedControlType_PassSimilar()
        {
            var e = new MockA11yElement();
            e.Name = "Comfortable";
            e.LocalizedControlType = "Tab";

            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void NameExcludesLocalizedControlType_Fail()
        {
            var e = new MockA11yElement();
            e.Name = "This is a button, yep";
            e.LocalizedControlType = "Button";

            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void NameExcludesLocalizedControlType_FailIdentical()
        {
            var e = new MockA11yElement();
            e.Name = "Custom";
            e.LocalizedControlType = "Custom";

            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameExcludesLocalizedControlType_ExceptionNullName()
        {
            var e = new MockA11yElement();
            e.Name = null;
            e.LocalizedControlType = "Button";

            Rule.PassesTest(e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameExcludesLocalizedControlType_ExceptionNullLocalizedControlType()
        {
            var e = new MockA11yElement();
            e.Name = "name";
            e.LocalizedControlType = null;

            Rule.PassesTest(e);
        }

        [TestMethod]
        public void NameExcludesLocalizedControlType_ApplicableTypes()
        {
            var e = new MockA11yElement();
            e.Name = "same";
            e.LocalizedControlType = "same";

            int[] nonapplicableTypes = { AppBar, Custom, Header, MenuBar, SemanticZoom, StatusBar, TitleBar, Text };

            foreach (var t in nonapplicableTypes)
            {
                e.ControlTypeId = t;
                Assert.IsFalse(Rule.Condition.Matches(e), $"Type: {t}");
            } // for each type

            var applicableTypes = ControlType.All.Except(nonapplicableTypes);

            foreach (var t in applicableTypes)
            {
                e.ControlTypeId = t;
                Assert.IsTrue(Rule.Condition.Matches(e), $"Type: {t}");
            } // for each type
        }
    } // class
} // namespace
