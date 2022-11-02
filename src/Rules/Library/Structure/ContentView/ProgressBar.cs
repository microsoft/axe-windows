// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Misc;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ContentViewProgressBarStructure)]
    class ContentViewProgressBarStructure : Rule
    {
        public ContentViewProgressBarStructure()
        {
            this.Info.Description = Descriptions.Structure.WithParameters(ContentView.ProgressBarStructure);
            this.Info.HowToFix = HowToFix.Structure.WithParameters(ContentView.ProgressBarStructure);
            this.Info.Standard = A11yCriteriaId.InfoAndRelationships;
            this.Info.ErrorCode = EvaluationCode.NeedsReview;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return ContentView.ProgressBarStructure.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return ProgressBar;
        }
    } // class
} // namespace
