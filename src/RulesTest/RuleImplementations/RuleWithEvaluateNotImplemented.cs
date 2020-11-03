// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules;
using System;

namespace RulesTests
{
    /*
    This class can be removed once every rule in the assembly has been converted to calling PassesTest instead of Evaluate
    */
    [RuleInfo(ID = default(RuleId))]
    class RuleWithEvaluateNotImplemented : Rule
    {
        protected override Condition CreateCondition()
        {
            return Condition.True;
        }

        public override bool PassesTest(IA11yElement element)
        {
            // Don't throw an exception here so we can be sure the only exception thrown is in Rule.Evaluate
            return true;
        }
    } // class
} // namespace
