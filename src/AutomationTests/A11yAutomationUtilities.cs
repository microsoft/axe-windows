// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Enums;
using Axe.Windows.Desktop.UIAutomation;
using System.Runtime.InteropServices;
using UIAutomationClient;

namespace Axe.Windows.AutomationTests
{
    /// <summary>
    /// Wrapper for CUIAutomation COM object
    /// </summary>
    public class A11yAutomationUtilities
    {
        internal static DesktopElement GetFocusedElement()
        {
            IUIAutomation uiAutomation = A11yAutomation.GetDefaultInstance().UIAutomation;
            IUIAutomationElement focusedElement = uiAutomation.GetFocusedElement();
            return new DesktopElement(focusedElement, keepElement: true, setMembers: true);
        }

        internal static DesktopElement GetDepthFirstLastLeafControlElement(DesktopElement rootElement)
        {
            var walker = A11yAutomation.GetDefaultInstance().GetTreeWalker(TreeViewMode.Control);
            try
            {
                IUIAutomationElement leafElement = (IUIAutomationElement)rootElement.PlatformObject;
                for (IUIAutomationElement currentElement = walker.GetLastChildElement(leafElement); currentElement != null; currentElement = walker.GetLastChildElement(leafElement))
                {
                    Marshal.ReleaseComObject(leafElement);
                    leafElement = currentElement;
                }

                return leafElement is null
                    ? rootElement
                    : new DesktopElement(leafElement, keepElement: true, setMembers: true);
            }
            finally
            {
                Marshal.ReleaseComObject(walker);
            }
        }
    }
}
