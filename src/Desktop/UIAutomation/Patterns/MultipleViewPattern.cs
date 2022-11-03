// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using UIAutomationClient;

using static System.FormattableString;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for MultipleView Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671282(v=vs.85).aspx
    /// </summary>
    public class MultipleViewPattern : A11yPattern
    {
        IUIAutomationMultipleViewPattern _pattern;

        public MultipleViewPattern(A11yElement e, IUIAutomationMultipleViewPattern p) : base(e, PatternType.UIA_MultipleViewPatternId)
        {
            _pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "CurrentView", Value = _pattern.CurrentCurrentView });
            var array = _pattern.GetCurrentSupportedViews();
            if (array.Length > 0)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    var view = (int)array.GetValue(i);
                    Properties.Add(new A11yPatternProperty() { Name = Invariant($"SupportedViews[{i}]"), Value = Invariant($"{view}: {_pattern.GetViewName(view)}") });
                }
            }
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod]
        public void SetCurrentView(int view)
        {
            _pattern.SetCurrentView(view);
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
