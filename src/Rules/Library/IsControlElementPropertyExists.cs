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
    [RuleInfo(ID = RuleId.IsControlElementPropertyExists)]
    class IsControlElementPropertyExists : Rule
    {
        public IsControlElementPropertyExists()
        {
            Info.Description = Descriptions.IsControlElementPropertyExists;
            Info.HowToFix = HowToFix.IsControlElementPropertyExists;
            Info.Standard = A11yCriteriaId.ObjectInformation;
            Info.PropertyID = PropertyType.UIA_IsControlElementPropertyId;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return IsControlElementExists.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            // This test should probably go away and we should always return Condition.True
            // But I am leaving it because it is the same as the old logic for the scans.
            return ElementGroups.IsControlElementTrueRequired;
        }
    } // class
} // namespace
