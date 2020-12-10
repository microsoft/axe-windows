// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using static Axe.Windows.RulesTest.ControlType;
using EvaluationCode = Axe.Windows.Rules.EvaluationCode;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class NameExcludesLocalizedControlType
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.NameExcludesLocalizedControlType();

        [TestMethod]
        public void NameExcludesLocalizedControlType_Pass()
        {
            var e = new MockA11yElement();
            e.Name = "This is a button";
            e.LocalizedControlType = "Checkbox";

            Assert.AreEqual(EvaluationCode.Pass, Rule.Evaluate(e));
        }

        [TestMethod]
        public void NameExcludesLocalizedControlType_PassSimilar()
        {
            var e = new MockA11yElement();
            e.Name = "Comfortable";
            e.LocalizedControlType = "Tab";

            Assert.AreEqual(EvaluationCode.Pass, Rule.Evaluate(e));
        }

        [TestMethod]
        public void NameExcludesLocalizedControlType_Fail()
        {
            var e = new MockA11yElement();
            e.Name = "This is a button, yep";
            e.LocalizedControlType = "Button";

            Assert.AreEqual(EvaluationCode.Error, Rule.Evaluate(e));
        }

        [TestMethod]
        public void NameExcludesLocalizedControlType_FailIdentical()
        {
            var e = new MockA11yElement();
            e.Name = "Custom";
            e.LocalizedControlType = "Custom";

            Assert.AreEqual(EvaluationCode.Error, Rule.Evaluate(e));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameExcludesLocalizedControlType_ExceptionNullName()
        {
            var e = new MockA11yElement();
            e.Name = null;
            e.LocalizedControlType = "Button";

            Rule.Evaluate(e);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NameExcludesLocalizedControlType_ExceptionNullLocalizedControlType()
        {
            var e = new MockA11yElement();
            e.Name = "name";
            e.LocalizedControlType = null;

            Rule.Evaluate(e);
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

        [TestMethod]
        public void NameExcludesLocalizedControlType_EdgeEdit_ApplicableLCT()
        {
            var e = new MockA11yElement();
            e.Name = "not empty";
            e.LocalizedControlType = "not empty";

            e.ControlTypeId = ControlType.Edit;
            e.Framework = Core.Enums.Framework.Edge;

            Assert.IsTrue(Rule.Condition.Matches(e));

            string[] nonMatchingTypes = { "password", "email" };

            foreach (var lct in nonMatchingTypes)
            {
                e.LocalizedControlType = lct;
                Assert.IsFalse(Rule.Condition.Matches(e));
            } // for each type
        }

        [TestMethod]
        public void NameExcludesLocalizedControlType_EditButNotEdge_Match()
        {
            var e = new MockA11yElement();
            e.Name = "not empty";

            e.ControlTypeId = ControlType.Edit;
            e.LocalizedControlType = "password";

            Assert.IsTrue(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void NameExcludesLocalizedControlType_EdgeButNotEdit_Match()
        {
            var e = new MockA11yElement();
            e.Name = "not empty";

            e.Framework = Core.Enums.Framework.Edge;
            e.LocalizedControlType = "password";

            Assert.IsTrue(Rule.Condition.Matches(e));
        }
    } // class
} // namespace
