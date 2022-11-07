// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Misc;
using System;

namespace Axe.Windows.Rules
{
    /// <summary>
    /// This condition is used to change the output of the ToString function for a given condition.
    /// This can be useful for adding meaningful or simplified names to otherwise complicated conditions.
    /// </summary>
    class StringDecoratorCondition : Condition
    {
        private readonly Condition _sub;
        private readonly string _decoration;

        public StringDecoratorCondition(Condition c, string decoration)
        {
            _sub = c ?? throw new ArgumentNullException(nameof(c));
            _decoration = decoration ?? throw new ArgumentNullException(nameof(decoration));
        }

        public override bool Matches(IA11yElement element)
        {
            return _sub.Matches(element);
        }

        public override string ToString()
        {
            return _decoration.WithParameters(_sub);
        }
    } // class
} // namespace
