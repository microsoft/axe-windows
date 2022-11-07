// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Misc;
using Axe.Windows.Rules.Resources;
using System;

namespace Axe.Windows.Rules
{
    class AndCondition : Condition
    {
        private readonly Condition _a;
        private readonly Condition _b;

        public AndCondition(Condition a, Condition b)
        {
            _a = a ?? throw new ArgumentNullException(nameof(a));
            _b = b ?? throw new ArgumentNullException(nameof(b));
        }

        public override bool Matches(IA11yElement element)
        {
            return _a.Matches(element)
                && _b.Matches(element);
        }

        public override string ToString()
        {
            return ConditionDescriptions.And.WithParameters(_a.ToString(), _b.ToString());
        }
    } // class
} // namespace
