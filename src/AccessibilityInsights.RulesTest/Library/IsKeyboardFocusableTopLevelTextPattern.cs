// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class IsKeyboardFocusableTopLevelTextPattern
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.IsKeyboardFocusableTopLevelTextPattern();

        [TestMethod]
        public void ConditionMismatch_XAMLTextControlWithTextPattern_ReturnFalse()
        {
            var e = new MockA11yElement();
            var p = new A11yPattern(e, PatternType.UIA_TextPatternId);

            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_TextControlTypeId;
            e.IsEnabled = true;
            e.IsOffScreen = false;
            e.IsContentElement = true;
            e.BoundingRectangle = new System.Drawing.Rectangle(0, 0, 100, 100);
            e.Framework = "XAML";

            p.Properties.Add(new A11yPatternProperty() { Name = "SupportedTextSelection", Value = UIAutomationClient.SupportedTextSelection.SupportedTextSelection_Single });

            e.Patterns.Add(p);

            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void ConditionMatch_NonXAMLTextControlWithTextPattern_ReturnTrue()
        {
            var e = new MockA11yElement();
            var p = new A11yPattern(e, PatternType.UIA_TextPatternId);

            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_TextControlTypeId;
            e.IsEnabled = true;
            e.IsOffScreen = false;
            e.IsContentElement = true;
            e.BoundingRectangle = new System.Drawing.Rectangle(0, 0, 100, 100);

            p.Properties.Add(new A11yPatternProperty() { Name = "SupportedTextSelection", Value = UIAutomationClient.SupportedTextSelection.SupportedTextSelection_Single });

            e.Patterns.Add(p);

            Assert.IsTrue(Rule.Condition.Matches(e));
        }
    } // class
} // namespace
