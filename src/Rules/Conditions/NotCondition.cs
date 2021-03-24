// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Rules.Resources;
using System;
using System.Globalization;

namespace Axe.Windows.Rules
{
    class NotCondition : Condition
    {
        private readonly Condition A;

        public NotCondition(Condition a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));

            this.A = a;
        }

        public override bool Matches(IA11yElement element)
        {
            return !this.A.Matches(element);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, ConditionDescriptions.Not, this.A.ToString());
        }
    } // class
} // namespace
