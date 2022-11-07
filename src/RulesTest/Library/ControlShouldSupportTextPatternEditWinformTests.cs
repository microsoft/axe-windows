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
    public class ControlShouldSupportTextPatternEditWinformTests
    {
        private static readonly Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.ControlShouldSupportTextPatternEditWinform();

        [TestMethod]
        public void FrameworkIssueLink_IsNotNull()
        {
            Assert.IsNotNull(Rule.Info.FrameworkIssueLink);
        }

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
        public void NoFramework_NotApplicable()
        {
            var e = new MockA11yElement
            {
                ControlTypeId = ControlType.Edit
            };

            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void WinFormsEdit_Applicable()
        {
            var e = new MockA11yElement
            {
                ControlTypeId = ControlType.Edit,
                Framework = FrameworkId.WinForm
            };

            Assert.IsTrue(Rule.Condition.Matches(e));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNull_Exception()
        {
            Rule.PassesTest(null);
        }
    } // class
} // namespace
