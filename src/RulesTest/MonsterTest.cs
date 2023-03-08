// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.UnitTestSharedLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EvaluationCode = Axe.Windows.Rules.EvaluationCode;

namespace Axe.Windows.RulesTests
{
    [TestClass]
    public class MonsterTest
    {
        [TestMethod()]
        public void MonsterButtonTest()
        {
            A11yElement e = Utility.LoadA11yElementsFromJSON("Snapshots/MonsterButton.snapshot");
            var results = GetTestResultsAsDictionary(e);

            CheckResults(results, new Dictionary<RuleId, EvaluationCode>
            {
                { RuleId.BoundingRectangleNotAllZeros, EvaluationCode.Pass },
                { RuleId.BoundingRectangleNotNull, EvaluationCode.Pass },
                { RuleId.BoundingRectangleSizeReasonable, EvaluationCode.Pass },
                { RuleId.ButtonInvokeAndExpandCollapsePatterns, EvaluationCode.Pass },
                { RuleId.ButtonInvokeAndTogglePatterns, EvaluationCode.Pass },
                { RuleId.ButtonShouldHavePatterns, EvaluationCode.Pass },
                { RuleId.ButtonToggleAndExpandCollapsePatterns, EvaluationCode.Pass },
                { RuleId.ContentViewButtonStructure, EvaluationCode.NeedsReview },
                { RuleId.ControlViewButtonStructure, EvaluationCode.Pass },
                { RuleId.HelpTextExcludesPrivateUnicodeCharacters, EvaluationCode.Pass },
                { RuleId.HelpTextNotEqualToName, EvaluationCode.Warning },
                { RuleId.IsControlElementPropertyExists, EvaluationCode.Pass },
                { RuleId.IsControlElementTrueRequiredButtonWPF, EvaluationCode.Pass },
                { RuleId.IsKeyboardFocusableShouldBeTrue, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeExcludesPrivateUnicodeCharacters, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotEmpty, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotNull, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotWhiteSpace, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeReasonable, EvaluationCode.Pass },
                { RuleId.NameExcludesControlType, EvaluationCode.Pass },
                { RuleId.NameExcludesLocalizedControlType, EvaluationCode.Pass },
                { RuleId.NameExcludesPrivateUnicodeCharacters, EvaluationCode.Pass },
                { RuleId.NameIsInformative, EvaluationCode.Pass },
                { RuleId.NameNotEmpty, EvaluationCode.Pass },
                { RuleId.NameNotNull, EvaluationCode.Pass },
                { RuleId.NameNotWhiteSpace, EvaluationCode.Pass },
                { RuleId.NameReasonableLength, EvaluationCode.Pass },
                { RuleId.ParentChildShouldNotHaveSameNameAndLocalizedControlType, EvaluationCode.RuleExecutionError },
            });
        }

        [TestMethod]
        public void MonsterListView()
        {
            A11yElement e = Utility.LoadA11yElementsFromJSON("Snapshots/MonsterListView.snapshot");
            var results = GetTestResultsAsDictionary(e);

            CheckResults(results, new Dictionary<RuleId, EvaluationCode>
            {
                { RuleId.BoundingRectangleNotAllZeros, EvaluationCode.Pass },
                { RuleId.BoundingRectangleNotNull, EvaluationCode.Pass },
                { RuleId.BoundingRectangleSizeReasonable, EvaluationCode.Pass },
                { RuleId.ContentViewListStructure, EvaluationCode.Pass },
                { RuleId.ControlViewListStructure, EvaluationCode.Pass },
                { RuleId.IsControlElementPropertyExists, EvaluationCode.Pass },
                { RuleId.IsControlElementTrueRequired, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeExcludesPrivateUnicodeCharacters, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotEmpty, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotNull, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotWhiteSpace, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeReasonable, EvaluationCode.Pass },
                { RuleId.NameNotNull, EvaluationCode.Error },
                { RuleId.ParentChildShouldNotHaveSameNameAndLocalizedControlType, EvaluationCode.RuleExecutionError },
            });
        }

        [TestMethod]
        public void MonsterDataGrid()
        {
            A11yElement e = Utility.LoadA11yElementsFromJSON("Snapshots/MonsterDataGrid.snapshot");
            var results = GetTestResultsAsDictionary(e);

            CheckResults(results, new Dictionary<RuleId, EvaluationCode>
            {
                { RuleId.BoundingRectangleNotAllZeros, EvaluationCode.Pass },
                { RuleId.BoundingRectangleNotNull, EvaluationCode.Pass },
                { RuleId.BoundingRectangleSizeReasonable, EvaluationCode.Pass },
                { RuleId.ContentViewDataGridStructure, EvaluationCode.Pass },
                { RuleId.ControlShouldSupportGridPattern, EvaluationCode.Pass },
                { RuleId.ControlViewDataGridStructure, EvaluationCode.Pass },
                { RuleId.IsControlElementPropertyExists, EvaluationCode.Pass },
                { RuleId.IsControlElementTrueRequired, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeExcludesPrivateUnicodeCharacters, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotEmpty, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotNull, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotWhiteSpace, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeReasonable, EvaluationCode.Pass },
                { RuleId.NameNotNull, EvaluationCode.Error },
                { RuleId.ParentChildShouldNotHaveSameNameAndLocalizedControlType, EvaluationCode.RuleExecutionError },
            });
        }

        [TestMethod]
        public void MonsterDataGridHeader()
        {
            A11yElement e = Utility.LoadA11yElementsFromJSON("Snapshots/MonsterDataGrid.snapshot").Children.First<A11yElement>();
            var results = GetTestResultsAsDictionary(e);

            CheckResults(results, new Dictionary<RuleId, EvaluationCode>
            {
                { RuleId.BoundingRectangleCompletelyObscuresContainer, EvaluationCode.Pass },
                { RuleId.BoundingRectangleContainedInParent, EvaluationCode.Pass },
                { RuleId.BoundingRectangleNotAllZeros, EvaluationCode.Pass },
                { RuleId.BoundingRectangleNotNull, EvaluationCode.Pass },
                { RuleId.BoundingRectangleSizeReasonable, EvaluationCode.Pass },
                { RuleId.ControlViewHeaderStructure, EvaluationCode.Pass },
                { RuleId.IsControlElementPropertyExists, EvaluationCode.Pass },
                { RuleId.IsControlElementTrueRequired, EvaluationCode.Pass },
                { RuleId.IsKeyboardFocusableShouldBeFalse, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeExcludesPrivateUnicodeCharacters, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotWhiteSpace, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeReasonable, EvaluationCode.Pass },
                { RuleId.NameNoSiblingsOfSameType, EvaluationCode.NeedsReview },
            });
        }

        [TestMethod]
        public void MonsterEdit()
        {
            A11yElement e = Utility.LoadA11yElementsFromJSON("Snapshots/MonsterEdit.snapshot");
            var results = GetTestResultsAsDictionary(e);

            CheckResults(results, new Dictionary<RuleId, EvaluationCode>
            {
                { RuleId.BoundingRectangleNotAllZeros, EvaluationCode.Pass },
                { RuleId.BoundingRectangleNotNull, EvaluationCode.Pass },
                { RuleId.BoundingRectangleSizeReasonable, EvaluationCode.Pass },
                { RuleId.ContentViewEditStructure, EvaluationCode.Pass },
                { RuleId.ControlShouldSupportTextPattern, EvaluationCode.Pass },
                { RuleId.ControlViewEditStructure, EvaluationCode.NeedsReview },
                { RuleId.IsControlElementPropertyExists, EvaluationCode.Pass },
                { RuleId.IsControlElementTrueRequired, EvaluationCode.Pass },
                { RuleId.IsKeyboardFocusableShouldBeTrue, EvaluationCode.Pass },
                { RuleId.IsKeyboardFocusableTopLevelTextPattern, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeExcludesPrivateUnicodeCharacters, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotEmpty, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotNull, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotWhiteSpace, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeReasonable, EvaluationCode.Pass },
                { RuleId.NameNotNull, EvaluationCode.Error },
                { RuleId.ParentChildShouldNotHaveSameNameAndLocalizedControlType, EvaluationCode.RuleExecutionError },
            });
        }

        [TestMethod]
        public void MonsterMenu()
        {
            A11yElement e = Utility.LoadA11yElementsFromJSON("Snapshots/MonsterMenu.snapshot");
            var results = GetTestResultsAsDictionary(e);

            CheckResults(results, new Dictionary<RuleId, EvaluationCode>
            {
                { RuleId.BoundingRectangleNotAllZeros, EvaluationCode.Pass },
                { RuleId.BoundingRectangleNotNull, EvaluationCode.Pass },
                { RuleId.BoundingRectangleSizeReasonable, EvaluationCode.Pass },
                { RuleId.ContentViewMenuStructure, EvaluationCode.Pass },
                { RuleId.ControlViewMenuStructure, EvaluationCode.Pass },
                { RuleId.IsControlElementPropertyExists, EvaluationCode.Pass },
                { RuleId.IsControlElementTrueRequired, EvaluationCode.Pass },
                { RuleId.IsKeyboardFocusableShouldBeTrue, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeExcludesPrivateUnicodeCharacters, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotEmpty, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotNull, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotWhiteSpace, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeReasonable, EvaluationCode.Pass },
                { RuleId.NameNotNull, EvaluationCode.Error },
                { RuleId.ParentChildShouldNotHaveSameNameAndLocalizedControlType, EvaluationCode.RuleExecutionError },
            });
        }

        [TestMethod]
        public void MonsterUserControl()
        {
            A11yElement e = Utility.LoadA11yElementsFromJSON("Snapshots/MonsterUserControl.snapshot");
            var results = GetTestResultsAsDictionary(e);

            CheckResults(results, new Dictionary<RuleId, EvaluationCode>
            {
                { RuleId.BoundingRectangleNotAllZeros, EvaluationCode.Pass },
                { RuleId.BoundingRectangleNotNull, EvaluationCode.Pass },
                { RuleId.BoundingRectangleSizeReasonable, EvaluationCode.Pass },
                { RuleId.ControlShouldNotSupportValuePattern, EvaluationCode.Pass },
                { RuleId.IsContentElementPropertyExists, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeExcludesPrivateUnicodeCharacters, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeNotWhiteSpace, EvaluationCode.Pass },
                { RuleId.LocalizedControlTypeReasonable, EvaluationCode.Pass },
                { RuleId.NameEmptyButElementNotKeyboardFocusable, EvaluationCode.Pass },
                { RuleId.NameExcludesPrivateUnicodeCharacters, EvaluationCode.Pass },
                { RuleId.NameNotWhiteSpace, EvaluationCode.Pass },
                { RuleId.NameNullButElementNotKeyboardFocusable, EvaluationCode.Pass },
            });
        }

        private static Dictionary<RuleId, EvaluationCode> GetTestResultsAsDictionary(A11yElement e)
        {
            var results = Axe.Windows.Rules.Rules.RunAll(e, CancellationToken.None);
            return results.ToDictionary(r => r.RuleInfo.ID, r => r.EvaluationCode);
        }

        private static void CheckResults(Dictionary<RuleId, EvaluationCode> actualResults,
            Dictionary<RuleId, EvaluationCode> expectedResults)
        {
            DoNotPassNotApplicableAsExpectedResults(expectedResults);

            foreach (KeyValuePair<RuleId, EvaluationCode> actualPair in actualResults)
            {
                if (actualPair.Value == EvaluationCode.NotApplicable)
                    continue;

                if (expectedResults.TryGetValue(actualPair.Key, out EvaluationCode expectedValue))
                {
                    Assert.AreEqual(expectedValue, actualPair.Value, $"Rule: {actualPair.Key}");
                    expectedResults.Remove(actualPair.Key);
                }
                else
                {
                    Assert.Fail($"You must pass an expected value for this rule: {actualPair.Key}");
                }
            }

            CheckForExtraRules(expectedResults);
        }

        private static void DoNotPassNotApplicableAsExpectedResults(Dictionary<RuleId, EvaluationCode> expectedResults)
        {
            foreach (KeyValuePair<RuleId, EvaluationCode> expectedResult in expectedResults)
            {
                if (expectedResult.Value == EvaluationCode.NotApplicable)
                {
                    Assert.Fail($"Do not pass EvaluationCode.NotApplicable as an expected result. Key: {expectedResult.Key}");
                }
            }
        }

        private static void CheckForExtraRules(Dictionary<RuleId, EvaluationCode> results)
        {
            List<string> extraRules = new List<string>();
            foreach (KeyValuePair<RuleId, EvaluationCode> pair in results)
            {
                extraRules.Add(pair.Key.ToString());
            }

            if (!extraRules.Any()) return;

            extraRules.Sort();
            Assert.Fail($"Remove {extraRules.Count} extra rule(s) from expected: {string.Join(", ", extraRules)}");
        }
    } // class
} // namespace
