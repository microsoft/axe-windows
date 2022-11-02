// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.LocalizedLandmarkTypeNotNull)]
    class LocalizedLandmarkTypeNotNull : Rule
    {
        public LocalizedLandmarkTypeNotNull()
        {
            Info.Description = Descriptions.LocalizedLandmarkTypeNotNull;
            Info.HowToFix = HowToFix.LocalizedLandmarkTypeNotNull;
            Info.Standard = A11yCriteriaId.InfoAndRelationships;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return LocalizedLandmarkType.NotNull.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return Landmarks.AnyUIA;
        }
    } // class
} // namespace
