// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;
using static Axe.Windows.Rules.PropertyConditions.ElementGroups;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.BoundingRectangleNotNullListViewXAML)]
    class BoundingRectangleNotNullListViewXAML : Rule
    {
        public BoundingRectangleNotNullListViewXAML()
        {
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = Axe.Windows.Core.Types.PropertyType.UIA_BoundingRectanglePropertyId;
            this.Info.Description = Descriptions.BoundingRectangleNotNull;
            this.Info.HowToFix = HowToFix.BoundingRectangleNotNull;
            this.Info.ErrorCode = EvaluationCode.Error;
            this.Info.FrameworkIssueLink = "https://aka.ms/FrameworkIssue-BoundingRectangleNotNullListViewXAML";
        }

        public override bool PassesTest(IA11yElement e)
        {
            return BoundingRectangle.NotNull.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            // This rule is scoped to XAML List elements
            return IsNotOffScreen
                & ListXAML;
        }
    } // class
} // namespace
