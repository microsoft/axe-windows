// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;
using static Axe.Windows.Rules.PropertyConditions.Framework;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ClickablePointOnScreen)]
    class ClickablePointOnScreen : Rule
    {
        public ClickablePointOnScreen()
        {
            Info.Description = Descriptions.ClickablePointOnScreen;
            Info.HowToFix = HowToFix.ClickablePointOnScreen;
            Info.Standard = A11yCriteriaId.ObjectInformation;
            Info.PropertyID = PropertyType.UIA_IsOffscreenPropertyId;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return IsNotOffScreen.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return (IsKeyboardFocusable & ClickablePoint.OnScreen) & ~WPF;
        }
    } // class
} // namespace
