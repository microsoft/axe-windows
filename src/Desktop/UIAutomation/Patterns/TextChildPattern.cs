// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for TextChild Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/hh448740(v=vs.85).aspx
    /// </summary>
    public class TextChildPattern : A11yPattern
    {
        IUIAutomationTextChildPattern _pattern;

        public TextChildPattern(A11yElement e, IUIAutomationTextChildPattern p) : base(e, PatternType.UIA_TextChildPatternId)
        {
            _pattern = p;
        }

        [PatternMethod]
        public DesktopElement TextContainer()
        {
            return new DesktopElement(_pattern.TextContainer, true, true);
        }

        [PatternMethod]
        public TextRange TextRange()
        {
            return new TextRange(_pattern.TextRange, null);
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
