// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Misc;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ControlViewRadioButtonStructure)]
    class ControlViewRadioButtonStructure : Rule
    {
        public ControlViewRadioButtonStructure()
        {
            this.Info.Description = Descriptions.Structure.WithParameters(ControlView.RadioButtonStructure);
            this.Info.HowToFix = HowToFix.Structure.WithParameters(ControlView.RadioButtonStructure);
            this.Info.Standard = A11yCriteriaId.InfoAndRelationships;
            this.Info.ErrorCode = EvaluationCode.NeedsReview;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return ControlView.RadioButtonStructure.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return RadioButton;
        }
    } // class
} // namespace
