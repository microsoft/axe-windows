// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for Grid Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671277(v=vs.85).aspx
    /// </summary>
    public class GridPattern : A11yPattern
    {
        IUIAutomationGridPattern _pattern;

        public GridPattern(A11yElement e, IUIAutomationGridPattern p) : base(e, PatternType.UIA_GridPatternId)
        {
            _pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "ColumnCount", Value = _pattern.CurrentColumnCount });
            Properties.Add(new A11yPatternProperty() { Name = "RowCount", Value = _pattern.CurrentRowCount });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod]
        public DesktopElement GetItem(int row, int column)
        {
            var uiae = _pattern.GetItem(row, column);

            return uiae != null ? new DesktopElement(uiae) : null;
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
