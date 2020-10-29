// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ControlShouldSupportTablePatternInEdge)]
    class ControlShouldSupportTablePatternInEdge : Rule
    {
        public ControlShouldSupportTablePatternInEdge()
        {
            this.Info.Description = Descriptions.ControlShouldSupportTablePattern;
            this.Info.HowToFix = HowToFix.ControlShouldSupportTablePattern;
            this.Info.Standard = A11yCriteriaId.AvailableActions;
            this.Info.PropertyID = PropertyType.UIA_IsTablePatternAvailablePropertyId;
        }

        public override EvaluationCode Evaluate(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return Patterns.Table.Matches(e) ? EvaluationCode.Pass : EvaluationCode.Warning;
        }

        protected override Condition CreateCondition()
        {
            return (Calendar | Table) & StringProperties.Framework.Is(Framework.Edge);
        }
    } // class
} // namespace
