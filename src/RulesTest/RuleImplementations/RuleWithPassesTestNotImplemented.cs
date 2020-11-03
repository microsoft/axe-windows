// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules;
using System;

namespace RulesTests.RuleImplementations
{
    [RuleInfo(ID = default(RuleId))]
    class RuleWithPassesTestNotImplemented : Rule
    {
        public RuleWithPassesTestNotImplemented() : base()
        {
            // We have to set the following field so that PassesTest in the base class will be called
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override EvaluationCode Evaluate(IA11yElement element)
        {
            // Don't throw an exception here so we can be sure the only exception thrown is in Rule.PassesTest
            return EvaluationCode.Pass;
        }

        protected override Condition CreateCondition()
        {
            return Condition.True;
        }
    } // class
} // namespace
