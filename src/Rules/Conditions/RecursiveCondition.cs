// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using System;

namespace Axe.Windows.Rules
{
    class RecursiveCondition : Condition
    {
        private Condition _a;

        private void Init(Condition a)
        {
            _a = a ?? throw new ArgumentNullException(nameof(a));
        }

        public static RecursiveCondition operator %(RecursiveCondition r, Condition c)
        {
            r.Init(c);
            return r;
        }

        public override bool Matches(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (_a == null) return false;

            return _a.Matches(e);
        }

        public override string ToString()
        {
            return "Recursive";
        }
    } // class
} // namespace
