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
    [RuleInfo(ID = RuleId.BoundingRectangleNotValidButOffScreen)]
    class BoundingRectangleNotValidButOffScreen : Rule
    {
        public BoundingRectangleNotValidButOffScreen()
        {
            Info.Description = Descriptions.BoundingRectangleNotValidButOffScreen;
            Info.HowToFix = HowToFix.BoundingRectangleNotValidButOffScreen;
            Info.Standard = A11yCriteriaId.ObjectInformation;
            Info.PropertyID = PropertyType.UIA_BoundingRectanglePropertyId;
            Info.ErrorCode = EvaluationCode.NeedsReview;
        }

        public override bool PassesTest(IA11yElement e)
        {
            return false;
        }

        protected override Condition CreateCondition()
        {
            return IsOffScreen & BoundingRectangle.NotValid;
        }
    } // class
} // namespace
