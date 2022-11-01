﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Misc;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using System.Globalization;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ControlViewSeparatorStructure)]
    class ControlViewSeparatorStructure : Rule
    {
        public ControlViewSeparatorStructure()
        {
            Info.Description = Descriptions.Structure.WithParameters(ControlView.SeparatorStructure);
            Info.HowToFix = HowToFix.Structure.WithParameters(ControlView.SeparatorStructure);
            Info.Standard = A11yCriteriaId.InfoAndRelationships;
            Info.ErrorCode = EvaluationCode.NeedsReview;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return ControlView.SeparatorStructure.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return Separator;
        }
    } // class
} // namespace
