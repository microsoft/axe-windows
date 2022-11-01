// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Exceptions;
using Axe.Windows.Core.Misc;
using Axe.Windows.Rules.Resources;
using System.Globalization;

namespace Axe.Windows.Rules
{
    interface IRule
    {
        RuleInfo Info { get; }
        Condition Condition { get; }
        bool PassesTest(IA11yElement element);
    }

    abstract class Rule : IRule
    {
        public RuleInfo Info { get; private set; }
        public Condition Condition { get; private set; }

        protected Rule()
        {
            // keep these two calls in the following order or the RuleInfo.Condition string won't get populated
#pragma warning disable CA2214
            Condition = CreateCondition();
#pragma warning restore CA2214

            InitRuleInfo();
        }

        private void InitRuleInfo()
        {
            var info = GetRuleInfoFromAttributes();
            if (info == null) throw new AxeWindowsException(ErrorMessages.MissingRuleInforAttribute.WithParameters(GetType().Name));

            info.Condition = Condition?.ToString();

            Info = info;
        }

        private RuleInfo GetRuleInfoFromAttributes()
        {
            var type = GetType();

            foreach (var a in type.GetCustomAttributes(true))
            {
                if (a.GetType() == typeof(RuleInfo))
                    return a as RuleInfo;
            }

            return null;
        }

        public abstract bool PassesTest(IA11yElement element);

        protected abstract Condition CreateCondition();
    }
} // namespace
