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
        IUIAutomationRangeValuePattern Pattern;

        public RangeValuePattern(A11yElement e, IUIAutomationRangeValuePattern p) : base(e, PatternType.UIA_RangeValuePatternId)
        {
            Pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "IsReadOnly", Value = Convert.ToBoolean(Pattern.CurrentIsReadOnly) });
            Properties.Add(new A11yPatternProperty() { Name = "LargeChange", Value = Pattern.CurrentLargeChange });
            Properties.Add(new A11yPatternProperty() { Name = "Maximum", Value = Pattern.CurrentMaximum });
            Properties.Add(new A11yPatternProperty() { Name = "Minimum", Value = Pattern.CurrentMinimum });
            Properties.Add(new A11yPatternProperty() { Name = "SmallChange", Value = Pattern.CurrentSmallChange });
            Properties.Add(new A11yPatternProperty() { Name = "Value", Value = Pattern.CurrentValue });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod(IsUIAction = true)]
        public void SetValue(double val)
        {
            Pattern.SetValue(val);
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

