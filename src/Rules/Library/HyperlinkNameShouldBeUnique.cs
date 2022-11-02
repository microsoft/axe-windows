// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.Relationships;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.HyperlinkNameShouldBeUnique)]
    class HyperlinkNameShouldBeUnique : Rule
    {
        private static readonly Condition EligibleHyperlink = CreateHyperlinkSiblingCondition();

        public HyperlinkNameShouldBeUnique()
        {
            Info.Description = Descriptions.HyperlinkNameShouldBeUnique;
            Info.HowToFix = HowToFix.HyperlinkNameShouldBeUnique;
            Info.Standard = A11yCriteriaId.NameRoleValue;
            Info.ErrorCode = EvaluationCode.Warning;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (e.Parent == null) throw new ArgumentException(ErrorMessages.ElementParentNull, nameof(e));

            var siblings = SiblingCount(EligibleHyperlink & Name.Is(e.Name)) <= 1;
            return siblings.Matches(e);
        }

        protected override Condition CreateCondition() => EligibleHyperlink;

        private static Condition CreateHyperlinkSiblingCondition()
        {
            return Hyperlink
                & IsContentOrControlElement
                & ParentExists
                & Name.NotNullOrEmpty
                & BoundingRectangle.Valid;
        }
    } // class
} // namespace
