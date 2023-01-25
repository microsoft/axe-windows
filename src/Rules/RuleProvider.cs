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
        IEnumerable<IRule> ExclusionRules { get; }
        IEnumerable<IRule> IncludedRules { get; }
    }

    /// <summary>
    /// Supplies rules, ensuring that each rule is created only once, and only when requested.
    /// </summary>
    class RuleProvider : IRuleProvider
    {
        private readonly IRuleFactory _ruleFactory;
        private readonly ConcurrentDictionary<RuleId, IRule> _allRules = new ConcurrentDictionary<RuleId, IRule>();
        // Rules that, if violated, cause an element to be excluded from the scan.
        private readonly ConcurrentDictionary<RuleId, IRule> _exclusionRules = new ConcurrentDictionary<RuleId, IRule>();
        // Rules that are not exclusionary.
        private readonly ConcurrentDictionary<RuleId, IRule> _includedRules = new ConcurrentDictionary<RuleId, IRule>();
        private readonly object _allRulesLock = new object();
        private bool _areAllRulesInitialized;

        public RuleProvider(IRuleFactory ruleFactory)
        {
            _ruleFactory = ruleFactory ?? throw new ArgumentNullException(nameof(ruleFactory));
        }

        private void InitAllRules()
        {
            if (_areAllRulesInitialized) return;

            /* the lock is an optimization of the ConcurrentDictionary
             * So that if multiple threads simultaneously run all rules
             * the ConcurrentDictionary won't end up creating extra objects that will never be used
             * and only be fodder for the garbage collector.
             * */
            lock (_allRulesLock)
            {
                if (_areAllRulesInitialized) return;

                foreach (var k in Axe.Windows.Rules.RuleFactory.RuleIds)
                {
                    if (!_allRules.ContainsKey(k))
                    {
                        var rule = _ruleFactory.CreateRule(k);
                        if (rule.Exclusionary)
                        {
                            _exclusionRules.TryAdd(k, rule);
                        }
                        else
                        {
                            _includedRules.TryAdd(k, rule);
                        }
                        _allRules.TryAdd(k, rule);
                    }
                }

                _areAllRulesInitialized = true;
            } // lock
        }

        public IRule GetRule(RuleId id)
        {
            return _allRules.GetOrAdd(id, key => _ruleFactory.CreateRule(key));
        }

        public IEnumerable<IRule> All
        {
            get
            {
                InitAllRules();

                return _allRules.Values;
            }
        }

        public IEnumerable<IRule> ExclusionRules
        {
            get
            {
                InitAllRules();

                return _exclusionRules.Values;
            }
        }

        public IEnumerable<IRule> IncludedRules
        {
            get
            {
                InitAllRules();

                return _includedRules.Values;
            }
        }
    } // class
} // namespace
