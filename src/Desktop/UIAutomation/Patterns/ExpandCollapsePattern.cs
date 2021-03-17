// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.Types;
using Axe.Windows.Telemetry;
using System.Runtime.InteropServices;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for ExpandCollapse Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671276(v=vs.85).aspx
    /// </summary>
    [PatternEvent(Id = EventType.UIA_AutomationPropertyChangedEventId)]
    public class ExpandCollapsePattern : A11yPattern
    {
        IUIAutomationExpandCollapsePattern Pattern;

        public ExpandCollapsePattern(A11yElement e, IUIAutomationExpandCollapsePattern p) : base(e, PatternType.UIA_ExpandCollapsePatternId)
        {
            // UIA sometimes throws a COMException. If it does, exclude it
            // from telemetry
            ExcludingExceptionWrapper.ExecuteWithExcludedExceptionConversion(typeof(COMException), () =>
            {
                Pattern = p;

                PopulateProperties();
            });
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            this.Properties.Add(new A11yPatternProperty() { Name = "ExpandCollapseState", Value = this.Pattern.CurrentExpandCollapseState });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod(IsUIAction = true)]
        public void Expand()
        {
            this.Pattern.Expand();
        }

        [PatternMethod(IsUIAction = true)]
        public void Collapse()
        {
            this.Pattern.Collapse();
        }

        protected override void Dispose(bool disposing)
        {
            if (Pattern != null)
            {
                Marshal.ReleaseComObject(Pattern);
                this.Pattern = null;
            }

            base.Dispose(disposing);
        }
    }
}
