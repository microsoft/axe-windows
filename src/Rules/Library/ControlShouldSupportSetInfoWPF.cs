// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.Framework;
using static Axe.Windows.Rules.PropertyConditions.IntProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ControlShouldSupportSetInfoWPF)]
    class ControlShouldSupportSetInfoWPF : Rule
    {
        public ControlShouldSupportSetInfoWPF()
        {
            this.Info.Description = Descriptions.ControlShouldSupportSetInfo;
            this.Info.HowToFix = HowToFix.ControlShouldSupportSetInfo;
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.ErrorCode = EvaluationCode.Error;
            this.Info.FrameworkIssueLink = "https://aka.ms/FrameworkIssue-ControlShouldSupportSetInfoWPF";
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return PositionInSet.Exists.Matches(e) & SizeOfSet.Exists.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return WPF & (ListItem | TreeItem);
        }
    }
}
