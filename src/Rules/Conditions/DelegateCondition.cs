// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using System;

namespace Axe.Windows.Rules
{
    class DelegateCondition : Condition
    {
        public delegate bool MatchesDelegate(IA11yElement element);
        private readonly MatchesDelegate _matches;

        public DelegateCondition(MatchesDelegate matches)
        {
            _matches = matches ?? throw new ArgumentNullException(nameof(matches));
        }

        public override bool Matches(IA11yElement element)
        {
            return _matches(element);
        }

        public override string ToString()
        {
            return "DelegateCondition";
        }
    } // class
} // namespace
