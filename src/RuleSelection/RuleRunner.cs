// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Exceptions;
using Axe.Windows.Core.HelpLinks;
using Axe.Windows.Core.Results;
using Axe.Windows.Rules;
using Axe.Windows.RuleSelection.Resources;
using Axe.Windows.Telemetry;
using System;
using static System.FormattableString;

namespace Axe.Windows.RuleSelection
{
    /// <summary>
    /// Runs rules from Axe.Windows.Rules and adapts the results to the RuleResults format
    /// </summary>
    public static class RuleRunner
    {
        public static void Run(A11yElement e)
        {
            try
            {
                RunUnsafe(e);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                ex.ReportException();
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        private static void RunUnsafe(A11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            if (e.ScanResults == null)
                e.ScanResults = new ScanResults();

            Run(e.ScanResults, e);
        }

        private static void Run(ScanResults results, A11yElement e)
        {
            var runResults = Axe.Windows.Rules.Rules.RunAll(e);
            foreach (var r in runResults)
            {
                if (r.EvaluationCode == EvaluationCode.NotApplicable) continue;

                var scanResult = ConvertRunResultToScanResult(r);
                results.AddScanResult(scanResult);
            } // for each
        }

        private static ScanResult ConvertRunResultToScanResult(RunResult runResult)
        {
            if (runResult == null) throw new ArgumentNullException(nameof(runResult));
            if (runResult.RuleInfo == null) throw new ArgumentException(ErrorMessages.RunResultRuleInfoNull, nameof(runResult));

            var scanResult = CreateResult(runResult.RuleInfo, runResult.element);

            var description = runResult.RuleInfo.Description;
            var ruleResult = scanResult.GetRuleResultInstance(runResult.RuleInfo.ID, description, runResult.RuleInfo.FrameworkIssueLink);
            ruleResult.Status = ConvertEvaluationCodeToScanStatus(runResult.EvaluationCode);
            ruleResult.AddMessage(runResult.RuleInfo.HowToFix);

            return scanResult;
        }

        public static ScanResult CreateResult(RuleInfo info, IA11yElement e)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));
            if (e == null) throw new ArgumentNullException(nameof(e));

            var guidelineInfo = ReferenceLinks.GetGuidelineInfo(info.Standard);
            var scanResult = new ScanResult(info.Description, guidelineInfo.ShortDescription, e, info.PropertyID)
            {
                HelpUrl = new HelpUrl
                {
                    Url = guidelineInfo.Url,
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

        public static string RuleVersion => RuleVersions.Version;

        private static Exception Error(string message)
        {
#if DEBUG
            var callStack = new System.Diagnostics.StackFrame(1, true);
            return new AxeWindowsException(Invariant($"{message} in {callStack.GetMethod()} at line {callStack.GetFileLineNumber()} in {callStack.GetFileName()}"));
#else
            return new AxeWindowsException(message);
#endif
        }
    } // class
} // namespace
