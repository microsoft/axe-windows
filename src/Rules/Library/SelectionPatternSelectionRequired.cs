// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.SelectionPatternSelectionRequired)]
    class SelectionPatternSelectionRequired : Rule
    {
        public SelectionPatternSelectionRequired()
        {
            Info.Description = Descriptions.SelectionPatternSelectionRequired;
            Info.HowToFix = HowToFix.SelectionPatternSelectionRequired;
            Info.Standard = A11yCriteriaId.AvailableActions;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return IsSelectionRequired(e);
        }

        private static bool IsSelectionRequired(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var selection = e.GetPattern(PatternType.UIA_SelectionPatternId);
            if (selection == null) return false;

            return selection.GetValue<bool>("IsSelectionRequired");
        }

        protected override Condition CreateCondition()
        {
            return Tab & Patterns.Selection;
        }
    } // class
} // namespace
