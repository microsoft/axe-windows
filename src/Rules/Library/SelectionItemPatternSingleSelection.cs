// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.Relationships;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.SelectionItemPatternSingleSelection)]
    class SelectionItemPatternSingleSelection : Rule
    {
        public SelectionItemPatternSingleSelection()
        {
            Info.Description = Descriptions.SelectionItemPatternSingleSelection;
            Info.HowToFix = HowToFix.SelectionItemPatternSingleSelection;
            Info.Standard = A11yCriteriaId.AvailableActions;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            var controlType = new ControlTypeCondition(e.ControlTypeId);
            var condition = SiblingCount(controlType & SelectionItemPattern.IsSelected) <= 1;

            return condition.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return TabItem
                & Patterns.SelectionItem
                & SelectionItemPattern.IsSelected;
        }
    } // class
} // namespace
