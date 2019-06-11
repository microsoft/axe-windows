// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.AutomationTests
{
    [TestClass()]
    public class ScanResultsAssemblerTests
    {
        [TestMethod]
        [Timeout(2000)]
        public void AssembleScanResultss_AssemblesErrors()
        {
            A11yElement element = UnitTestSharedLibrary.Utility.LoadA11yElementsFromJSON("Snapshots/MonsterEdit.snapshot");

            element.ScanResults.Items[0].Items = new List<RuleResult>()
            {
                new RuleResult() { Status = ScanStatus.Fail, Rule = RuleId.ControlViewButtonStructure }
            };
            element.Children[0].ScanResults.Items[0].Items = new List<RuleResult>()
            {
                new RuleResult() { Status = ScanStatus.Fail, Rule = RuleId.BoundingRectangleNotNull }
            };

            var errors = new List<Automation.ScanResult>();
            int count = ScanResultsAssembler.AssembleScanResults(errors, element, null);

            // there should be 2 errors
            Assert.AreEqual(2, count);
            Assert.AreEqual(2, errors.Count);

            // the two results should have the correct rule results
            Assert.AreEqual(Rules.Rules.All[RuleId.ControlViewButtonStructure], errors[0].Rule);
            Assert.AreEqual(Rules.Rules.All[RuleId.BoundingRectangleNotNull], errors[1].Rule);

            // the second error's parent is the same as the first error's element
            Assert.AreEqual(errors[0].Element, errors[1].Element.Parent);
        }


        [TestMethod]
        [Timeout(2000)]
        public void AssembleScanResults_AssemblesNoErrors()
        {
            A11yElement element = UnitTestSharedLibrary.Utility.LoadA11yElementsFromJSON("Snapshots/MonsterEdit.snapshot");

            var errors = new List<Automation.ScanResult>();
            int count = ScanResultsAssembler.AssembleScanResults(errors, element, null);

            // if there were no rule violations, there should be no results.
            Assert.AreEqual(0, count);
            Assert.AreEqual(0, errors.Count);
        }

        [TestMethod]
        [Timeout(2000)]
        public void AssembleScanResults_ThrowsArgumentException()
        {
            A11yElement element = new A11yElement() { ScanResults = null };

            var errors = new List<Automation.ScanResult>();

            Assert.ThrowsException<ArgumentException>(() => ScanResultsAssembler.AssembleScanResults(errors, element, null));
        }

        [TestMethod]
        [Timeout(2000)]
        public void GetFailedRuleResultsFromElement_ReturnsRuleResults()
        {
            A11yElement element = new A11yElement() { ScanResults = new Core.Results.ScanResults() };

            element.ScanResults.Items.Add(new Core.Results.ScanResult()
            {
                Items = new List<RuleResult>()
                {
                    new RuleResult() { Status = ScanStatus.Pass, Rule = RuleId.IsKeyboardFocusable },
                    new RuleResult() { Status = ScanStatus.Uncertain, Rule = RuleId.BoundingRectangleNotNull },
                    new RuleResult() { Status = ScanStatus.NoResult, Rule = RuleId.ListItemSiblingsUnique },
                    new RuleResult() { Status = ScanStatus.Fail, Rule = RuleId.BoundingRectangleCompletelyObscuresContainer },
                }
            });

            element.ScanResults.Items.Add(new Core.Results.ScanResult()
            {
                Items = new List<RuleResult>()
                {
                    new RuleResult() { Status = ScanStatus.Fail, Rule = RuleId.ControlViewButtonStructure },
                    new RuleResult() { Status = ScanStatus.Pass, Rule = RuleId.TypicalTreeStructureRaw },
                    new RuleResult() { Status = ScanStatus.Fail, Rule = RuleId.LandmarkNoDuplicateContentInfo },
                    new RuleResult() { Status = ScanStatus.NoResult, Rule = RuleId.PatternsSupportedByControlType },
                    new RuleResult() { Status = ScanStatus.Fail, Rule = RuleId.BoundingRectangleCompletelyObscuresContainer },
                }
            });

            var results = ScanResultsAssembler.GetFailedRuleResultsFromElement(element).ToList();

            Assert.AreEqual(4, results.Count);
            Assert.AreEqual(RuleId.BoundingRectangleCompletelyObscuresContainer, results[0].Rule);
            Assert.AreEqual(RuleId.ControlViewButtonStructure, results[1].Rule);
            Assert.AreEqual(RuleId.LandmarkNoDuplicateContentInfo, results[2].Rule);
            Assert.AreEqual(RuleId.BoundingRectangleCompletelyObscuresContainer, results[3].Rule);
        }

        [TestMethod]
        [Timeout(2000)]
        public void MakeElementInfoFromElement_ReturnsElementInfo()
        {
            A11yElement element = UnitTestSharedLibrary.Utility.LoadA11yElementsFromJSON("Snapshots/MonsterEdit.snapshot");

            ElementInfo parentInfo = new ElementInfo
            {
                Patterns = element.Patterns.ConvertAll<string>(x => x.Name),
                Properties = element.Properties.ToDictionary(p => p.Value.Name, p => p.Value.TextValue)
            };

            ElementInfo expectedInfo = new ElementInfo
            {
                Patterns = element.Children[0].Patterns.ConvertAll<string>(x => x.Name),
                Properties = element.Children[0].Properties.ToDictionary(p => p.Value.Name, p => p.Value.TextValue)
            };

            var actualInfo = ScanResultsAssembler.MakeElementInfoFromElement(element.Children[0], parentInfo);

            Assert.IsTrue(expectedInfo.Patterns.SequenceEqual(actualInfo.Patterns));
            Assert.IsTrue(expectedInfo.Properties.SequenceEqual(actualInfo.Properties));
            Assert.IsTrue(parentInfo.Properties.SequenceEqual(actualInfo.Parent.Properties));
            Assert.IsTrue(parentInfo.Properties.SequenceEqual(actualInfo.Parent.Properties));
        }
    }
}