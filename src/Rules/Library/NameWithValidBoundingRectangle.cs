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
    [RuleInfo(ID = RuleId.NameWithValidBoundingRectangle)]
    class NameWithValidBoundingRectangle : Rule
    {
        public NameWithValidBoundingRectangle()
        {
            this.Info.Description = Descriptions.NameWithValidBoundingRectangle;
            this.Info.HowToFix = HowToFix.NameWithValidBoundingRectangle;
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = PropertyType.UIA_NamePropertyId;
            this.Info.ErrorCode = EvaluationCode.Warning;
        }

        public override bool PassesTest(IA11yElement e)
        {
            return false;
        }

        protected override Condition CreateCondition()
        {
            return BoundingRectangle.NotValid & Name.NullOrEmpty & ElementGroups.NameRequired;
        }
    } // class
} // namespace
