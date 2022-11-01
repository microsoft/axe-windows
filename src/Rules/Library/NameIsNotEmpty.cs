// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.NameNotEmpty)]
    class NameIsNotEmpty : Rule
    {
        public NameIsNotEmpty()
        {
            Info.Description = Descriptions.NameNotEmpty;
            Info.HowToFix = HowToFix.NameNotEmpty;
            Info.Standard = A11yCriteriaId.ObjectInformation;
            Info.PropertyID = PropertyType.UIA_NamePropertyId;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            return e.Name.Length > 0;
        }

        protected override Condition CreateCondition()
        {
            // Regardless if it is focusable, ProgressBar should be reported as an error

            return (IsKeyboardFocusable | ProgressBar)
                & Name.NotNull
                & BoundingRectangle.Valid
                & ElementGroups.NameRequired;
        }
    } // class
} // namespace
