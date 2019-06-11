using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Provides methods used to assemble a ScanResults object
    /// </summary>
    public static class ScanResultsAssembler
    {
        /// <summary>
        /// Assembles failed scans from the provided element
        /// </summary>
        /// <param name="element">Root element from which scan results will be assembled</param>
        /// <returns>A ScanResults object containing the relevant errors and error count</returns>
        public static ScanResults AssembleScanResultsFromElement(A11yElement element)
        {
            var errors = new List<ScanResult>();
            int count = AssembleScanResults(errors, element, null);

            return new ScanResults()
            {
                Errors = errors,
                ErrorCount = count,
            };
        }

        /// <summary>
        /// Assembles errors recursively starting at the given element
        /// </summary>
        /// <param name="errors">Where the results will be accumulated</param>
        /// <param name="element">The root element to check</param>
        /// <param name="parent">The root element to check</param>
        /// <returns>Count of errors found</returns>
        internal static int AssembleScanResults(List<ScanResult> errors, A11yElement element, ElementInfo parent)
        {
            if (element.ScanResults == null) throw new ArgumentException("element.ScanResults must not be null");

            int failedCount = 0;
            ElementInfo elementInfo = MakeElementInfoFromElement(element, parent);
            IEnumerable<RuleResult> results = GetFailedRuleResultsFromElement(element);

            foreach (var res in results)
            {
                ScanResult result = new ScanResult()
                {
                    Element = elementInfo,
                    Rule = Rules.Rules.All[res.Rule],
                };

                errors.Add(result);
                failedCount++;
            }

            if (element.Children == null) return failedCount;

            foreach (var child in element.Children)
            {
                failedCount+= AssembleScanResults(errors, child, elementInfo);
            }

            return failedCount;
        }

        internal static ElementInfo MakeElementInfoFromElement(A11yElement element, ElementInfo parent)
        {
            return new ElementInfo
            {
                Parent = parent,
                Patterns = element.Patterns.ConvertAll<string>(x => x.Name),
                Properties = element.Properties.ToDictionary(p => p.Value.Name, p => p.Value.TextValue)
            };
        }

        internal static IEnumerable<RuleResult> GetFailedRuleResultsFromElement(A11yElement element)
        {
           return from scanResult in element.ScanResults.Items
                  where scanResult.Status == ScanStatus.Fail
                  from ruleResult in scanResult.Items
                  where ruleResult.Status == ScanStatus.Fail
                  select ruleResult;
        }
    }
}
