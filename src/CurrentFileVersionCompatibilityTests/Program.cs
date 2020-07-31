// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Results;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;


namespace CurrentFileVersionCompatibilityTests
{
    class Program
    {
        private enum ReturnValue
        {
            Success,
            BadUsage,
            InvalidData,
        }

        static int Main(string[] args)
        {
            try
            {
                if (args.Length != 3)
                {
                    Console.WriteLine("Bad Usage! Command line is:");
                    Console.WriteLine();
                    Console.WriteLine("OldFileVersionWriteTests <file> <expectedFailureCount> <expectedProcessId>");
                    Console.WriteLine();
                    Console.WriteLine("ErrorLevel is 0 on success, non-zero on error");
                    return (int)ReturnValue.BadUsage;
                }

                RequireSystemCollectionsImmutable(args);

                string filePath = args[0];
                int expectedFailureCount = int.Parse(args[1]);
                int expectedProcessId = int.Parse(args[2]);

                Console.WriteLine("Input file: {0}", filePath);
                Console.WriteLine("Expected Failure Count: {0}", expectedFailureCount);
                Console.WriteLine("Expected Process ID: {0}", expectedProcessId);
                ValidateOneFile(filePath, expectedFailureCount, expectedProcessId);
                Console.WriteLine("All tests passed");
                return (int)ReturnValue.Success;
            }
            catch (Exception e)
            {
                Console.WriteLine("\nCaught Exception: {0}", e.ToString());
                return (int)ReturnValue.InvalidData;
            }
        }

        // ------------------------------------------------------------------------
        // This code is nearly identical to what's in LoadOldFileVersions.cs. It's
        // just different enough to have a separate copy.
        // ------------------------------------------------------------------------

        internal static void ValidateOneFile(string filePath, int expectedFailureCount,
            int expectedProcessId)
        {
            // Ideally, we'd use a Using block for SelectAction. Unfortunately, the
            // current code doesn't support this properly. Use ClearDefaultInstance
            // instead.
            try
            {
                SelectAction sa = SelectAction.GetDefaultInstance();
                sa.SelectLoadedData(filePath);

                List<(RuleResult Result, A11yElement Element)> failedResults = ExtractFailedResults(sa);

                Assert.AreEqual(expectedFailureCount, failedResults.Count);

                foreach (var failedResult in failedResults)
                {
                    ValidateOneRuleResult(failedResult.Result);
                    ValidateOneA11yElement(expectedProcessId, failedResult.Element);
                }
            }
            finally
            {
                SelectAction.ClearDefaultInstance();
            }
        }

        private static void ValidateOneRuleResult(RuleResult result)
        {
            Assert.AreEqual(ScanStatus.Fail, result.Status);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Description));
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.HelpUrl.Url));
            Assert.IsNull(result.IssueDisplayText);
            Assert.IsNull(result.IssueLink);
            Assert.IsNotNull(result.MetaInfo);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Source));
            Assert.AreEqual(RuleId.Indecisive, result.Rule);

            foreach (string message in result.Messages)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(message));
            }
        }

        private static void ValidateOneA11yElement(int processId, A11yElement element)
        {
            // element.ProcessName is undefined in Axe.Windows 0.3.1 (fixed in 0.3.2)
            //Assert.AreEqual(string.Empty, element.ProcessName);
            Assert.AreEqual(processId, element.ProcessId);

            int thumb = 50027;
            if (element.ControlTypeId != thumb)
                Assert.IsFalse(element.BoundingRectangle.IsEmpty);

            Assert.IsFalse(string.IsNullOrWhiteSpace(element.ClassName));
            Assert.AreNotEqual(0, element.UniqueId);
            Assert.AreNotEqual(0, element.Properties.Count);
            Assert.AreNotEqual(0, element.Patterns.Count);
            Assert.AreEqual(0, element.PlatformProperties.Count);
        }

        /// <summary>
        /// Code to extract all A11yElement objects with failed results. Based on
        /// the code to display the failed results in Accessibility Insights for Windows.
        /// </summary>
        /// <param name="sa">The SelectAction that defines the context</param>
        private static List<(RuleResult, A11yElement)> ExtractFailedResults(SelectAction sa)
        {
            Guid ecId = sa.GetSelectedElementContextId().Value;
            ElementDataContext dataContext = GetDataAction.GetElementDataContext(ecId);

            List<(RuleResult, A11yElement)> list = new List<(RuleResult, A11yElement)>();
            foreach (var element in dataContext.Elements.Values)
            {
                if (element.ScanResults?.Items == null ||
                    element.ScanResults.Items.Count == 0)
                {
                    continue;
                }

                foreach (var item in element.ScanResults.Items)
                {
                    var failures =
                        from ruleResult in item.Items
                        where ruleResult.Status == ScanStatus.Fail || ruleResult.Status == ScanStatus.ScanNotSupported
                        select (ruleResult, element);
                    list.AddRange(failures);
                }
            }

            return list;
        }

        static void RequireSystemCollectionsImmutable(string[] args)
        {
            // This code exists to force a build break if we revise the version of
            // System.Collections.Immutable without upgrading the project reference
            var dependencyCheck = args.ToImmutableArray<string>();
            if (args.Length != dependencyCheck.Length)
            {
                throw new ArgumentException("This should never happen!", nameof(args));
            }
        }
    }
}
