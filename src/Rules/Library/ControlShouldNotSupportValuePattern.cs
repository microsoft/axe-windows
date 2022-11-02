// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ControlShouldNotSupportValuePattern)]
    class ControlShouldNotSupportValuePattern : Rule
    {
        public ControlShouldNotSupportValuePattern()
        {
            Info.Description = Descriptions.ControlShouldNotSupportValuePattern;
            Info.HowToFix = HowToFix.ControlShouldNotSupportValuePattern;
            Info.Standard = A11yCriteriaId.AvailableActions;
            Info.ErrorCode = EvaluationCode.Warning;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return !Patterns.Value.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            // Removed Calendar, even though it is specified in MSDN
            // This seems to be an error in the documentation, or at least it seems to no longer be valid
            return Text;
        }
    } // class
} // namespace
