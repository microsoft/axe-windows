// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for VirtualizedItem Control Pattern
    /// </summary>
    public class VirtualizedItemPattern : A11yPattern
    {
        IUIAutomationVirtualizedItemPattern _pattern;

        public VirtualizedItemPattern(A11yElement e, IUIAutomationVirtualizedItemPattern p) : base(e, PatternType.UIA_VirtualizedItemPatternId)
        {
            _pattern = p;
        }

        [PatternMethod]
        public void Realize()
        {
            _pattern.Realize();
        }

        protected override void Dispose(bool disposing)
        {
            if (_pattern != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_pattern);
                _pattern = null;
            }

            base.Dispose(disposing);
        }
    }
}
