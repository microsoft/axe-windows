// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.ElementGroups;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ControlShouldSupportTextPatternEditWinform)]
    class ControlShouldSupportTextPatternEditWinform : Rule
    {
        public ControlShouldSupportTextPatternEditWinform()
        {
            Info.Description = Descriptions.ControlShouldSupportTextPattern;
            Info.HowToFix = HowToFix.ControlShouldSupportTextPattern;
            Info.Standard = A11yCriteriaId.AvailableActions;
            Info.ErrorCode = EvaluationCode.Error;
            Info.FrameworkIssueLink = "https://aka.ms/FrameworkIssue-ControlShouldSupportTextPatternEditWinForm";
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return Patterns.Text.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return WinFormsEdit;
        }
    } // class
} // namespace
