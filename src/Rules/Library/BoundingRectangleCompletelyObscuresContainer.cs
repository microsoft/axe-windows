// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
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
    [RuleInfo(ID = RuleId.BoundingRectangleCompletelyObscuresContainer)]
    class BoundingRectangleCompletelyObscuresContainer : Rule
    {
        public BoundingRectangleCompletelyObscuresContainer()
        {
            this.Info.Description = Descriptions.BoundingRectangleCompletelyObscuresContainer;
            this.Info.HowToFix = HowToFix.BoundingRectangleCompletelyObscuresContainer;
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = PropertyType.UIA_BoundingRectanglePropertyId;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return !BoundingRectangle.CompletelyObscuresContainer.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            // Windows and dialogs can be any size, regardless of their parents
var isDialog = Pane & IsDialog;

            //  Light dismiss buttons cover the whole window so that clicking dismisses the combo box
            var isLightDismissButton = Button & IsNotKeyboardFocusable & XAML & ClassName.Is("ComboBoxLightDismiss");

            return ~Window
                & ~isDialog
                & IsNotOffScreen
                & BoundingRectangle.Valid
                & ParentExists
                & Parent(IsNotDesktop)
                & Parent(BoundingRectangle.Valid)
                & ~isLightDismissButton;
        }
    } // class
} // namespace
