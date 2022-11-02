﻿// Copyright (c) Microsoft. All rights reserved.
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
        private readonly ContextDelegate Initialize;
        private readonly ContextDelegate Finalize;
        private readonly Condition Sub;

        public ContextCondition(Condition sub, ContextDelegate initialize, ContextDelegate finalize)
        {
            Sub = sub ?? throw new ArgumentNullException(nameof(sub));
            Initialize = initialize ?? throw new ArgumentNullException(nameof(initialize));
            Finalize = finalize ?? throw new ArgumentNullException(nameof(finalize));
        }

        public override bool Matches(IA11yElement e)
        {
            // Ensure the context is initialized
            Condition.InitContext();
            Initialize(e);

            bool retVal = false;

            try
            {
                retVal = Sub.Matches(e);
            }
            catch (Exception)
            {
                Finalize(e);
                throw;
            }

            Finalize(e);

            return retVal;
        }

        public override string ToString()
        {
            return Sub?.ToString();
        }
    } // class
} // namespace
