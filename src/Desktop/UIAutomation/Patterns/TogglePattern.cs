// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for Toggle Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671290(v=vs.85).aspx
    /// </summary>
    public class TogglePattern : A11yPattern
    {
        IUIAutomationTogglePattern Pattern;

        public TogglePattern(A11yElement e, IUIAutomationTogglePattern p) : base(e, PatternType.UIA_TogglePatternId)
        {
            Pattern = p;
            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "ToggleState", Value = Pattern.CurrentToggleState });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod(IsUIAction = true)]
        public void Toggle()
        {
            Pattern.Toggle();
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
