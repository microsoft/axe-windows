// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Misc;
using Axe.Windows.Rules.Resources;
using Axe.Windows.Telemetry;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Axe.Windows.Rules
{
    class RuleRunner
    {
        private readonly IRuleProvider _provider;

        public RuleRunner(IRuleProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public RunResult RunRuleByID(RuleId id, IA11yElement element)
        {
            var rule = _provider.GetRule(id);
            if (rule == null)
            {
                return new RunResult
                {
                    EvaluationCode = EvaluationCode.RuleExecutionError,
                    element = element,
                    ErrorMessage = ErrorMessages.NoRuleMatchingId.WithParameters(id)
                };
            }

            var retVal = RunRule(rule, element);

            return retVal;
        }

        public IEnumerable<RunResult> RunAll(IA11yElement element, CancellationToken cancellationToken)
        {
            var results = new List<RunResult>();

            foreach (var rule in _provider.All)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = RunRule(rule, element);
                results.Add(result);
            } // for all rules

            return results;
        }

        private static RunResult RunRule(IRule rule, IA11yElement element)
        {
            try
            {
                return RunRuleWorker(rule, element);
            }
#pragma warning disable CA1031
            catch (Exception ex)
#pragma warning restore CA1031
            {
                ex.ReportException();
                if (System.Diagnostics.Debugger.IsLogging())
                    System.Diagnostics.Debug.WriteLine(ex.ToString());

                return new RunResult
                {
                    RuleInfo = rule.Info,
                    element = element,
                    EvaluationCode = EvaluationCode.RuleExecutionError,
                    ErrorMessage = ex.ToString()
                };
            } // catch
        }

        private static RunResult RunRuleWorker(IRule rule, IA11yElement element)
        {
            var result = new RunResult
            {
                RuleInfo = rule.Info,
                element = element,
                EvaluationCode = EvaluationCode.NotApplicable
            };

            if (!ShouldRun(rule.Condition, element))
                return result;

            result.EvaluationCode = rule.PassesTest(element) ? EvaluationCode.Pass : rule.Info.ErrorCode;

            return result;
        }

        private static bool ShouldRun(Condition condition, IA11yElement element)
        {
            if (condition == null) return true;

            return condition.Matches(element);
        }
    } // class
} // namespace
