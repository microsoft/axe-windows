// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Axe.Windows.Rules
{
    /// <summary>
    /// Use this class to run rules and get information about any rule in the Rules assembly.
    /// </summary>
#pragma warning disable CA1724
    public static class Rules
#pragma warning restore CA1724
    {
        private static readonly IRuleProvider Provider = new RuleProvider(new RuleFactory());
        private static readonly RuleRunner Runner = new RuleRunner(Provider);

        // the following is rarely used because its purpose is just to contain metadata about the rules
        private static readonly Lazy<IReadOnlyDictionary<RuleId, RuleInfo>> AllInfo = new Lazy<IReadOnlyDictionary<RuleId, RuleInfo>>(CreateRuleInfo);

        /// <summary>
        /// A dictionary containing metadata for all the rules in the Rules assembly.
        /// </summary>
        public static IReadOnlyDictionary<RuleId, RuleInfo> All => AllInfo.Value;

        private static IReadOnlyDictionary<RuleId, RuleInfo> CreateRuleInfo()
        {
            var dictionary = new Dictionary<RuleId, RuleInfo>();
            foreach (var rule in Provider.All)
                dictionary.Add(rule.Info.ID, rule.Info);

            return dictionary;
        }

        /// <summary>
        /// Run a single rule based on the given RuleID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static RunResult RunRuleByID(RuleId id, IA11yElement element)
        {
            return Runner.RunRuleByID(id, element);
        }

        /// <summary>
        /// Run all the rules in the Rules assembly.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IEnumerable<RunResult> RunAll(IA11yElement element, CancellationToken? cancellationToken)
        {
            return Runner.RunAll(element, cancellationToken);
        }
    } // class
} // namespace
