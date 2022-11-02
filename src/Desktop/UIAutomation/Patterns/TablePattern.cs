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
    /// Control pattern wrapper for Table Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671288(v=vs.85).aspx
    /// </summary>
    public class TablePattern : A11yPattern
    {
        IUIAutomationTablePattern Pattern;

        public TablePattern(A11yElement e, IUIAutomationTablePattern p) : base(e, PatternType.UIA_TablePatternId)
        {
            Pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "RowOrColumnMajor", Value = Pattern.CurrentRowOrColumnMajor });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod]
        public IList<DesktopElement> GetColumnHeaders()
        {
            return Pattern.GetCurrentColumnHeaders()?.ToListOfDesktopElements();
        }

        [PatternMethod]
        public IList<DesktopElement> GetRowHeaders()
        {
            return Pattern.GetCurrentRowHeaders()?.ToListOfDesktopElements();
        }

        protected override void Dispose(bool disposing)
        {
            if (Pattern != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(Pattern);
                Pattern = null;
            }

            base.Dispose(disposing);
        }
    }
}
