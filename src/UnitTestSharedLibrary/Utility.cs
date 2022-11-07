// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Results;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Axe.Windows.UnitTestSharedLibrary
{
    /// <summary>
    /// Class to contain shared utility methods for Unit tests
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Load the UI Automation elements hierarchy tree from JSON file.
        /// it returns the root UI Automation element from the tree.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static A11yElement LoadA11yElementsFromJSON(string path)
        {
            A11yElement element = null;
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                element = JsonConvert.DeserializeObject<A11yElement>(json);
                if (element == null)
                {
                    return null;
                }
                // Set parents
                Queue<A11yElement> elements = new Queue<A11yElement>();
                elements.Enqueue(element);
                while (elements.Count > 0)
                {
                    var next = elements.Dequeue();
                    if (next.Children != null)
                    {
                        foreach (var c in next.Children)
                        {
                            c.Parent = next;
                            elements.Enqueue(c);
                        };
                    }
                }
            }

            return element;
        }

        /// <summary>
        /// Populates all descendants with test results and sets them to
        ///     pass if the control is a button (any predicate would work)
        ///     and returns number that should pass
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static void PopulateChildrenTests(A11yElement element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            foreach (var item in element.ScanResults.Items)
            {
                item.Items = new List<RuleResult>
                {
                    new RuleResult
                    {
                        Status = element.ControlTypeId == Axe.Windows.Core.Types.ControlType.UIA_ButtonControlTypeId ? ScanStatus.Pass : ScanStatus.Fail
                    }
                };
            };
            foreach (var c in element.Children)
            {
                PopulateChildrenTests(c);
            };
        }
    }
}
