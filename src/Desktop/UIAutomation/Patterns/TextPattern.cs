// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.Types;
using Axe.Windows.Telemetry;
using System;
using System.Collections.Generic;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for Text Control Pattern
    /// </summary>
    [PatternEvent(Id = EventType.UIA_Text_TextChangedEventId)]
    [PatternEvent(Id = EventType.UIA_Text_TextSelectionChangedEventId)]
    public class TextPattern : A11yPattern
    {
        IUIAutomationTextPattern _pattern;

        public TextPattern(A11yElement e, IUIAutomationTextPattern p) : base(e, PatternType.UIA_TextPatternId)
        {
            // UIA sometimes throws a NotImplementedException. If it does, exclude it
            // from telemetry
            ExcludingExceptionWrapper.ExecuteWithExcludedExceptionConversion(typeof(NotImplementedException), () =>
            {
                _pattern = p;

                PopulateProperties();

                // if textPattern is supported , it means that user would do select text in the control.
                // so it should be marked as UI actionable
                IsUIActionable = true;
            });
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "SupportedTextSelection", Value = _pattern.SupportedTextSelection });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod]
        public TextRange DocumentRange()
        {
            return new TextRange(_pattern.DocumentRange, this);
        }

        [PatternMethod]
        public IList<TextRange> GetSelection()
        {
            return ToListOfTextRanges(_pattern.GetSelection());
        }

        [PatternMethod]
        public IList<TextRange> GetVisibleRanges()
        {
            return ToListOfTextRanges(_pattern.GetVisibleRanges());
        }

        [PatternMethod]
        public TextRange RangeFromChild(DesktopElement child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));

            return new TextRange(_pattern.RangeFromChild(child.PlatformObject), this);
        }

        [PatternMethod]
        public TextRange RangeFromPoint(tagPOINT pt)
        {
            return new TextRange(_pattern.RangeFromPoint(pt), this);
        }

        List<TextRange> ToListOfTextRanges(IUIAutomationTextRangeArray array)
        {
            List<TextRange> list = new List<TextRange>();

            if (array != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    list.Add(new TextRange(array.GetElement(i), this));
                }
            }

            return list;
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
