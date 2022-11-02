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
        private readonly Condition A;
        private readonly Condition B;

        public AndCondition(Condition a, Condition b)
        {
            A = a ?? throw new ArgumentNullException(nameof(a));
            B = b ?? throw new ArgumentNullException(nameof(b));
        }

        public override bool Matches(IA11yElement element)
        {
            return A.Matches(element)
                && B.Matches(element);
        }

        public override string ToString()
        {
            return ConditionDescriptions.And.WithParameters(A.ToString(), B.ToString());
        }
    } // class
} // namespace
