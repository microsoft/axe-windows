// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using System.Runtime.InteropServices;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{

    /// <summary>
    /// CustomNavigation Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/dn858171(v=vs.85).aspx
    /// </summary>
    class CustomNavigationPattern : A11yPattern
    {
        IUIAutomationCustomNavigationPattern Pattern;

        public CustomNavigationPattern(A11yElement e, IUIAutomationCustomNavigationPattern p) : base(e, PatternType.UIA_CustomNavigationPatternId)
        {
            this.Pattern = p;
        }

        /// <summary>
        /// Navigate action
        /// </summary>
        /// <param name="direction"></param>
        [PatternMethod(IsUIAction = true)]
        public void Navigate(NavigateDirection direction)
        {
            this.Pattern.Navigate(direction);
        }

        protected override void Dispose(bool disposing)
        {
            if (Pattern != null)
            {
                Marshal.ReleaseComObject(Pattern);
                this.Pattern = null;
            }

            base.Dispose(disposing);
        }
    }
}
