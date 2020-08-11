// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Axe.Windows.Core.Bases;
using Axe.Windows.Rules.Resources;

namespace Axe.Windows.Rules
{
    class ControlTypeCondition : Condition
    {
        public int ControlType { get; } = 0;

        public ControlTypeCondition(int controlType)
        {
            if (controlType == 0) throw new ArgumentException(ErrorMessages.IntParameterEqualsZero, nameof(controlType));

            this.ControlType = controlType;
        }

        public override bool Matches(IA11yElement element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            return element.ControlTypeId == this.ControlType;
        }

        public override string ToString()
        {
            // stripping away the integer name because it makes conditions harder to read
            var s = Axe.Windows.Core.Types.ControlType.GetInstance()?.GetNameById(this.ControlType);
            return s.Substring(0, s.IndexOf('('));
        }
    } // class
} // namespace
