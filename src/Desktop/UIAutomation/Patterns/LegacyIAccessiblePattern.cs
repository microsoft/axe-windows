// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.Utility;
using Axe.Windows.Telemetry;
using System;
using System.Collections.Generic;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for LegacyIAccessible Control Pattern
    /// </summary>
    public class LegacyIAccessiblePattern : A11yPattern
    {
        IUIAutomationLegacyIAccessiblePattern Pattern;

        public LegacyIAccessiblePattern(A11yElement e, IUIAutomationLegacyIAccessiblePattern p) : base(e, PatternType.UIA_LegacyIAccessiblePatternId)
        {
            Pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
            try
            {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
                Properties.Add(new A11yPatternProperty() { Name = "ChildId", Value = Pattern.CurrentChildId });
                Properties.Add(new A11yPatternProperty() { Name = "DefaultAction", Value = Pattern.CurrentDefaultAction });
                Properties.Add(new A11yPatternProperty() { Name = "Description", Value = Pattern.CurrentDescription });
                Properties.Add(new A11yPatternProperty() { Name = "Help", Value = Pattern.CurrentHelp });
                Properties.Add(new A11yPatternProperty() { Name = "KeyboardShorcut", Value = Pattern.CurrentKeyboardShortcut });
                Properties.Add(new A11yPatternProperty() { Name = "Name", Value = Pattern.CurrentName });
                Properties.Add(new A11yPatternProperty() { Name = "Role", Value = Pattern.CurrentRole });
                Properties.Add(new A11yPatternProperty() { Name = "State", Value = Pattern.CurrentState });
                Properties.Add(new A11yPatternProperty() { Name = "Value", Value = Pattern.CurrentValue });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                e.ReportException();
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        [PatternMethod]
        public IList<DesktopElement> GetSelection()
        {
            return Pattern.GetCurrentSelection().ToListOfDesktopElements();
        }

        [PatternMethod]
        public void DoDefaultAction()
        {
            Pattern.DoDefaultAction();
        }

        [PatternMethod]
        public IAccessible GetIAccessible()
        {
            return Pattern.GetIAccessible();
        }

        [PatternMethod]
        public void Select(int flagSelect)
        {
            Pattern.Select(flagSelect);
        }

        [PatternMethod]
        public void SetValue(string value)
        {
            Pattern.SetValue(value);
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
