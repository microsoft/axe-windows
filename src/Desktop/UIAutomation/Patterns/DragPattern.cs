// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.Types;
using Axe.Windows.Desktop.Utility;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UIAutomationClient;

using static System.FormattableString;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for Drag Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/hh707348(v=vs.85).aspx
    /// </summary>
    [PatternEvent(Id = EventType.UIA_Drag_DragStartEventId)]
    [PatternEvent(Id = EventType.UIA_Drag_DragCancelEventId)]
    [PatternEvent(Id = EventType.UIA_Drag_DragCompleteEventId)]
    public class DragPattern : A11yPattern
    {
        IUIAutomationDragPattern _pattern;

        public DragPattern(A11yElement e, IUIAutomationDragPattern p) : base(e, PatternType.UIA_DragPatternId)
        {
            _pattern = p;

            // though member method is not action specific, this pattern means for UI action.
            IsUIActionable = true;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "DropEffect", Value = _pattern.CurrentDropEffect });
            Properties.Add(new A11yPatternProperty() { Name = "DropEffects", Value = GetDropEffectsString(_pattern.CurrentDropEffects) });
            Properties.Add(new A11yPatternProperty() { Name = "IsGrabbed", Value = Convert.ToBoolean(_pattern.CurrentIsGrabbed) });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        private static dynamic GetDropEffectsString(Array effects)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');

            if (effects.Length > 0)
            {
                sb.Append(effects.GetValue(0));
                for (int i = 1; i < effects.Length; i++)
                {
                    sb.Append(Invariant($", {effects.GetValue(i)}"));
                }
            }
            sb.Append(']');

            return sb.ToString();
        }

        [PatternMethod]
        public IList<DesktopElement> GetGrabbedItems()
        {
            return _pattern.GetCurrentGrabbedItems()?.ToListOfDesktopElements();
        }

        protected override void Dispose(bool disposing)
        {
            if (_pattern != null)
            {
                Marshal.ReleaseComObject(_pattern);
                _pattern = null;
            }

            base.Dispose(disposing);
        }
    }
}

