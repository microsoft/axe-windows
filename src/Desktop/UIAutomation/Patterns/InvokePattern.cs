// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for Invoke Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671279(v=vs.85).aspx
    /// </summary>
    public class InvokePattern : A11yPattern
    {
        IUIAutomationInvokePattern _pattern;

        public InvokePattern(A11yElement e, IUIAutomationInvokePattern p) : base(e, PatternType.UIA_InvokePatternId)
        {
            _pattern = p;
            IsUIActionable = true;
        }

        [PatternMethod(IsUIAction = true)]
        public void Invoke()
        {
            _pattern.Invoke();
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
