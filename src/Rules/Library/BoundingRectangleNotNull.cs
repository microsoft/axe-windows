// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;
using static Axe.Windows.Rules.PropertyConditions.ElementGroups;
using static Axe.Windows.Rules.PropertyConditions.Framework;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.BoundingRectangleNotNull)]
    class BoundingRectangleNotNull : Rule
    {
        public BoundingRectangleNotNull()
        {
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = Axe.Windows.Core.Types.PropertyType.UIA_BoundingRectanglePropertyId;
            this.Info.Description = Descriptions.BoundingRectangleNotNull;
            this.Info.HowToFix = HowToFix.BoundingRectangleNotNull;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            return BoundingRectangle.NotNull.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            var sysmenubar = ControlType.MenuBar & StringProperties.AutomationID.Is("SystemMenuBar");
            var sysmenuitem = ControlType.MenuItem & Relationships.Parent(sysmenubar);

            // This exception is meant to apply to the non-Chromium version of Edge
            var edgeGroups = ControlType.Group & Edge;

            // the Bounding rectangle property might be empty due to
            // a non-existent property, or an invalid data format.
            // If the Bounding rectangle property is not empty, it means all the above criteria were met successfully
            // So we only want to test for null when the BoundingRectangle property is empty.
            //
            // 2 exceptions related with Menubar "System" and Menu item "System". 
            // These two are excluded since Windows 10 sets the bounding rectangles of these as null by default.
            // WPF Scrollbar page buttons may sometimes
            // legitimately be null or all zeros when the thumb control is at the maximum or minimum value.
            return IsNotOffScreen
                & ~WPFScrollBarPageButtons
                & ~NonFocusableSliderButtons
                & ~edgeGroups
                & ~sysmenubar
                & ~sysmenuitem;
        }
    } // class
} // namespace
