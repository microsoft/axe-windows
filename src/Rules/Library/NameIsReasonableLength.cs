// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Globalization;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(
        ID = RuleId.NameReasonableLength)]
    class NameIsReasonableLength : Rule
    {
        private const int ReasonableLength = 512;

        public NameIsReasonableLength()
        {
            Info.Description = Descriptions.NameReasonableLength;
            this.Info.HowToFix = HowToFix.NameReasonableLength;
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = PropertyType.UIA_NamePropertyId;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (e.Name == null) throw new ArgumentException(ErrorMessages.ElementNameNullOrWhiteSpace, nameof(e));

            var condition = Name.Length <= ReasonableLength;
            return condition.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return Name.NotNullOrEmpty & ~Text;
        }
    } // class
} // namespace
