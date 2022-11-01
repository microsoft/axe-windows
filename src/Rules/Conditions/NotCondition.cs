// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Misc;
using Axe.Windows.Rules.Resources;
using System;

namespace Axe.Windows.Rules
{
    class NotCondition : Condition
    {
        private readonly Condition A;

        public NotCondition(Condition a)
        {
            this.A = a ?? throw new ArgumentNullException(nameof(a));
        }

        public override bool Matches(IA11yElement element)
        {
            return !this.A.Matches(element);
        }

        public override string ToString()
        {
            return ConditionDescriptions.Not.WithParameters(this.A.ToString());
        }
    } // class
} // namespace
