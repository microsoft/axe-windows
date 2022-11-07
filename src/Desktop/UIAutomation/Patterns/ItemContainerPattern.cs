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
    /// Control pattern wrapper for Item Container Control Pattern
    /// </summary>
    public class ItemContainerPattern : A11yPattern
    {
        IUIAutomationItemContainerPattern _pattern;

        public ItemContainerPattern(A11yElement e, IUIAutomationItemContainerPattern p) : base(e, PatternType.UIA_ItemContainerPatternId)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            _pattern = p;
        }

        [PatternMethod]
        public DesktopElement FindItemByProperty(int propertyId, object value)
        {
            // null specifies finding the first matching item
            return new DesktopElement(_pattern.FindItemByProperty(null, propertyId, value));
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
