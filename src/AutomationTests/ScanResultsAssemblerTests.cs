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
        ScanResultsAssembler assembler = new ScanResultsAssembler();

        [TestMethod]
        [Timeout(2000)]
        public void AssembleScanResultsFromElement_AssemblesErrors()
        {
            A11yElement element = GenerateA11yElementWithChild();
            List<RuleId> expectedRuleIDs = GetExpectedRuleIDs();

            ElementInfo expectedParentInfo = new ElementInfo
            {
                Patterns = element.Patterns.ConvertAll<string>(x => x.Name),
                Properties = element.Properties.ToDictionary(p => p.Value.Name, p => p.Value.TextValue)
            };

            ElementInfo expectedChildInfo = new ElementInfo
            {
                Patterns = element.Children[0].Patterns.ConvertAll<string>(x => x.Name),
                Properties = element.Children[0].Properties.ToDictionary(p => p.Value.Name, p => p.Value.TextValue)
            };

            var scanResults = assembler.AssembleScanResultsFromElement(element);

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
                Assert.IsTrue(expectedParentInfo.Properties.SequenceEqual(error.Element.Properties));
                Assert.IsTrue(expectedParentInfo.Patterns.SequenceEqual(error.Element.Patterns));
            }

            // the patterns for the child errors and their parents should be as expected 
            foreach (var error in childErrors)
            {
                Assert.IsTrue(expectedChildInfo.Patterns.SequenceEqual(error.Element.Patterns));
                Assert.IsTrue(expectedChildInfo.Properties.SequenceEqual(error.Element.Properties));
                Assert.IsTrue(expectedParentInfo.Properties.SequenceEqual(error.Element.Parent.Properties));
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

            var scanResults = assembler.AssembleScanResultsFromElement(element);

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

        private A11yElement GenerateA11yElementWithChild()
        {
            A11yElement element = new A11yElement()
            {
                ScanResults = new Core.Results.ScanResults(),
                Children = new List<A11yElement>(),
            };

            element.Properties = GetFillerProperties();
            element.Patterns = GetFillerPatterns();
            element.ScanResults.Items.AddRange(GetFillerScanResults());
            element.Children.Add(GenerateA11yElementWithoutChild());

            return element;
        }
        private A11yElement GenerateA11yElementWithoutChild()
        {
            A11yElement element = new A11yElement() { ScanResults = new Core.Results.ScanResults() };

            element.Properties = GetFillerProperties();
            element.Patterns = GetFillerPatterns();
            element.ScanResults.Items.AddRange(GetFillerScanResults());

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

        private List<Core.Results.ScanResult> GetFillerScanResults() => new List<Core.Results.ScanResult>()
        {
            new Core.Results.ScanResult()
            {
                Items = new List<RuleResult>()
                {
                    new RuleResult() { Status = ScanStatus.Pass, Rule = RuleId.IsKeyboardFocusable },
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