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
    [RuleInfo(ID = RuleId.ParentChildShouldNotHaveSameNameAndLocalizedControlType)]
    class ParentChildShouldNotHaveSameNameAndLocalizedControlType : Rule
    {
        public ParentChildShouldNotHaveSameNameAndLocalizedControlType()
        {
            Info.Description = Descriptions.ParentChildShouldNotHaveSameNameAndLocalizedControlType;
            Info.HowToFix = HowToFix.ParentChildShouldNotHaveSameNameAndLocalizedControlType;
            Info.Standard = A11yCriteriaId.ObjectInformation;
            Info.PropertyID = PropertyType.UIA_NamePropertyId;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (e.Parent == null) throw new ArgumentException(ErrorMessages.ElementParentNull, nameof(e));

            return (e.Name != e.Parent.Name) || (e.LocalizedControlType != e.Parent.LocalizedControlType);
        }

        protected override Condition CreateCondition()
        {
            return IsKeyboardFocusable & BoundingRectangle.Valid & ElementGroups.NameRequired;
        }
    } // class
} // namespace
