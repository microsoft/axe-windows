// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Axe.Windows.Rules
{
    interface IRuleProvider
    {
        IRule GetRule(RuleId id);
        IEnumerable<IRule> All { get; }
    }

    /// <summary>
    /// Supplies rules, ensuring that each rule is created only once, and only when requested.
    /// </summary>
    class RuleProvider : IRuleProvider
    {
        private readonly IRuleFactory RuleFactory;
        private readonly ConcurrentDictionary<RuleId, IRule> AllRules = new ConcurrentDictionary<RuleId, IRule>();
        private readonly Object AllRulesLock = new Object();
        private bool AreAllRulesInitialized;

        public RuleProvider(IRuleFactory ruleFactory)
        {
            this.RuleFactory = ruleFactory ?? throw new ArgumentNullException(nameof(ruleFactory));
        }

        private void InitAllRules()
        {
            if (AreAllRulesInitialized) return;

            /* the lock is an optimization of the ConcurrentDictionary
             * So that if multiple threads simultaneously run all rules
             * the ConcurrentDictionary won't end up creating extra objects that will never be used
             * and only be fodder for the garbage collector.
             * */
            lock (AllRulesLock)
            {
                if (AreAllRulesInitialized) return;

                // Calling GetRule ensures that the rule is created.
                foreach (var k in Axe.Windows.Rules.RuleFactory.RuleIds)
                    GetRule(k);

                AreAllRulesInitialized = true;
            } // lock
        }

        public IRule GetRule(RuleId id)
        {
            return AllRules.GetOrAdd(id, key => this.RuleFactory.CreateRule(key));
        }

        public IEnumerable<IRule> All
        {
            get
            {
                InitAllRules();

                return AllRules.Values;
            }
        }
    } // class
} // namespace
