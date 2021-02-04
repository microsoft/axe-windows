// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.SelectionPatternSingleSelection)]
    class SelectionPatternSingleSelection : Rule
    {
        public SelectionPatternSingleSelection()
        {
            this.Info.Description = Descriptions.SelectionPatternSingleSelection;
            this.Info.HowToFix = HowToFix.SelectionPatternSingleSelection;
            this.Info.Standard = A11yCriteriaId.AvailableActions;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return !SelectionPattern.CanSelectMultiple.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            var controls = Spinner | Tab;
            return controls & Patterns.Selection;
        }
    } // class
} // namespace
