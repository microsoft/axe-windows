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
    [RuleInfo(ID = RuleId.IsControlElementTrueOptional)]
    class IsControlElementTrueOptional : Rule
    {
        public IsControlElementTrueOptional()
        {
            this.Info.Description = Descriptions.IsControlElementTrueOptional;
            this.Info.HowToFix = HowToFix.IsControlElementTrueOptional;
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = PropertyType.UIA_IsControlElementPropertyId;
            this.Info.ErrorCode = EvaluationCode.Open;
        }

        public override bool PassesTest(IA11yElement e)
        {
            return false;
        }

        protected override Condition CreateCondition()
        {
            return IsNotControlElement & BoundingRectangle.Valid & ElementGroups.IsControlElementTrueOptional;
        }
    } // class
} // namespace
