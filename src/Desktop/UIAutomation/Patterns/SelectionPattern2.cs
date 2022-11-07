﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.Types;
using Axe.Windows.Desktop.Utility;
using System;
using System.Collections.Generic;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for Selection Control Pattern2
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671285(v=vs.85).aspx
    /// </summary>
    [PatternEvent(Id = EventType.UIA_Selection_InvalidatedEventId)]
    public class SelectionPattern2 : A11yPattern
    {
        IUIAutomationSelectionPattern2 _pattern;

        public SelectionPattern2(A11yElement e, IUIAutomationSelectionPattern2 p) : base(e, PatternType.UIA_SelectionPattern2Id)
        {
            _pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "CanSelectMultiple", Value = Convert.ToBoolean(_pattern.CurrentCanSelectMultiple) });
            Properties.Add(new A11yPatternProperty() { Name = "IsSelectionRequired", Value = Convert.ToBoolean(_pattern.CurrentIsSelectionRequired) });
            Properties.Add(new A11yPatternProperty() { Name = "CurrentItemCount", Value = _pattern.CurrentItemCount });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod]
        public IList<DesktopElement> GetSelection()
        {
            return _pattern.GetCurrentSelection().ToListOfDesktopElements();
        }

        [PatternMethod]
        public DesktopElement LastSelectedItem()
        {
            return new DesktopElement(_pattern.CurrentLastSelectedItem);
        }

        [PatternMethod]
        public DesktopElement CurrentSelectedItem()
        {
            return new DesktopElement(_pattern.CurrentCurrentSelectedItem);
        }

        [PatternMethod]
        public DesktopElement FirstSelectedItem()
        {
            return new DesktopElement(_pattern.CurrentFirstSelectedItem);
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
