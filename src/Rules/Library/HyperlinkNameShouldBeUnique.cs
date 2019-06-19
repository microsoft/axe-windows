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
        private static Condition EligibleHyperlink = CreateHyperlinkSiblingCondition();

        public HyperlinkNameShouldBeUnique()
        {
            this.Info.Description = Descriptions.HyperlinkNameShouldBeUnique;
            this.Info.HowToFix = HowToFix.HyperlinkNameShouldBeUnique;
            this.Info.Standard = A11yCriteriaId.NameRoleValue;
        }

        public override EvaluationCode Evaluate(IA11yElement e)
        {
            if (e == null) throw new ArgumentException(nameof(e));
            if (e.Parent == null) throw new ArgumentNullException(nameof(e.Parent));

            var siblings = SiblingCount(EligibleHyperlink & Name.Is(e.Name)) <= 1;
            return siblings.Matches(e) ? EvaluationCode.Pass : EvaluationCode.Warning;
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
