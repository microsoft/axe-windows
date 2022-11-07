// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for ObjectModel Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/hh448771(v=vs.85).aspx
    /// though it has a method to call, it may not be actionable in AT perspective.
    /// </summary>
    public class ObjectModelPattern : A11yPattern
    {
        IUIAutomationObjectModelPattern _pattern;

        public ObjectModelPattern(A11yElement e, IUIAutomationObjectModelPattern p) : base(e, PatternType.UIA_ObjectModelPatternId)
        {
            _pattern = p;
        }

        [PatternMethod]
        public dynamic GetUnderlyingObjectModel()
        {
            return _pattern.GetUnderlyingObjectModel();
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
