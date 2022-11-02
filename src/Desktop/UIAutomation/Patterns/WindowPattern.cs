// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.Types;
using System;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for Window Control Pattern
    /// </summary>
    [PatternEvent(Id = EventType.UIA_Window_WindowClosedEventId)]
    [PatternEvent(Id = EventType.UIA_Window_WindowOpenedEventId)]
    public class WindowPattern : A11yPattern
    {
        IUIAutomationWindowPattern Pattern;

        public WindowPattern(A11yElement e, IUIAutomationWindowPattern p) : base(e, PatternType.UIA_WindowPatternId)
        {
            Pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "CanMaximize", Value = Convert.ToBoolean(Pattern.CurrentCanMaximize) });
            Properties.Add(new A11yPatternProperty() { Name = "CanMinimize", Value = Convert.ToBoolean(Pattern.CurrentCanMinimize) });
            Properties.Add(new A11yPatternProperty() { Name = "IsModal", Value = Convert.ToBoolean(Pattern.CurrentIsModal) });
            Properties.Add(new A11yPatternProperty() { Name = "IsTopmost", Value = Convert.ToBoolean(Pattern.CurrentIsTopmost) });
            Properties.Add(new A11yPatternProperty() { Name = "WindowInteractionState", Value = Pattern.CurrentWindowInteractionState });
            Properties.Add(new A11yPatternProperty() { Name = "WindowVisualState", Value = Pattern.CurrentWindowVisualState });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod]
        public void SetWindowVisualState(WindowVisualState state)
        {
            Pattern.SetWindowVisualState(state);
        }

        /// <summary>
        /// Wait for no input (i.e. idle)
        /// </summary>
        /// <param name="ms">milliseconds</param>
        [PatternMethod]
        public void WaitForInputIdle(int ms)
        {
            Pattern.WaitForInputIdle(ms);
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
