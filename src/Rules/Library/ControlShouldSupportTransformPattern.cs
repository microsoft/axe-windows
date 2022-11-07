// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ControlShouldSupportTransformPattern)]
    class ControlShouldSupportTransformPattern : Rule
    {
        public ControlShouldSupportTransformPattern()
        {
            Info.Description = Descriptions.ControlShouldSupportTransformPattern;
            Info.HowToFix = HowToFix.ControlShouldSupportTransformPattern;
            Info.Standard = A11yCriteriaId.AvailableActions;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return Patterns.Transform.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            // since this rule is based on the code from ScannerHeader
            // the assumption is that the test should only be true for header elements.
            // However, it seems like this should perhaps always be true.

            var controls = Header | HeaderItem;
            return controls & TransformPattern_CanResize;
        }
    } // class
} // namespace
