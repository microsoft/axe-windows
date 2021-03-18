// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Types;
using System.Collections.Generic;
using Axe.Windows.Core.Bases;
using UIAutomationClient;
using Axe.Windows.Desktop.Utility;
using Axe.Windows.Core.Attributes;

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
            this.Properties.Add(new A11yPatternProperty() { Name = "RowOrColumnMajor", Value = this.Pattern.CurrentRowOrColumnMajor });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod]
        public IList<DesktopElement> GetColumnHeaders()
        {
            return this.Pattern.GetCurrentColumnHeaders()?.ToListOfDesktopElements();
        }

        [PatternMethod]
        public IList<DesktopElement> GetRowHeaders()
        {
            return this.Pattern.GetCurrentRowHeaders()?.ToListOfDesktopElements();
        }

        protected override void Dispose(bool disposing)
        {
            if (Pattern != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(Pattern);
                this.Pattern = null;
            }

            base.Dispose(disposing);
        }
    }
}
