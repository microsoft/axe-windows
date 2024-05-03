// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Enums;
using Axe.Windows.Desktop.UIAutomation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using UIAutomationClient;

namespace Axe.Windows.AutomationTests
{
    /// <summary>
    /// Wrapper for CUIAutomation COM object
    /// </summary>
    public class A11yAutomationUtilities
    {
        internal static DesktopElement GetRootElement()
        {
            IUIAutomation uiAutomation = A11yAutomation.GetDefaultInstance().UIAutomation;
            IUIAutomationElement focusedElement = uiAutomation.GetRootElement();
            return new DesktopElement(focusedElement, keepElement: true, setMembers: true);
        }

        internal static DesktopElement GetFocusedElement()
        {
            IUIAutomation uiAutomation = A11yAutomation.GetDefaultInstance().UIAutomation;
            IUIAutomationElement focusedElement = uiAutomation.GetFocusedElement();
            return new DesktopElement(focusedElement, keepElement: true, setMembers: true);
        }

        internal static DesktopElement GetDepthFirstLastLeafHWNDElement(DesktopElement rootElement)
        {
            var walker = A11yAutomation.GetDefaultInstance().GetTreeWalker(TreeViewMode.Control);
            try
            {
                IUIAutomationElement rootAutomationElement = rootElement.PlatformObject;
                IUIAutomationElement leafElement = FindLastWindowHandleElement(rootAutomationElement);

                return leafElement is null
                    ? rootElement
                    : new DesktopElement(leafElement, keepElement: true, setMembers: true);
            }
            finally
            {
                Marshal.ReleaseComObject(walker);
            }

            IUIAutomationElement FindLastWindowHandleElement(IUIAutomationElement parent)
            {
                List<IUIAutomationElement> matchingElements = new();
                for (var child = walker.GetFirstChildElement(parent); child != null; child = MatchAndMoveNext(child))
                {
                }

                foreach (var element in matchingElements)
                {
                    FindLastWindowHandleElement(element);
                }

                foreach (var element in matchingElements.Take(matchingElements.Count - 1))
                {
                    Marshal.ReleaseComObject(element);
                }

                return matchingElements.LastOrDefault();

                IUIAutomationElement MatchAndMoveNext(IUIAutomationElement element)
                {
                    IUIAutomationElement next;
                    try
                    {
                        next = walker.GetNextSiblingElement(element);
                    }
                    catch(COMException e)
                    {
                        Debug.WriteLine(message: $"Error getting sibling of {element.CurrentName}: {e}");
                        next = null;
                    }

                    if (element.CurrentNativeWindowHandle != IntPtr.Zero)
                    {
                        Debug.WriteLine(message: $"HWND={element.CurrentNativeWindowHandle}; Name='{element.CurrentName}'; ClassName='{element.CurrentClassName}'");
                        matchingElements.Add(element);
                    }
                    else
                    {
                        Marshal.ReleaseComObject(element);
                    }

                    return next;
                }
            }
        }
    }
}
