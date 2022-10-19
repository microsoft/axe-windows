// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Exceptions;
using Axe.Windows.Core.Misc;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using System.Globalization;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.Relationships;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ListItemSiblingsUnique)]
    class ListItemSiblingsUnique : Rule
    {
        public ListItemSiblingsUnique()
        {
            this.Info.Description = Descriptions.ListItemSiblingsUnique;
            this.Info.HowToFix = HowToFix.ListItemSiblingsUnique;
            this.Info.Standard = A11yCriteriaId.NameRoleValue;
            this.Info.ErrorCode = EvaluationCode.Warning;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (e.Parent == null) throw new ArgumentException(ErrorMessages.ElementParentNull, nameof(e));

            var siblings = SiblingCount(new ControlTypeCondition(e.ControlTypeId)
                & (e.IsKeyboardFocusable ? IsKeyboardFocusable : IsNotKeyboardFocusable)
                & Name.Is(e.Name)
                & LocalizedControlType.Is(e.LocalizedControlType));
            var count = siblings.GetValue(e);
            if (count < 1) throw new AxeWindowsException(ErrorMessages.NoElementFound.WithParameters(this.Info.ID));

            return count == 1;
        }

        protected override Condition CreateCondition()
        {
            return ListItem
                & IsContentOrControlElement
                & ParentExists
                & Name.NotNullOrEmpty
                & LocalizedControlType.NotNullOrEmpty
                & BoundingRectangle.Valid;
        }
    } // class
} // namespace
