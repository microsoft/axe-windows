// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Rules.Resources;
using System;

namespace Axe.Windows.Rules
{
    class ControlTypeCondition : Condition
    {
        public int ControlType { get; }

        public ControlTypeCondition(int controlType)
        {
            if (controlType == 0) throw new ArgumentException(ErrorMessages.IntParameterEqualsZero, nameof(controlType));

            ControlType = controlType;
        }

        public override bool Matches(IA11yElement element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            return element.ControlTypeId == ControlType;
        }

        public override string ToString()
        {
            // stripping away the integer name because it makes conditions harder to read
            var s = Axe.Windows.Core.Types.ControlType.GetInstance()?.GetNameById(ControlType);
            return s.Substring(0, s.IndexOf('('));
        }
    } // class
} // namespace
