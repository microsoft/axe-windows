// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.Framework;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ControlShouldSupportTablePattern)]
    class ControlShouldSupportTablePattern : Rule
    {
        public ControlShouldSupportTablePattern()
        {
            this.Info.Description = Descriptions.ControlShouldSupportTablePattern;
            this.Info.HowToFix = HowToFix.ControlShouldSupportTablePattern;
            this.Info.Standard = A11yCriteriaId.AvailableActions;
            this.Info.PropertyID = PropertyType.UIA_IsTablePatternAvailablePropertyId;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return Patterns.Table.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return (Calendar | Table) & ~Edge;
        }
    } // class
} // namespace
