// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Results;
using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.AutomationTests
{
    [TestClass()]
    public class ScanResultsAssemblerTests
    {
        readonly ScanResultsAssembler assembler = new ScanResultsAssembler();

        [TestMethod]
        [Timeout(2000)]
        public void AssembleScanResultsFromElement_AssemblesErrors()
        {
            A11yElement element = GenerateA11yElementWithChild();
            List<RuleId> expectedRuleIDs = GetExpectedRuleIDs();

            ElementInfo expectedParentInfo = new ElementInfo
            {
                Patterns = element.Patterns.ToList().ConvertAll<string>(x => x.Name),
                Properties = null,
            };

            ElementInfo expectedChildInfo = new ElementInfo
            {
                Patterns = null,
                Properties = element.Children[0].Properties.ToDictionary(p => p.Value.Name, p => p.Value.TextValue),
            };

            var scanResults = assembler.AssembleWindowScanOutputFromElement(element);

            var errors = scanResults.Errors.ToList();

            // the first half of the results are from the parent element; the second half are from the child element
            // we'll separate them here to improve the clarity of later validations
            var parentErrors = errors.GetRange(0, errors.Count / 2);
            var childErrors = errors.GetRange(errors.Count / 2, errors.Count / 2);

            // there should be the proper number of errors
            Assert.AreEqual(8, errors.Count);
            Assert.AreEqual(8, scanResults.ErrorCount);

            // the patterns for the parent errors should be as expected
            foreach (var error in parentErrors)
            {
                Assert.AreEqual(expectedParentInfo.Properties, error.Element.Properties);
                Assert.IsTrue(expectedParentInfo.Patterns.SequenceEqual(error.Element.Patterns));
            }

            // the patterns for the child errors and their parents should be as expected
            foreach (var error in childErrors)
            {
                Assert.AreEqual(expectedChildInfo.Patterns, error.Element.Patterns);
                Assert.IsTrue(expectedChildInfo.Properties.SequenceEqual(error.Element.Properties));
                Assert.AreEqual(expectedParentInfo.Properties, error.Element.Parent.Properties);
                Assert.IsTrue(expectedParentInfo.Patterns.SequenceEqual(error.Element.Parent.Patterns));
            }

            // the errors should have the correct rule results
            for (int x = 0; x < errors.Count; x++)
            {
                Assert.AreEqual(Rules.Rules.All[expectedRuleIDs[x]], errors[x].Rule, $"Error number {x}");
            }

            // the elements from the child element's parents are the same as the elements from the parent elements
            for (int x = 0; x < errors.Count / 2; x++)
            {
                Assert.AreEqual(parentErrors[x].Element, childErrors[x].Element.Parent, $"Error number {x}");
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void AssembleScanResultsFromElement_AssemblesNoErrors()
        {
            A11yElement element = UnitTestSharedLibrary.Utility.LoadA11yElementsFromJSON("Snapshots/MonsterEdit.snapshot");

            var scanResults = assembler.AssembleWindowScanOutputFromElement(element);

            // if there were no rule violations, there should be no results.
            Assert.AreEqual(0, scanResults.ErrorCount);
            Assert.AreEqual(0, scanResults.Errors.Count());
        }

        [TestMethod]
        [Timeout(1000)]
        public void AssembleScanResults_ThrowsArgumentNullException_NullErrors()
        {
            A11yElement element = new A11yElement() { ScanResults = null };

            Assert.ThrowsException<ArgumentNullException>(() => ScanResultsAssembler.AssembleErrorsFromElement(null, element, null));
        }

        [TestMethod]
        [Timeout(1000)]
        public void AssembleScanResults_ThrowsArgumentNullException_NullElement()
        {
            var errors = new List<Automation.ScanResult>();

            Assert.ThrowsException<ArgumentNullException>(() => ScanResultsAssembler.AssembleErrorsFromElement(errors, null, null));
        }

        [TestMethod]
        [Timeout(1000)]
        public void AssembleScanResults_ThrowsKeyNotFoundException_BadResults()
        {
            A11yElement element = GenerateA11yElementWithBadScanResults();
            var errors = new List<Automation.ScanResult>();

            Assert.ThrowsException<KeyNotFoundException>(() => ScanResultsAssembler.AssembleErrorsFromElement(errors, element, null));
        }

        private List<RuleId> GetExpectedRuleIDs() => new List<RuleId>()
        {
            RuleId.BoundingRectangleCompletelyObscuresContainer,
            RuleId.ControlViewButtonStructure ,
            RuleId.LandmarkNoDuplicateContentInfo ,
            RuleId.BoundingRectangleCompletelyObscuresContainer,
            RuleId.BoundingRectangleCompletelyObscuresContainer,
            RuleId.ControlViewButtonStructure ,
            RuleId.LandmarkNoDuplicateContentInfo ,
            RuleId.BoundingRectangleCompletelyObscuresContainer,
        };

        private void AddScanResults(IList<Core.Results.ScanResult> target, IList<Core.Results.ScanResult> itemsToAdd)
        {
            foreach (var item in itemsToAdd)
            {
                target.Add(item);
            }
        }

        private A11yElement GenerateA11yElementWithChild()
        {
            A11yElement element = new A11yElement
            {
                ScanResults = new Core.Results.ScanResults(),
                Children = new List<A11yElement>(),
                Properties = null,
                Patterns = GetFillerPatterns()
            };
            AddScanResults(element.ScanResults.Items, GetFillerScanResults());
            element.Children.Add(GenerateA11yElementWithoutChild());

            return element;
        }
        private A11yElement GenerateA11yElementWithoutChild()
        {
            A11yElement element = new A11yElement
            {
                ScanResults = new Core.Results.ScanResults(),
                Properties = GetFillerProperties(),
                Patterns = null
            };
            AddScanResults(element.ScanResults.Items, GetFillerScanResults());

            return element;
        }

        private A11yElement GenerateA11yElementWithBadScanResults()
        {
            A11yElement element = new A11yElement() { ScanResults = new Core.Results.ScanResults() };

            AddScanResults(element.ScanResults.Items, GetBadScanResults());

            return element;
        }

        private Dictionary<int, A11yProperty> GetFillerProperties() => new Dictionary<int, A11yProperty>()
        {
            { PropertyType.UIA_AriaRolePropertyId, new A11yProperty(PropertyType.UIA_AriaRolePropertyId, "Nice property")},
            { PropertyType.UIA_CulturePropertyId, new A11yProperty(PropertyType.UIA_CulturePropertyId, "A culture")},
            { PropertyType.UIA_DescribedByPropertyId, new A11yProperty(PropertyType.UIA_DescribedByPropertyId, "Descriptor")},
        };

        private List<A11yPattern> GetFillerPatterns() => new List<A11yPattern>()
        {
            new A11yPattern() { Name = "Pat" },
            new A11yPattern() { Name = "Tern" },
        };

        private List<Core.Results.ScanResult> GetBadScanResults() => new List<Core.Results.ScanResult>()
        {
            new Core.Results.ScanResult()
            {
                Items = new List<RuleResult>()
                {
                    new RuleResult() { Status = ScanStatus.Uncertain, Rule = RuleId.BoundingRectangleNotNull },
                    new RuleResult() { Status = ScanStatus.Fail, Rule = RuleId.Indecisive },
                    new RuleResult() { Status = ScanStatus.NoResult, Rule = RuleId.ListItemSiblingsUnique },
                    new RuleResult() { Status = ScanStatus.Fail, Rule = RuleId.BoundingRectangleCompletelyObscuresContainer },
                }
            }
        };

        private List<Core.Results.ScanResult> GetFillerScanResults() => new List<Core.Results.ScanResult>()
        {
            new Core.Results.ScanResult()
            {
                Items = new List<RuleResult>()
                {
                    new RuleResult() { Status = ScanStatus.Pass, Rule = RuleId.Indecisive },
                    new RuleResult() { Status = ScanStatus.Uncertain, Rule = RuleId.BoundingRectangleNotNull },
                    new RuleResult() { Status = ScanStatus.NoResult, Rule = RuleId.ListItemSiblingsUnique },
                    new RuleResult() { Status = ScanStatus.Fail, Rule = RuleId.BoundingRectangleCompletelyObscuresContainer },
                }
            },
            new Core.Results.ScanResult()
            {
                Items = new List<RuleResult>()
                {
                    new RuleResult() { Status = ScanStatus.Fail, Rule = RuleId.ControlViewButtonStructure },
                    new RuleResult() { Status = ScanStatus.Pass, Rule = RuleId.TypicalTreeStructureRaw },
                    new RuleResult() { Status = ScanStatus.Fail, Rule = RuleId.LandmarkNoDuplicateContentInfo },
                    new RuleResult() { Status = ScanStatus.NoResult, Rule = RuleId.PatternsSupportedByControlType },
                    new RuleResult() { Status = ScanStatus.Fail, Rule = RuleId.BoundingRectangleCompletelyObscuresContainer },
                }
            }
        };
    }
}
