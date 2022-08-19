﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Misc;
using Axe.Windows.Desktop.UIAutomation.TreeWalkers;
using Axe.Windows.Telemetry;
using Axe.Windows.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation
{
    /// <summary>
    /// Wrapper for CUIAutomation COM object
    /// </summary>
    public static class A11yAutomation
    {
        static readonly IUIAutomation UIAutomation = GetUIAutomationInterface();

        private static IUIAutomation GetUIAutomationInterface()
        {
            return Win32Helper.IsWindows7()
                ? new CUIAutomation() as IUIAutomation
                : new CUIAutomation8() as IUIAutomation;
        }

        /// <summary>
        /// The IUIAutomation object currently in use.
        /// </summary>
        public static IUIAutomation UIAutomationObject => UIAutomation;

        /// <summary>
        /// Get DesktopElement based on Process Id.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns>return null if fail to get an element by process Id</returns>
        public static DesktopElement ElementFromProcessId(int pid)
        {
            var elements = ElementsFromProcessId(pid);
            return elements?.First();
        }

        private static bool FindProcessMatchingChildren(IUIAutomationElement parent, IUIAutomationTreeWalker walker, int pid, IList<IUIAutomationElement> matchingElements, IList<IUIAutomationElement> nonMatchingElements)
        {
            for (var child = walker.GetFirstChildElement(parent); child != null; child = walker.GetNextSiblingElement(child))
            {
                if (child.CurrentProcessId == pid)
                {
                    matchingElements.Add(child);
                }
                else
                {
                    nonMatchingElements.Add(child);
                }
            }

            return matchingElements.Any();
        }

        private static IList<IUIAutomationElement> FindProcessMatchingChildrenOrGrandchildren(IUIAutomationElement root, int pid)
        {
            IUIAutomationTreeWalker walker = GetTreeWalker(TreeViewMode.Control);
            List<IUIAutomationElement> matchingElements = new List<IUIAutomationElement>();
            List<IUIAutomationElement> nonMatchingElements = new List<IUIAutomationElement>();
            List<IUIAutomationElement> nonMatchingElementsSecondLevel = new List<IUIAutomationElement>();
            try
            {
                if (FindProcessMatchingChildren(root, walker, pid, matchingElements, nonMatchingElements))
                {
                    return matchingElements;
                }

                foreach (var nonMatchingElement in nonMatchingElements)
                {
                    if (FindProcessMatchingChildren(nonMatchingElement, walker, pid, matchingElements, nonMatchingElementsSecondLevel))
                    {
                        return matchingElements;
                    }
                }
            }
            finally
            {
                ReleaseElements(walker, nonMatchingElements, nonMatchingElementsSecondLevel);
            }
            return matchingElements; // Will always be empty
        }

        private static void ReleaseElements(IUIAutomationTreeWalker walker, params IList<IUIAutomationElement>[] elementLists)
        {
            foreach (var list in elementLists)
            {
                foreach(var element in list)
                {
                    Marshal.ReleaseComObject(element);
                }
            }
            Marshal.ReleaseComObject(walker);
        }

        /// <summary>
        /// Get DesktopElements based on Process Id.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns>return null if we fail to get elements by process Id</returns>
        public static IEnumerable<DesktopElement> ElementsFromProcessId(int pid)
        {
            IUIAutomationElement root = null;
            IEnumerable<DesktopElement> elements = null;
            IList<IUIAutomationElement> matchingElements;

            try
            {
                // check whether process exists first.
                // if not, it will throw an ArgumentException
                using (var proc = Process.GetProcessById(pid))
                {
                    root = UIAutomation.GetRootElement();
                    matchingElements = FindProcessMatchingChildrenOrGrandchildren(root, pid);
                    elements = ElementsFromUIAElements(matchingElements);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                // report and let it return null
                ex.ReportException();
            }
#pragma warning restore CA1031 // Do not catch general exception types
            finally
            {
                if (root != null)
                {
                    Marshal.ReleaseComObject(root);
                }
            }

            return elements;
        }

        /// <summary>
        /// Get the top level DesktopElement from Windows handle
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static DesktopElement ElementFromHandle(IntPtr hWnd)
        {
            try
            {
                return ElementFromUIAElement(UIAutomation.ElementFromHandle(hWnd));
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                e.ReportException();
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return null;
        }

        /// <summary>
        /// Get DesktopElement from UIAElement interface.
        /// </summary>
        /// <param name="uia"></param>
        /// <returns></returns>
        private static DesktopElement ElementFromUIAElement(IUIAutomationElement uia)
        {
            if (uia != null)
            {
                if (!DesktopElement.IsFromCurrentProcess(uia))
                {
                    var el = new DesktopElement(uia, true, false);

                    el.PopulateMinimumPropertiesForSelection();

                    return el;
                }
                else
                {
                    Marshal.ReleaseComObject(uia);
                }
            }

            return null;
        }

        /// <summary>
        /// Get DesktopElements from UIAElements.
        /// </summary>
        /// <param name="uia"></param>
        /// <returns>An IEnumerable of <see cref="DesktopElement"/></returns>
        private static IEnumerable<DesktopElement> ElementsFromUIAElements(IList<IUIAutomationElement> elementList)
        {
            if (elementList == null) throw new ArgumentNullException(nameof(elementList));

            // Return an empty IEnumerable<DesktopElement> instead of null from ElementsFromUIAElements so that downstream calls to Linq extensions on the IEnumerable don't throw null reference exceptions.
            if (!elementList.Any()) return Enumerable.Empty<DesktopElement>();

            // This function was originally an iterator
            // Meaning it used the yield keyword to yield return each element
            // But that caused a com exception IRL
            // So now we use a list
            List<DesktopElement> elements = new List<DesktopElement>();

            foreach (var uiaElement in elementList)
            {
                var e = ElementFromUIAElement(uiaElement);
                if (e == null) continue;

                elements.Add(e);
            } // for each element

            return elements;
        }

        /// <summary>
        /// Get the top level IUIAutomationElement from Windows handle
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static IUIAutomationElement UIAElementFromHandle(IntPtr hWnd)
        {
            try
            {
                return UIAutomation.ElementFromHandle(hWnd);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                e.ReportException();
                return null;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        /// <summary>
        /// Get the focused Element
        /// </summary>
        /// <returns></returns>
        public static DesktopElement GetFocusedElement()
        {
            try
            {
                var uia = UIAutomation.GetFocusedElement();

                if (!DesktopElement.IsFromCurrentProcess(uia))
                {
                    return new DesktopElement(uia, true);
                }
                else
                {
                    Marshal.ReleaseComObject(uia);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                e.ReportException();
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return null;
        }

        /// <summary>
        /// Get an App element from uia
        /// it is trace back to the top most ancestor with same process Id.
        /// </summary>
        /// <param name="e">A11yElement</param>
        /// <returns></returns>
        public static A11yElement GetAppElement(A11yElement e)
        {
            var walker = GetTreeWalker(TreeViewMode.Control);
            var tree = new DesktopElementAncestry(TreeViewMode.Control, e);
            Marshal.ReleaseComObject(walker);
            A11yElement app = tree.First;

            // make sure that we have an app first
            if (app != null)
            {
                // get first item under Desktop. if app is not Desktop, then it is OK to take it as-is.
                if (app.IsRootElement())
                {
                    app = app.Children.First();
                }

                tree.Items.Remove(app);

                ListHelper.DisposeAllItems(tree.Items);

                // make sure that Unique ID is set to 0 since this element will be POI.
                app.UniqueId = 0;
            }

            return app;
        }

        /// <summary>
        /// Normalize an element to the specified TreeViewMode
        /// </summary>
        /// <param name="element">A11yElement</param>
        /// <param name="treeViewMode">mode to normalize to</param>
        /// <returns></returns>
        public static A11yElement GetNormalizedElement(A11yElement element, TreeViewMode treeViewMode)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            var walker = GetTreeWalker(treeViewMode);
            var normalizedElement = walker.NormalizeElement((IUIAutomationElement)element.PlatformObject);
            Marshal.ReleaseComObject(walker);

            return new DesktopElement(normalizedElement, true);
        }

        /// <summary>
        /// Get IUIAutomationTreeWalker based on indicated mode.
        /// </summary>
        /// <param name="mode">TreeViewMode to get walker</param>
        /// <returns></returns>
        public static IUIAutomationTreeWalker GetTreeWalker(TreeViewMode mode)
        {
            IUIAutomationTreeWalker walker = null;

            var uia = A11yAutomation.UIAutomationObject;

            switch (mode)
            {
                case TreeViewMode.Content:
                    walker = uia.ContentViewWalker;
                    break;
                case TreeViewMode.Control:
                    walker = uia.ControlViewWalker;
                    break;
                case TreeViewMode.Raw:
                    walker = uia.RawViewWalker;
                    break;
            }

            return walker;
        }

        /// <summary>
        /// Get DesktopElement from x, y position
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <returns></returns>
        public static DesktopElement ElementFromPoint(int xPos, int yPos)
        {
            try
            {
                var uia = UIAutomation.ElementFromPoint(new tagPOINT() { x = xPos, y = yPos });

                if (!DesktopElement.IsFromCurrentProcess(uia))
                {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
                    var e = new DesktopElement(uia, true, false);
#pragma warning restore CA2000
                    e.PopulateMinimumPropertiesForSelection();

                    return e;
                }
                else
                {
                    Marshal.ReleaseComObject(uia);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                e.ReportException();
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return null;
        }

        /// <summary>
        /// Get a DesktopElement from x, y position, normalized to treeViewMode
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <param name="treeViewMode">current TreeViewMode</param>
        /// <returns></returns>
        public static A11yElement NormalizedElementFromPoint(int xPos, int yPos, TreeViewMode treeViewMode)
        {
            try
            {
                A11yElement element = ElementFromPoint(xPos, yPos);

                if (element == null)
                    return null;

                if (treeViewMode == TreeViewMode.Raw)
                    return element;

                if (treeViewMode == TreeViewMode.Control && element.IsControlElement)
                    return element;

                if (treeViewMode == TreeViewMode.Content && element.IsContentElement)
                    return element;

                using (element)
                {
                    return GetNormalizedElement(element, treeViewMode);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                e.ReportException();
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return null;
        }

        /// <summary>
        /// Get Property programmatic Name from UIA service.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static string GetPropertyProgrammaticName(int pid)
        {
            return UIAutomation.GetPropertyProgrammaticName(pid);
        }

        /// <summary>
        /// Get DesktopElement from Desktop
        /// </summary>
        /// <returns></returns>
        public static DesktopElement ElementFromDesktop()
        {
            return new DesktopElement(UIAutomation.GetRootElement());
        }
    }
}
