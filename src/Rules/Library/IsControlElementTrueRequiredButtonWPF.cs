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
    [RuleInfo(ID = RuleId.IsControlElementTrueRequiredButtonWPF)]
    class IsControlElementTrueRequiredButtonWPF : Rule
    {
        public IsControlElementTrueRequiredButtonWPF()
        {
            this.Info.Description = Descriptions.IsControlElementTrueRequired;
            this.Info.HowToFix = HowToFix.IsControlElementTrueRequired;
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = PropertyType.UIA_IsControlElementPropertyId;
            this.Info.ErrorCode = EvaluationCode.Error;
            this.Info.FrameworkIssueLink = "https://go.microsoft.com/fwlink/?linkid=2214420";
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return IsControlElement.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return BoundingRectangle.Valid
                & ElementGroups.IsControlElementTrueRequired
                & ElementGroups.WPFButton;
        }
    } // class
} // namespace
