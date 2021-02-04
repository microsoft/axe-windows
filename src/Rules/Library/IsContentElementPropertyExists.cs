// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.IsContentElementPropertyExists)]
    class IsContentElementPropertyExists : Rule
    {
        public IsContentElementPropertyExists()
        {
            this.Info.Description = Descriptions.IsContentElementPropertyExists;
            this.Info.HowToFix = HowToFix.IsContentElementPropertyExists;
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = PropertyType.UIA_IsContentElementPropertyId;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return IsContentElementExists.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return BoundingRectangle.Valid & (ToolTip | Text);
        }
    } // class
} // namespace
