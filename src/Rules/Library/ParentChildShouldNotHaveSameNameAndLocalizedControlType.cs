// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ParentChildShouldNotHaveSameNameAndLocalizedControlType)]
    class ParentChildShouldNotHaveSameNameAndLocalizedControlType : Rule
    {
        public ParentChildShouldNotHaveSameNameAndLocalizedControlType()
        {
            this.Info.Description = Descriptions.ParentChildShouldNotHaveSameNameAndLocalizedControlType;
            this.Info.HowToFix = HowToFix.ParentChildShouldNotHaveSameNameAndLocalizedControlType;
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = PropertyType.UIA_NamePropertyId;
        }

        public override EvaluationCode Evaluate(IA11yElement e)
        {
            if (e == null) return EvaluationCode.RuleExecutionError;
            if (e.Parent == null) return EvaluationCode.RuleExecutionError;

            return e.Name == e.Parent.Name && e.LocalizedControlType == e.Parent.LocalizedControlType ? EvaluationCode.Error : EvaluationCode.Pass;
        }

        protected override Condition CreateCondition()
        {
            return IsKeyboardFocusable & BoundingRectangle.Valid & ElementGroups.NameRequired;
        }
    } // class
} // namespace
