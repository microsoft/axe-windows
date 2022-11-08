// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using System;

namespace Axe.Windows.Rules
{
    class PatternCondition : Condition
    {
        private readonly int _patternID;

        private ValidateProperty Validate { get; set; }

        public delegate bool ValidateProperty(IA11yElement e);

        public PatternCondition(int patternID, ValidateProperty validate = null)
        {
            if (patternID == 0) throw new ArgumentNullException(nameof(patternID));

            _patternID = patternID;
            Validate = validate;
        }

        public override bool Matches(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var pattern = e.GetPattern(_patternID);

            return pattern != null && (Validate == null || Validate(e));
        }

        public override string ToString()
        {
            var patternName = PatternType.GetInstance().GetNameById(_patternID);
            return $"has {patternName} pattern";
        }
    } // class
} // namespace
