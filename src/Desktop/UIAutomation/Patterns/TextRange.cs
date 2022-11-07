// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Attributes;
using Axe.Windows.Desktop.Utility;
using Axe.Windows.Telemetry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Wrapper for IUIAutomationTextRange
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671377(v=vs.85).aspx
    /// </summary>
    public class TextRange : IDisposable
    {
        /// <summary>
        /// UIAInterface of TextRange
        /// </summary>
        public IUIAutomationTextRange UIATextRange { get; private set; }

        /// <summary>
        /// TextPattern which this Range was from
        /// </summary>
        public TextPattern TextPattern { get; }

        public TextRange(IUIAutomationTextRange tr, TextPattern tp)
        {
            UIATextRange = tr;
            TextPattern = tp;
        }

        [PatternMethod]
        public void Select()
        {
            UIATextRange.Select();
        }

        [PatternMethod]
        public void AddToSelection()
        {
            UIATextRange.AddToSelection();
        }

        [PatternMethod]
        public void RemoveFromSelection()
        {
            UIATextRange.RemoveFromSelection();
        }

        [PatternMethod]
        public void ScrollIntoView(bool alignToTop)
        {
            UIATextRange.ScrollIntoView(alignToTop ? 1 : 0);
        }

        [PatternMethod]
        public IList<DesktopElement> GetChildren()
        {
            return UIATextRange.GetChildren()?.ToListOfDesktopElements();
        }

        [PatternMethod]
        public DesktopElement GetEnclosingElement()
        {
            return new DesktopElement(UIATextRange.GetEnclosingElement());
        }

        [PatternMethod]
        public int Move(TextUnit unit, int count)
        {
            return UIATextRange.Move(unit, count);
        }

        [PatternMethod]
        public void MoveEndpointByRange(TextPatternRangeEndpoint srcEndPoint, TextRange tr, TextPatternRangeEndpoint targetEndPoint)
        {
            if (tr == null) throw new ArgumentNullException(nameof(tr));

            UIATextRange.MoveEndpointByRange(srcEndPoint, tr.UIATextRange, targetEndPoint);
        }

        [PatternMethod]
        public int MoveEndpointByUnit(TextPatternRangeEndpoint endpoint, TextUnit unit, int count)
        {
            return UIATextRange.MoveEndpointByUnit(endpoint, unit, count);
        }

        [PatternMethod]
        public void ExpandToEnclosingUnit(TextUnit tu)
        {
            UIATextRange.ExpandToEnclosingUnit(tu);
        }

        [PatternMethod]
        public TextRange Clone()
        {
            return new TextRange(UIATextRange.Clone(), TextPattern);
        }

        [PatternMethod]
        public bool Compare(TextRange tr)
        {
            if (tr == null) throw new ArgumentNullException(nameof(tr));

            return Convert.ToBoolean(UIATextRange.Compare(tr.UIATextRange));
        }

        [PatternMethod]
        public int CompareEndpoints(TextPatternRangeEndpoint srcEndPoint, TextRange tr, TextPatternRangeEndpoint targetEndPoint)
        {
            if (tr == null) throw new ArgumentNullException(nameof(tr));

            return UIATextRange.CompareEndpoints(srcEndPoint, tr.UIATextRange, targetEndPoint);
        }

        [PatternMethod]
        public TextRange FindAttribute(int attr, object val, bool backward)
        {
            var uiatr = UIATextRange.FindAttribute(attr, val, backward ? 1 : 0);
            return uiatr != null ? new TextRange(uiatr, TextPattern) : null;
        }

        [PatternMethod]
        public TextRange FindText(string text, bool backward, bool ignoreCase)
        {
            var uiatr = UIATextRange.FindText(text, backward ? 1 : 0, ignoreCase ? 1 : 0);
            return uiatr != null ? new TextRange(uiatr, TextPattern) : null;
        }

        /// <summary>
        /// it is not still clear on how we will handle GetAttributeValue().
        /// ToDo Item.
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        [PatternMethod]
        public dynamic GetAttributeValue(int attr)
        {
            try
            {
                return UIATextRange.GetAttributeValue(attr);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                e.ReportException();
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return null;
        }

        [PatternMethod]
        public IList<Rectangle> GetBoundingRectangles()
        {
            List<Rectangle> list = new List<Rectangle>();

            var arr = UIATextRange.GetBoundingRectangles();
            for (int i = 0; i < arr.Length; i += 4)
            {
                list.Add(new Rectangle(Convert.ToInt32((double)arr.GetValue(i)), Convert.ToInt32((double)arr.GetValue(i + 1)), Convert.ToInt32((double)arr.GetValue(i + 2)), Convert.ToInt32((double)arr.GetValue(i + 3))));
            }

            return list;
        }

        [PatternMethod]
        public string GetText(int max)
        {
            return UIATextRange.GetText(max);
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (UIATextRange != null)
                {
                    Marshal.ReleaseComObject(UIATextRange);
                    UIATextRange = null;
                }
                _disposedValue = true;
            }
        }

        ~TextRange()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
