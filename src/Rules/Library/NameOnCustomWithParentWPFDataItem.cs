// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.NameOnCustomWithParentWPFDataItem)]
    class NameOnCustomWithParentWPFDataItem : Rule
    {
        public NameOnCustomWithParentWPFDataItem()
        {
            Info.Description = Descriptions.NameOnCustomWithParentWPFDataItem;
            Info.HowToFix = HowToFix.NameOnCustomWithParentWPFDataItem;
            Info.Standard = A11yCriteriaId.ObjectInformation;
            Info.PropertyID = PropertyType.UIA_NamePropertyId;
            Info.ErrorCode = EvaluationCode.NeedsReview;
        }

        public override bool PassesTest(IA11yElement e)
        {
            return false;
        }

        protected override Condition CreateCondition()
        {
            return Custom & Name.NullOrEmpty & ElementGroups.ParentWPFDataItem;
        }
    } // class
} // namespace
