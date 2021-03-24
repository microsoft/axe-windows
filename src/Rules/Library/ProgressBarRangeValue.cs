// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ProgressBarRangeValue)]
    class ProgressBarRangeValue : Rule
    {
        public ProgressBarRangeValue()
        {
            this.Info.Description = Descriptions.ProgressBarRangeValue;
            this.Info.HowToFix = HowToFix.ProgressBarRangeValue;
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = PropertyType.UIA_IsRangeValuePatternAvailablePropertyId;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return AreMinMaxValuesCorrect(e);
        }

        private static bool AreMinMaxValuesCorrect(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var rangeValue = e.GetPattern(PatternType.UIA_RangeValuePatternId);

            return MaxGreaterThanMin(rangeValue)
                && PropertyValueMatches(rangeValue, "IsReadOnly", true);
        }

        private static bool PropertyValueMatches<T>(IA11yPattern pattern, string name, T value) where T : IEquatable<T>
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            var p = pattern.GetValue<T>(name);
            var equatable = p as IEquatable<T>;
            if (equatable == null) return false;

            return equatable.Equals(value);
        }

        private static bool MaxGreaterThanMin(IA11yPattern pattern)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            var max = pattern.GetValue<int>("Maximum");
            var min = pattern.GetValue<int>("Minimum");

            return max > min;
        }

        protected override Condition CreateCondition()
        {
            return ProgressBar & Patterns.RangeValue;
        }
    } // class
} // namespace
