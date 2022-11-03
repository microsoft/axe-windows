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
    [RuleInfo(ID = RuleId.BoundingRectangleNotNullTextBlockXAML)]
    class BoundingRectangleNotNullTextBlockXAML : Rule
    {
        public BoundingRectangleNotNullTextBlockXAML()
        {
            Info.Standard = A11yCriteriaId.ObjectInformation;
            Info.PropertyID = Axe.Windows.Core.Types.PropertyType.UIA_BoundingRectanglePropertyId;
            Info.Description = Descriptions.BoundingRectangleNotNull;
            Info.HowToFix = HowToFix.BoundingRectangleNotNull;
            Info.ErrorCode = EvaluationCode.Error;
            Info.FrameworkIssueLink = "https://go.microsoft.com/fwlink/?linkid=2213898";
        }

        public override bool PassesTest(IA11yElement e)
        {
            return BoundingRectangle.NotNull.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            // This rule is scoped to XAML Hyperlink elements parented by Text elements
            return IsNotOffScreen
                & HyperlinkInTextXAML;
        }
    } // class
} // namespace
