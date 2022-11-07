// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using System;

namespace Axe.Windows.Rules
{
    /// <summary>
    /// Handles the push and pop operations of setting context information.
    /// The context is a set of data that is stored during a rule run.
    /// It can be accessed by other conditions.
    /// </summary>
    class ContextCondition : Condition
    {
        public delegate void ContextDelegate(IA11yElement element);
        private readonly ContextDelegate _initialize;
        private readonly ContextDelegate _finalize;
        private readonly Condition _sub;

        public ContextCondition(Condition sub, ContextDelegate initialize, ContextDelegate finalize)
        {
            _sub = sub ?? throw new ArgumentNullException(nameof(sub));
            _initialize = initialize ?? throw new ArgumentNullException(nameof(initialize));
            _finalize = finalize ?? throw new ArgumentNullException(nameof(finalize));
        }

        public override bool Matches(IA11yElement e)
        {
            // Ensure the context is initialized
            Condition.InitContext();
            _initialize(e);

            bool retVal = false;

            try
            {
                retVal = _sub.Matches(e);
            }
            catch (Exception)
            {
                _finalize(e);
                throw;
            }

            _finalize(e);

            return retVal;
        }

        public override string ToString()
        {
            return _sub?.ToString();
        }
    } // class
} // namespace
