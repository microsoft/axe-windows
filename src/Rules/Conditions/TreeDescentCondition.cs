// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using System;

using static System.FormattableString;

namespace Axe.Windows.Rules
{
    /// <summary>
    /// Allows the grammatic structure [parent condition] / [child condition] to be expressed.
    /// </summary>
    class TreeDescentCondition : Condition
    {
        private readonly Condition _parentCondition;
        private readonly Condition _childCondition;

        public TreeDescentCondition(Condition parentCondition, Condition childCondition)
        {
            _parentCondition = parentCondition ?? throw new ArgumentNullException(nameof(parentCondition));
            _childCondition = childCondition ?? throw new ArgumentNullException(nameof(childCondition));
        }

        public override bool Matches(IA11yElement e)
        {
            if (!_parentCondition.Matches(e))
                return false;

            var child = e?.GetFirstChild();

            return _childCondition.Matches(child);
        }

        public override string ToString()
        {
            return Invariant($"{_parentCondition} / {_childCondition}");
        }
    } // class
} // namespace
