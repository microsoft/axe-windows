// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class ControlShouldSupportTextPatternUnitTests
    {
        private Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.ControlShouldSupportTextPattern();

        [TestMethod]
        public void HasTextPattern_Pass()
        {
            var e = new MockA11yElement();
            e.Patterns.Add(new A11yPattern(e, PatternType.UIA_TextPatternId));

            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void NoTextPattern_Error()
        {
            var e = new MockA11yElement();

            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void Document_IsApplicable()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = ControlType.Document;

            Assert.IsTrue(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void Edit_IsApplicable()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = ControlType.Edit;

            Assert.IsTrue(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void Win32Edit_NotApplicable()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = ControlType.Edit;
            e.Framework = FrameworkId.Win32;

            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void WinFormsEdit_NotApplicable()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = ControlType.Edit;
            e.Framework = FrameworkId.WinForm;

            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void DirectUIEdit_NotApplicable()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = ControlType.Edit;
            e.Framework = FrameworkId.DirectUI;
            e.IsKeyboardFocusable = false;

            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNull_Exception()
        {
            Rule.PassesTest(null);
        }
    } // class
} // namespace
