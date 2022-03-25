// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Axe.Windows.Telemetry;
using System;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for Value Control Pattern
    /// </summary>
    public class ValuePattern : A11yPattern
    {
        IUIAutomationValuePattern Pattern;

        public ValuePattern(A11yElement e, IUIAutomationValuePattern p) : base(e, PatternType.UIA_ValuePatternId)
        {
            Pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            this.Properties.Add(new A11yPatternProperty() { Name = "IsReadOnly", Value = Convert.ToBoolean(this.Pattern.CurrentIsReadOnly) });
            try
            {
                this.Properties.Add(new A11yPatternProperty() { Name = "Value", Value = this.Pattern.CurrentValue });
            }
            catch (InvalidOperationException e)
            {
                e.ReportException();
                // there is a known case that CurrentValue is not ready.
                // to avoid catastrophic failure downstream, handle it here.
            }
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod]
        public void SetValue(string val)
        {
            this.Pattern.SetValue(val);
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
