// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

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
        }

        public override EvaluationCode Evaluate(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return Patterns.Text.Matches(e) ? EvaluationCode.Pass : EvaluationCode.Error;
        }

        protected override Condition CreateCondition()
        {
            var win32Edit = Edit & StringProperties.Framework.Is(FrameworkId.Win32);
            var nonfocusableDirectUIEdit = Edit & ~IsKeyboardFocusable & StringProperties.Framework.Is(FrameworkId.DirectUI);

            return Document
                | (Edit
                & ~win32Edit
                & ~nonfocusableDirectUIEdit);
        }
    } // class
} // namespace
