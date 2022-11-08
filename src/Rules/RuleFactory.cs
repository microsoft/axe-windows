// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Axe.Windows.Rules
{
    class RuleFactory : IRuleFactory
    {
        private static readonly IReadOnlyDictionary<RuleId, Type> RuleTypes = CreateRuleTypes();
        private static readonly Lazy<IEnumerable<RuleId>> RuleIdsLazy = new Lazy<IEnumerable<RuleId>>(() => RuleTypes.Keys);
        public static IEnumerable<RuleId> RuleIds => RuleIdsLazy.Value;

        private static IReadOnlyDictionary<RuleId, Type> CreateRuleTypes()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var types = assembly.GetTypes();

            var ruleTypes = from t in types
                            where t.BaseType == typeof(Rule)
                            select t;

            return ruleTypes.ToDictionary(t =>
            {
                var info = (RuleInfo)t.GetCustomAttribute(typeof(RuleInfo));
                if (info == null) throw new InvalidOperationException(ErrorMessages.RuleInfoAttributeExpected);

                return info.ID;
            });
        }

        public IRule CreateRule(RuleId id)
        {
            if (!RuleTypes.TryGetValue(id, out Type type))
                return null;

            return Activator.CreateInstance(type) as IRule;
        }
    } // class
} // namespace
