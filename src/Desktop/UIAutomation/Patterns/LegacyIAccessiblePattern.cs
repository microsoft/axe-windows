// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Types;
using Axe.Windows.Telemetry;
using System;
using System.Collections.Generic;
using Axe.Windows.Core.Bases;
using UIAutomationClient;
using Axe.Windows.Core.Attributes;
using Axe.Windows.Desktop.Utility;

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
                this.Properties.Add(new A11yPatternProperty() { Name = "ChildId", Value = this.Pattern.CurrentChildId });
                this.Properties.Add(new A11yPatternProperty() { Name = "DefaultAction", Value = this.Pattern.CurrentDefaultAction });
                this.Properties.Add(new A11yPatternProperty() { Name = "Description", Value = this.Pattern.CurrentDescription });
                this.Properties.Add(new A11yPatternProperty() { Name = "Help", Value = this.Pattern.CurrentHelp });
                this.Properties.Add(new A11yPatternProperty() { Name = "KeyboardShorcut", Value = this.Pattern.CurrentKeyboardShortcut });
                this.Properties.Add(new A11yPatternProperty() { Name = "Name", Value = this.Pattern.CurrentName });
                this.Properties.Add(new A11yPatternProperty() { Name = "Role", Value = this.Pattern.CurrentRole });
                this.Properties.Add(new A11yPatternProperty() { Name = "State", Value = this.Pattern.CurrentState });
                this.Properties.Add(new A11yPatternProperty() { Name = "Value", Value = this.Pattern.CurrentValue });
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                e.ReportException();
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

#pragma warning disable CA1002 // Do not expose generic lists
        [PatternMethod]
        public List<DesktopElement> GetSelection()
        {
            return this.Pattern.GetCurrentSelection().ToListOfDesktopElements();
        }
#pragma warning restore CA1002 // Do not expose generic lists

        [PatternMethod]
        public void DoDefaultAction()
        {
            this.Pattern.DoDefaultAction();
        }

        [PatternMethod]
        public IAccessible GetIAccessible()
        {
            return this.Pattern.GetIAccessible();
        }

        [PatternMethod]
        public void Select(int flagSelect)
        {
            this.Pattern.Select(flagSelect);
        }

        [PatternMethod]
        public void SetValue(string value)
        {
            this.Pattern.SetValue(value);
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
