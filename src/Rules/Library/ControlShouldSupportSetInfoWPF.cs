// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.IntProperties;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

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
        }

        public override EvaluationCode Evaluate(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            if (PositionInSet.Exists.Matches(e) & SizeOfSet.Exists.Matches(e))
            {
                return EvaluationCode.Pass;
            }

            return EvaluationCode.Warning;
        }

        protected override Condition CreateCondition()
        {
            return PropertyConditions.StringProperties.Framework.Is(Core.Enums.Framework.WPF) & (ListItem | TreeItem);
        }
    }
}
