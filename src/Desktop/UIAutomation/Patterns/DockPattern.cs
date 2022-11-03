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
    /// DockPattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671421(v=vs.85).aspx
    /// </summary>
    public class DockPattern : A11yPattern
    {
        IUIAutomationDockPattern _pattern;

        public DockPattern(A11yElement e, IUIAutomationDockPattern p) : base(e, PatternType.UIA_DockPatternId)
        {
            _pattern = p;
            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "DockPosition", Value = _pattern.CurrentDockPosition });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod(IsUIAction = true)]
        public void SetDockPosition(DockPosition dockPos)
        {
            _pattern.SetDockPosition(dockPos);
        }

        protected override void Dispose(bool disposing)
        {
            if (_pattern != null)
            {
                Marshal.ReleaseComObject(_pattern);
                _pattern = null;
            }

            base.Dispose(disposing);
        }
    }
}
