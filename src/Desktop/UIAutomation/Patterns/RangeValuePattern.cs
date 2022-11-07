// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using System;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for RangeValue Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671283(v=vs.85).aspx
    /// </summary>
    public class RangeValuePattern : A11yPattern
    {
        IUIAutomationRangeValuePattern _pattern;

        public RangeValuePattern(A11yElement e, IUIAutomationRangeValuePattern p) : base(e, PatternType.UIA_RangeValuePatternId)
        {
            _pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "IsReadOnly", Value = Convert.ToBoolean(_pattern.CurrentIsReadOnly) });
            Properties.Add(new A11yPatternProperty() { Name = "LargeChange", Value = _pattern.CurrentLargeChange });
            Properties.Add(new A11yPatternProperty() { Name = "Maximum", Value = _pattern.CurrentMaximum });
            Properties.Add(new A11yPatternProperty() { Name = "Minimum", Value = _pattern.CurrentMinimum });
            Properties.Add(new A11yPatternProperty() { Name = "SmallChange", Value = _pattern.CurrentSmallChange });
            Properties.Add(new A11yPatternProperty() { Name = "Value", Value = _pattern.CurrentValue });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod(IsUIAction = true)]
        public void SetValue(double val)
        {
            _pattern.SetValue(val);
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

