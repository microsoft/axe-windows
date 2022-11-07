// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.Types;
using System.Runtime.InteropServices;
using UIAutomationClient;

using static System.FormattableString;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for DropTarget Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/hh707349(v=vs.85).aspx
    /// </summary>
    [PatternEvent(Id = EventType.UIA_DropTarget_DragEnterEventId)]
    [PatternEvent(Id = EventType.UIA_DropTarget_DragLeaveEventId)]
    [PatternEvent(Id = EventType.UIA_DropTarget_DroppedEventId)]
    public class DropTargetPattern : A11yPattern
    {
        IUIAutomationDropTargetPattern _pattern;

        public DropTargetPattern(A11yElement e, IUIAutomationDropTargetPattern p) : base(e, PatternType.UIA_DropTargetPatternId)
        {
            _pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "DropTargetEffect", Value = _pattern.CurrentDropTargetEffect });
            var array = _pattern.CurrentDropTargetEffects;
            if (array.Length != 0)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Properties.Add(new A11yPatternProperty() { Name = Invariant($"DropTargetEffects[{i}]"), Value = array.GetValue(i)?.ToString() });
                }
            }
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
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
