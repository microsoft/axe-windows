// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.Utility;
using System.Collections.Generic;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for TableItem Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671289(v=vs.85).aspx
    /// </summary>
    public class TableItemPattern : A11yPattern
    {
        IUIAutomationTableItemPattern _pattern;

        public TableItemPattern(A11yElement e, IUIAutomationTableItemPattern p) : base(e, PatternType.UIA_TableItemPatternId)
        {
            _pattern = p;
        }

        [PatternMethod]
        public IList<DesktopElement> GetColumnHeaderItems()
        {
            return _pattern.GetCurrentColumnHeaderItems()?.ToListOfDesktopElements();
        }

        [PatternMethod]
        public IList<DesktopElement> GetRowHeaderItems()
        {
            return _pattern.GetCurrentRowHeaderItems()?.ToListOfDesktopElements();
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
