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
    [RuleInfo(ID = RuleId.ClickablePointOnScreenWPF)]
    class ClickablePointOnScreenWPF : Rule
    {
        public ClickablePointOnScreenWPF()
        {
            this.Info.Description = Descriptions.ClickablePointOnScreen;
            this.Info.HowToFix = HowToFix.ClickablePointOnScreen;
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = PropertyType.UIA_IsOffscreenPropertyId;
            this.Info.ErrorCode = EvaluationCode.Error;
            this.Info.FrameworkIssueLink = "https://aka.ms/FrameworkIssue-ClickablePointOnScreenListViewWPF";
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return IsNotOffScreen.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return IsKeyboardFocusable & ClickablePoint.OnScreen & WPF;
        }
    } // class
} // namespace
