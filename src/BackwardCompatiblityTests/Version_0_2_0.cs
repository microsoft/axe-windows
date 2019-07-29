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

namespace Axe.Windows.BackwardCompatiblityTests
{
    [TestClass]
    public class Version_0_2_0
    {
        [TestMethod]
        public void ValidateFile()
        {
            using (SelectAction sa = SelectAction.GetDefaultInstance())
            {
                const int savedProcessId = 1880;
                const int knownFailuresInFile = 12;

                sa.SelectLoadedData(@".\TestFiles\WildlifeManager_AxeWindows_0_2_0.a11ytest");

                List<Tuple<RuleResult, A11yElement>> tuples = ExtractFailedResults(sa);

                Assert.AreEqual(knownFailuresInFile, tuples.Count);

                foreach (var tuple in tuples)
                {
                    ValidateOneRuleResult(tuple.Item1);
                    ValidateOneA11yElement(savedProcessId, tuple.Item2);
                }
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
        /// the code to display the failed results in Accessibiltiy Insights for Windows.
        /// </summary>
        /// <param name="sa">The SelectAction that defines the context</param>
        private static List<Tuple<RuleResult, A11yElement>> ExtractFailedResults(SelectAction sa)
        {
            Guid ecId = sa.GetSelectedElementContextId().Value;
            ElementDataContext dataContext = GetDataAction.GetElementDataContext(ecId);

            List<Tuple<RuleResult, A11yElement>> list = new List<Tuple<RuleResult, A11yElement>>();
            foreach (var element in dataContext.Elements.Values)
            {
                if (element.ScanResults?.Items != null && element.ScanResults.Items.Count > 0)
                {
                    foreach (var item in element.ScanResults.Items)
                    {
                        var failures = from rr in item.Items
                                       where rr.Status == ScanStatus.Fail || rr.Status == ScanStatus.ScanNotSupported
                                       select new Tuple<RuleResult, A11yElement>(rr, element);
                        list.AddRange(failures);
                    }
                }
            }

            return list;
        }
    }
}
