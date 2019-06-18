// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Results;
using Axe.Windows.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Provides methods used to assemble a <see cref="ScanResults"/> object
    /// </summary>
    internal class ScanResultsAssembler : IScanResultsAssembler
    {
        /// <summary>
        /// Assembles failed scans from the provided element
        /// </summary>
        /// <param name="element">Root element from which scan results will be assembled</param>
        /// <returns>A ScanResults object containing the relevant errors and error count</returns>
        public ScanResults AssembleScanResultsFromElement(A11yElement element)
        {
            var errors = new List<ScanResult>();

            AssembleErrorsFromElement(errors, element, null);

            return new ScanResults()
            {
                Errors = errors,
                ErrorCount = errors.Count,
            };
        }

        /// <summary>
        /// Assembles errors recursively into a <see cref="ScanResult"/> list starting at the given element
        /// </summary>
        /// <param name="errors">Where the <see cref="ScanResult"/> objects created from errors will be added</param>
        /// <param name="element">Root element from which errors will be assembled</param>
        /// <param name="parent">The parent of element</param>
        internal static void AssembleErrorsFromElement(List<ScanResult> errors, A11yElement element, ElementInfo parent)
        {
            if (errors == null) throw new ArgumentNullException(nameof(errors));

            if (element == null) throw new ArgumentNullException(nameof(element));

            if (element.ScanResults == null) return;

            ElementInfo elementInfo = MakeElementInfoFromElement(element, parent);

            foreach (var res in GetFailedRuleResultsFromElement(element))
            {
                errors.Add(MakeScanResult(elementInfo, res));
            }

            if (element.Children == null) return;

            foreach (var child in element.Children)
            {
                AssembleErrorsFromElement(errors, child, elementInfo);
            }
        }

        private static ScanResult MakeScanResult(ElementInfo elementInfo, RuleResult res)
        {
            if (!Rules.Rules.All.TryGetValue(res.Rule, out RuleInfo rule))
                throw new KeyNotFoundException($"{res.Rule} not found in {nameof(Rules)} dictionary.");

            return new ScanResult()
            {
                Element = elementInfo,
                Rule = rule,
            };
        }

        private static ElementInfo MakeElementInfoFromElement(A11yElement element, ElementInfo parent)
        {
            return new ElementInfo
            {
                Parent = parent,
                Patterns = element.Patterns?.ConvertAll<string>(x => x.Name),
                Properties = element.Properties?.ToDictionary(p => p.Value.Name, p => p.Value.TextValue)
            };
        }

        private static IEnumerable<RuleResult> GetFailedRuleResultsFromElement(A11yElement element)
        {
           return from scanResult in element.ScanResults.Items
                  where scanResult.Status == ScanStatus.Fail
                  from ruleResult in scanResult.Items
                  where ruleResult.Status == ScanStatus.Fail
                  select ruleResult;
        }
    }
}
