// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.LocalizedLandmarkTypeIsReasonableLength)]
    class LocalizedLandmarkTypeIsReasonableLength : Rule
    {
        private const int ReasonableLength = 64;

        public LocalizedLandmarkTypeIsReasonableLength()
        {
            this.Info.Description = Descriptions.LocalizedLandmarkTypeIsReasonableLength;
            this.Info.HowToFix = HowToFix.LocalizedLandmarkTypeIsReasonableLength;
            this.Info.Standard = A11yCriteriaId.InfoAndRelationships;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var condition = LocalizedLandmarkType.Length <= ReasonableLength;
            return condition.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return LocalizedLandmarkType.NotNullOrEmpty;
        }
    } // class
} // namespace
