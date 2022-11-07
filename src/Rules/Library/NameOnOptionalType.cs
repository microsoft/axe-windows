// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.NameOnOptionalType)]
    class NameOnOptionalType : Rule
    {
        public NameOnOptionalType()
        {
            Info.Description = Descriptions.NameOnOptionalType;
            Info.HowToFix = HowToFix.NameOnOptionalType;
            Info.Standard = A11yCriteriaId.ObjectInformation;
            Info.PropertyID = PropertyType.UIA_NamePropertyId;
            Info.ErrorCode = EvaluationCode.NeedsReview;
        }

        public override bool PassesTest(IA11yElement e)
        {
            return Name.NotNullOrEmpty.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return ElementGroups.NameOptional;
        }
    } // class
} // namespace
