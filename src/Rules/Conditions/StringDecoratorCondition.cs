// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Misc;
using System;
using System.Globalization;

namespace Axe.Windows.Rules
{
    /// <summary>
    /// This condition is used to change the output of the ToString function for a given condition.
    /// This can be useful for adding meaningful or simplified names to otherwise complicated conditions.
    /// </summary>
    class StringDecoratorCondition : Condition
    {
        private readonly Condition Sub;
        private readonly string Decoration;

        public StringDecoratorCondition(Condition c, string decoration)
        {
            this.Sub = c ?? throw new ArgumentNullException(nameof(c));
            this.Decoration = decoration ?? throw new ArgumentNullException(nameof(decoration));
        }

        public override bool Matches(IA11yElement element)
        {
            return this.Sub.Matches(element);
        }

        public override string ToString()
        {
            return this.Decoration.WithParameters(this.Sub);
        }
    } // class
} // namespace
