// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.Framework;
using static Axe.Windows.Rules.PropertyConditions.Relationships;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.LandmarkOneMain)]
    class LandmarkOneMain : Rule
    {
        public LandmarkOneMain()
        {
            this.Info.Description = Descriptions.LandmarkOneMain;
            this.Info.HowToFix = HowToFix.LandmarkOneMain;
            this.Info.Standard = A11yCriteriaId.InfoAndRelationships;
            this.Info.ErrorCode = EvaluationCode.Warning;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var condition = DescendantCount(Landmarks.Main) == 1;
            return condition.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return Pane & Edge & NotParent(Edge);
        }
    } // class
} // namespace
