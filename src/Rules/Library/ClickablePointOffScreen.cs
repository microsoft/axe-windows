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
        // Testable constructor
        internal ClickablePointOffScreen(Condition excludedCondition) : base(excludedCondition)
        {
            Info.Description = Descriptions.ClickablePointOffScreen;
            Info.HowToFix = HowToFix.ClickablePointOffScreen;
            Info.Standard = A11yCriteriaId.ObjectInformation;
            Info.PropertyID = PropertyType.UIA_IsOffscreenPropertyId;
            Info.ErrorCode = EvaluationCode.Warning;
        }

        public ClickablePointOffScreen() : this(DefaultExcludedCondition)
        {
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return IsOffScreen.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return ClickablePoint.OffScreen;
        }
    } // class
} // namespace
