// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.OldFileVersionCompatibilityTests
{
    [TestClass]
    public class LoadOldFileVersions
    {
        [TestMethod]
        public void ValidateFileVersion_0_1_0()
        {
            const string filePath = @".\TestFiles\WildlifeManager_AxeWindows_0_1_0.a11ytest";
            const int expectedProcessId = 1880;
            const int expectedFailureCount = 12;
            ValidateOneFile(filePath, expectedFailureCount, expectedProcessId);
        }

        [TestMethod]
        public void ValidateFileVersion_0_2_0()
        {
            const string filePath = @".\TestFiles\WildlifeManager_AxeWindows_0_2_0.a11ytest";
            const int expectedProcessId = 17152;
            const int expectedFailureCount = 12;
            ValidateOneFile(filePath, expectedFailureCount, expectedProcessId);
        }

        [TestMethod]
        public void ValidateFileVersion_0_3_1()
        {
            const string filePath = @".\TestFiles\WildlifeManager_AxeWindows_0_3_1.a11ytest";
            const int expectedProcessId = 22236;
            const int expectedFailureCount = 12;
            ValidateOneFile(filePath, expectedFailureCount, expectedProcessId);
        }

        #region UtilityFunctions

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
            Assert.AreEqual(string.Empty, element.ProcessName);
            Assert.AreEqual(processId, element.ProcessId);
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
        #endregion
    }
}
