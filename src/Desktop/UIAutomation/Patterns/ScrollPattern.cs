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
    /// Control pattern wrapper for Scroll Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee872087(v=vs.85).aspx
    /// </summary>
    public class ScrollPattern : A11yPattern
    {
        IUIAutomationScrollPattern _pattern;

        public ScrollPattern(A11yElement e, IUIAutomationScrollPattern p) : base(e, PatternType.UIA_ScrollPatternId)
        {
            _pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "HorizontallyScrollable", Value = Convert.ToBoolean(_pattern.CurrentHorizontallyScrollable) });
            Properties.Add(new A11yPatternProperty() { Name = "HorizontalScrollPercent", Value = _pattern.CurrentHorizontalScrollPercent });
            Properties.Add(new A11yPatternProperty() { Name = "HorizontalViewSize", Value = _pattern.CurrentHorizontalViewSize });
            Properties.Add(new A11yPatternProperty() { Name = "VerticallyScrollable", Value = Convert.ToBoolean(_pattern.CurrentVerticallyScrollable) });
            Properties.Add(new A11yPatternProperty() { Name = "VerticalScrollPercent", Value = _pattern.CurrentVerticalScrollPercent });
            Properties.Add(new A11yPatternProperty() { Name = "VerticalViewSize", Value = _pattern.CurrentVerticalViewSize });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        //Scroll() means that it has scroll functionality. but doesn't mean that item should be focused since scroll actually happens by scrollitem
        [PatternMethod]
        public void Scroll(ScrollAmount ha, ScrollAmount va)
        {
            _pattern.Scroll(ha, va);
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
