// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.Framework;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ControlShouldSupportTextPattern)]
    class ControlShouldSupportTextPattern : Rule
    {
        public ControlShouldSupportTextPattern()
        {
            this.Info.Description = Descriptions.ControlShouldSupportTextPattern;
            this.Info.HowToFix = HowToFix.ControlShouldSupportTextPattern;
            this.Info.Standard = A11yCriteriaId.AvailableActions;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return Patterns.Text.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            var win32Edit = Edit & Win32Framework;
            var nonfocusableDirectUIEdit = Edit & ~IsKeyboardFocusable & DirectUI;

            return Document
                | (Edit
                & ~win32Edit
                & ~nonfocusableDirectUIEdit);
        }
    } // class
} // namespace
