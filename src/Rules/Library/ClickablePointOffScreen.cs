// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ClickablePointOffScreen)]
    class ClickablePointOffScreen : Rule
    {
        public ClickablePointOffScreen()
        {
            this.Info.Description = Descriptions.ClickablePointOffScreen;
            this.Info.HowToFix = HowToFix.ClickablePointOffScreen;
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = PropertyType.UIA_IsOffscreenPropertyId;
        }

        public override EvaluationCode Evaluate(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return IsOffScreen.Matches(e) ? EvaluationCode.Pass : EvaluationCode.Warning;
        }

        protected override Condition CreateCondition()
        {
            return ClickablePoint.OffScreen;
        }
    } // class
} // namespace
