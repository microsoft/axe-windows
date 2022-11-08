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
        IUIAutomationLegacyIAccessiblePattern _pattern;

        public LegacyIAccessiblePattern(A11yElement e, IUIAutomationLegacyIAccessiblePattern p) : base(e, PatternType.UIA_LegacyIAccessiblePatternId)
        {
            _pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
            try
            {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
                Properties.Add(new A11yPatternProperty() { Name = "ChildId", Value = _pattern.CurrentChildId });
                Properties.Add(new A11yPatternProperty() { Name = "DefaultAction", Value = _pattern.CurrentDefaultAction });
                Properties.Add(new A11yPatternProperty() { Name = "Description", Value = _pattern.CurrentDescription });
                Properties.Add(new A11yPatternProperty() { Name = "Help", Value = _pattern.CurrentHelp });
                Properties.Add(new A11yPatternProperty() { Name = "KeyboardShorcut", Value = _pattern.CurrentKeyboardShortcut });
                Properties.Add(new A11yPatternProperty() { Name = "Name", Value = _pattern.CurrentName });
                Properties.Add(new A11yPatternProperty() { Name = "Role", Value = _pattern.CurrentRole });
                Properties.Add(new A11yPatternProperty() { Name = "State", Value = _pattern.CurrentState });
                Properties.Add(new A11yPatternProperty() { Name = "Value", Value = _pattern.CurrentValue });
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
            return _pattern.GetCurrentSelection().ToListOfDesktopElements();
        }

        [PatternMethod]
        public void DoDefaultAction()
        {
            _pattern.DoDefaultAction();
        }

        [PatternMethod]
        public IAccessible GetIAccessible()
        {
            return _pattern.GetIAccessible();
        }

        [PatternMethod]
        public void Select(int flagSelect)
        {
            _pattern.Select(flagSelect);
        }

        [PatternMethod]
        public void SetValue(string value)
        {
            _pattern.SetValue(value);
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
