// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.HelpLinks;
using Axe.Windows.Core.Results;
using Axe.Windows.Rules;
using Axe.Windows.RuleSelection.Resources;
using Axe.Windows.Telemetry;
using System;
using System.Threading;

namespace Axe.Windows.RuleSelection
{
    /// <summary>
    /// Runs rules from Axe.Windows.Rules and adapts the results to the RuleResults format
    /// </summary>
    public static class RuleRunner
    {
        public static void Run(A11yElement e, CancellationToken cancellationToken)
        {
            try
            {
                RunUnsafe(e, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                ex.ReportException();
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        private static void RunUnsafe(A11yElement e, CancellationToken cancellationToken)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            if (e.ScanResults == null)
                e.ScanResults = new ScanResults();

            Run(e.ScanResults, e, cancellationToken);
        }

        private static void Run(ScanResults results, A11yElement e, CancellationToken cancellationToken)
        {
            var runResults = Axe.Windows.Rules.Rules.RunInclusionRules(e, cancellationToken);
            foreach (var r in runResults)
            {
                if (r.EvaluationCode == EvaluationCode.NotApplicable) continue;

                ConvertAndAddNonExcludedRunResult(results, r);
            } // for each
        }

        public static bool ExcludeFromRun(A11yElement e, CancellationToken cancellationToken)
        {
            try
            {
                return ExcludeFromRunUnsafe(e, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                ex.ReportException();
                return false;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        private static bool ExcludeFromRunUnsafe(A11yElement e, CancellationToken cancellationToken)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            if (e.ScanResults == null)
                e.ScanResults = new ScanResults();

            return ExcludeFromRun(e.ScanResults, e, cancellationToken);
        }

        private static bool ExcludeFromRun(ScanResults results, A11yElement e, CancellationToken cancellationToken)
        {
            var runResults = Axe.Windows.Rules.Rules.RunExclusionRules(e, cancellationToken);
            bool exclude = false;
            foreach (var r in runResults)
            {
                if (r.EvaluationCode == EvaluationCode.Error)
                {
                    exclude = true;
                }

                ConvertAndAddNonExcludedRunResult(results, r);
            } // for each
            return exclude;
        }

        private static ScanResult ConvertRunResultToScanResult(RunResult runResult)
        {
            if (runResult == null) throw new ArgumentNullException(nameof(runResult));
            if (runResult.RuleInfo == null) throw new ArgumentException(ErrorMessages.RunResultRuleInfoNull, nameof(runResult));

            var scanResult = CreateResult(runResult.RuleInfo, runResult.element);

            var description = runResult.RuleInfo.Description;
            var ruleResult = scanResult.GetRuleResultInstance(runResult.RuleInfo.ID, description);
            ruleResult.Status = ConvertEvaluationCodeToScanStatus(runResult.EvaluationCode);
            ruleResult.AddMessage(runResult.RuleInfo.HowToFix);

            return scanResult;
        }

        public static ScanResult CreateResult(RuleInfo info, IA11yElement e)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));
            if (e == null) throw new ArgumentNullException(nameof(e));

            var (ShortDescription, Url) = ReferenceLinks.GetGuidelineInfo(info.Standard);
            var scanResult = new ScanResult(info.Description, ShortDescription, info.FrameworkIssueLink, e, info.PropertyID)
            {
                HelpUrl = new HelpUrl
                {
                    Url = Url,
                    Type = UrlType.Info
                }
            };

            return scanResult;
        }

        public static ScanStatus ConvertEvaluationCodeToScanStatus(EvaluationCode evaluationCode)
        {
            switch (evaluationCode)
            {
                case EvaluationCode.RuleExecutionError:
                    return ScanStatus.ScanNotSupported;
                case EvaluationCode.Error:
                    return ScanStatus.Fail;
                case EvaluationCode.NeedsReview:
                case EvaluationCode.Warning:
                    return ScanStatus.Uncertain;
                case EvaluationCode.NotApplicable:
                    return ScanStatus.Pass; // to avoid error in UI.
            } // switch

            return ScanStatus.Pass;
        }

        private static void ConvertAndAddNonExcludedRunResult(ScanResults results, RunResult runResult)
        {
            if (runResult.IncludeInResults)
            {
                ScanResult scanResult = ConvertRunResultToScanResult(runResult);
                results.AddScanResult(scanResult);
            }
        }

        public static string RuleVersion => RuleVersions.Version;
    } // class
} // namespace
