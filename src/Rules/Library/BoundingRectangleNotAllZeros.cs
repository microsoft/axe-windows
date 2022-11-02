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
    [RuleInfo(ID = RuleId.BoundingRectangleNotAllZeros)]
    class BoundingRectangleNotAllZeros : Rule
    {
        public BoundingRectangleNotAllZeros()
        {
            Info.Description = Descriptions.BoundingRectangleNotAllZeros;
            Info.HowToFix = HowToFix.BoundingRectangleNotAllZeros;
            Info.Standard = A11yCriteriaId.ObjectInformation;
            Info.PropertyID = PropertyType.UIA_BoundingRectanglePropertyId;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            return !BoundingRectangle.Empty.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return IsNotOffScreen & BoundingRectangle.NotNull;
        }
    } // class
} // namespace
