// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for Scroll Item Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671284(v=vs.85).aspx
    /// </summary>
    public class ScrollItemPattern : A11yPattern
    {
        IUIAutomationScrollItemPattern _pattern;

        public ScrollItemPattern(A11yElement e, IUIAutomationScrollItemPattern p) : base(e, PatternType.UIA_ScrollItemPatternId)
        {
            _pattern = p;
        }

        /// <summary>
        /// this method is not UI Action by user. but it is for UI automation.
        /// </summary>
        [PatternMethod]
        public void ScrollIntoView()
        {
            _pattern.ScrollIntoView();
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
