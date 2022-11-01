// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Exceptions;
using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.Resources;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for GridItem Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671278(v=vs.85).aspx
    /// </summary>
    public class GridItemPattern : A11yPattern
    {
        IUIAutomationGridItemPattern Pattern;

        public GridItemPattern(A11yElement e, IUIAutomationGridItemPattern p) : base(e, PatternType.UIA_GridItemPatternId)
        {
            Pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "Column", Value = Pattern.CurrentColumn });
            Properties.Add(new A11yPatternProperty() { Name = "ColumnSpan", Value = Pattern.CurrentColumnSpan });
            Properties.Add(new A11yPatternProperty() { Name = "Row", Value = Pattern.CurrentRow });
            Properties.Add(new A11yPatternProperty() { Name = "RowSpan", Value = Pattern.CurrentRowSpan });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

#pragma warning disable CA1024 // Use properties where appropriate
        [PatternMethod]
        public DesktopElement GetContainingGrid()
        {
            var e = new DesktopElement(Pattern.CurrentContainingGrid);

            if (e.Properties != null && e.Properties.Count != 0)
            {
                return e;
            }

            throw new AxeWindowsException(ErrorMessages.PatternNoLongerValid);
        }
#pragma warning restore CA1024 // Use properties where appropriate

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
