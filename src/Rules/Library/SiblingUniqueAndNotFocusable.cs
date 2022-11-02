﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Exceptions;
using Axe.Windows.Core.Misc;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.Framework;
using static Axe.Windows.Rules.PropertyConditions.Relationships;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.SiblingUniqueAndNotFocusable)]
    class SiblingUniqueAndNotFocusable : Rule
    {
        private static readonly Condition EligibleChild = CreateEligibleChildCondition();

        private static Condition CreateEligibleChildCondition()
        {
            var ExcludedType = DataItem | Image | Pane | ScrollBar | Thumb | TreeItem | ListItem | Hyperlink;

            return IsNotKeyboardFocusable
                & IsContentOrControlElement
                & ~ExcludedType
                & ParentExists
                & Name.NotNullOrEmpty
                & LocalizedControlType.NotNullOrEmpty
                & BoundingRectangle.Valid;
        }

        public SiblingUniqueAndNotFocusable()
        {
            Info.Description = Descriptions.SiblingUniqueAndNotFocusable;
            Info.HowToFix = HowToFix.SiblingUniqueAndNotFocusable;
            Info.Standard = A11yCriteriaId.NameRoleValue;
            Info.ErrorCode = EvaluationCode.NeedsReview;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (e.Parent == null) throw new ArgumentException(ErrorMessages.ElementParentNull, nameof(e));

            var siblings = SiblingCount(EligibleChild
                & Name.Is(e.Name)
                & LocalizedControlType.Is(e.LocalizedControlType));
            var count = siblings.GetValue(e);
            if (count < 1) throw new AxeWindowsException(ErrorMessages.NoElementFound.WithParameters(Info.ID));

            return count == 1;
        }

        protected override Condition CreateCondition()
        {
            var wpfDataItem = DataItem
                & WPF
                & NoChild(Custom | Name.NullOrEmpty);

            return EligibleChild & NotParent(wpfDataItem);
        }
    } // class
} // namespace
