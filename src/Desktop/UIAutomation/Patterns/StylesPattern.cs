// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for Styles Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/hh448775(v=vs.85).aspx
    /// </summary>
    public class StylesPattern : A11yPattern
    {
        IUIAutomationStylesPattern _pattern;

        public StylesPattern(A11yElement e, IUIAutomationStylesPattern p) : base(e, PatternType.UIA_StylesPatternId)
        {
            _pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "ExtendedProperties", Value = _pattern.CurrentExtendedProperties });
            Properties.Add(new A11yPatternProperty() { Name = "FillColor ", Value = _pattern.CurrentFillColor });
            Properties.Add(new A11yPatternProperty() { Name = "FillPatternColor ", Value = _pattern.CurrentFillPatternColor });
            Properties.Add(new A11yPatternProperty() { Name = "FillPatternStyle ", Value = _pattern.CurrentFillPatternStyle });
            Properties.Add(new A11yPatternProperty() { Name = "Shape ", Value = _pattern.CurrentShape });
            Properties.Add(new A11yPatternProperty() { Name = "StyleId ", Value = _pattern.CurrentStyleId });
            Properties.Add(new A11yPatternProperty() { Name = "StyleName ", Value = _pattern.CurrentStyleName });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
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
