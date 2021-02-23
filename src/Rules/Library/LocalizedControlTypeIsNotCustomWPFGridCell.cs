// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.Resources;
using System;
using System.Globalization;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.ElementGroups;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.LocalizedControlTypeNotCustomWPFGridCell)]
    class LocalizedControlTypeIsNotCustomWPFGridCell : Rule
    {
        public LocalizedControlTypeIsNotCustomWPFGridCell()
        {
            this.Info.Description = Descriptions.LocalizedControlTypeNotCustom;
            this.Info.HowToFix = string.Format(CultureInfo.CurrentCulture, HowToFix.LocalizedControlTypeNotCustomWPFGridCell, HowToFix.LocalizedControlTypeNotCustom);
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = PropertyType.UIA_LocalizedControlTypePropertyId;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return e.LocalizedControlType != LocalizedControlTypeNames.Custom;
        }

        protected override Condition CreateCondition()
        {
            return Custom
                & IsKeyboardFocusable
                & LocalizedControlType.NotNullOrEmpty
                & WPFDataGridCell;
        }
    } // class
} // namespace
