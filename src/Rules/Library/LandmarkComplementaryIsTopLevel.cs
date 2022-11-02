// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.Relationships;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.LandmarkComplementaryIsTopLevel)]
    class LandmarkComplementaryIsTopLevel : Rule
    {
        public LandmarkComplementaryIsTopLevel()
        {
            Info.Description = Descriptions.LandmarkComplementaryIsTopLevel;
            Info.HowToFix = HowToFix.LandmarkComplementaryIsTopLevel;
            Info.Standard = A11yCriteriaId.InfoAndRelationships;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var stopCondition = Condition.False;

            return !AnyAncestor(Landmarks.Any, stopCondition).Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return Landmarks.Complementary;
        }
    } // class
} // namespace
