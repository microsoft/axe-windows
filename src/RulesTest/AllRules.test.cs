// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.RulesTests
{
    [TestClass]
    public class AllRules
    {
        [TestMethod]
        public void AllRulesHaveErrorCode()
        {
            foreach (var rule in Rules.Rules.All.Values)
                Assert.AreNotEqual(EvaluationCode.NotSet, rule.ErrorCode);
        }
    } // class
} // namespace
