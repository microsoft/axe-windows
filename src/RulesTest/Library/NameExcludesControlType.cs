// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static Axe.Windows.RulesTest.ControlType;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class NameExcludesControlType
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.NameExcludesControlType();

        [TestMethod]
        public void NameExcludesControlType_Pass()
        {
            var e = new MockA11yElement();
            e.Name = "This is a button";
            e.ControlTypeId = CheckBox;

            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void NameExcludesControlType_PassSimilar()
        {
            var e = new MockA11yElement();
            e.Name = "Customize";
            e.ControlTypeId = Custom;

            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void NameExcludesControlType_Fail()
        {
            var e = new MockA11yElement();
            e.Name = "This is a button, yep";
            e.ControlTypeId = Button;

            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void NameExcludesControlType_FailIdentical()
        {
            var e = new MockA11yElement();
            e.Name = "Custom";
            e.ControlTypeId = Custom;

            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NameExcludesControlType_InvalidControlType()
        {
            var e = new MockA11yElement();
            e.Name = "Button";
            e.ControlTypeId = 0; // invalid

            Rule.PassesTest(e);
        }

        [TestMethod]
        public void NameExcludesControlType_NotApplicableType()
        {
            var e = new MockA11yElement();

            int[] types = { AppBar, Header, MenuBar, SemanticZoom, StatusBar, TitleBar };

            foreach (var t in types)
            {
                e.ControlTypeId = t;
                e.Name = Rules.Misc.ControlTypeStrings.Dictionary[t];

                Assert.IsFalse(Rule.Condition.Matches(e));
            } // for each type
        }
    } // class
} // namespace
