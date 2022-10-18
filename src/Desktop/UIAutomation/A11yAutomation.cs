// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Misc;
using Axe.Windows.Desktop.UIAutomation.CustomObjects;
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
    public class A11yAutomation
    {
        static readonly IUIAutomation UIAutomation = GetUIAutomationInterface();

        static readonly Lazy<A11yAutomation> Lazy = new Lazy<A11yAutomation>(() => CreateInstance());

        internal static A11yAutomation GetDefaultInstance() => Lazy.Value;

        private readonly IUIAutomation _uia;

        internal A11yAutomation(IUIAutomation uia)
        {
            _uia = uia;
        }

        internal static A11yAutomation CreateInstance()
        {
            return new A11yAutomation(GetUIAutomationInterface());
        }

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

        private IList<IUIAutomationElement> FindProcessMatchingChildrenOrGrandchildren(IUIAutomationElement root, int pid)
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
        public IEnumerable<DesktopElement> ElementsFromProcessId(int pid, TreeWalkerDataContext dataContext)
        {
            EnsureContextConsistency(dataContext);

            IUIAutomationElement root = null;
            IEnumerable<DesktopElement> elements = null;
            IList<IUIAutomationElement> matchingElements;

            try
            {
                // check whether process exists first.
                // if not, it will throw an ArgumentException
                using (var proc = Process.GetProcessById(pid))
                {
                    root = _uia.GetRootElement();
                    matchingElements = FindProcessMatchingChildrenOrGrandchildren(root, pid);
                    elements = ElementsFromUIAElements(matchingElements, dataContext);
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
        /// Get DesktopElement from UIAElement interface.
        /// </summary>
        /// <param name="uia"></param>
        /// <returns></returns>
        private DesktopElement ElementFromUIAElement(IUIAutomationElement uia, TreeWalkerDataContext dataContext)
        {
            EnsureContextConsistency(dataContext);
            if (uia != null)
            {
                if (!DesktopElement.IsFromCurrentProcess(uia))
                {
                    var el = new DesktopElement(uia, true, false);

                    el.PopulateMinimumPropertiesForSelection(dataContext);

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
        private IEnumerable<DesktopElement> ElementsFromUIAElements(IList<IUIAutomationElement> elementList, TreeWalkerDataContext dataContext)
        {
            EnsureContextConsistency(dataContext);

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
                var e = ElementFromUIAElement(uiaElement, dataContext);
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
        public IUIAutomationElement UIAElementFromHandle(IntPtr hWnd)
        {
            try
            {
                return _uia.ElementFromHandle(hWnd);
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
        public DesktopElement GetFocusedElement()
        {
            try
            {
                var uia = _uia.GetFocusedElement();

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
        /// <param name="dataContext">This MUST be the context that contains thia A11yAutomation object</param>
        /// <returns></returns>
        internal A11yElement GetAppElement(A11yElement e, TreeWalkerDataContext dataContext)
        {
            if (!ReferenceEquals(dataContext.A11yAutomation, this)) throw new ArgumentException("Called on wrong context", nameof(dataContext));

            // TODOL Check for consistency
            var walker = GetTreeWalker(TreeViewMode.Control);
            var tree = new DesktopElementAncestry(TreeViewMode.Control, e, false, dataContext);
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
        public A11yElement GetNormalizedElement(A11yElement element, TreeViewMode treeViewMode)
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
        public IUIAutomationTreeWalker GetTreeWalker(TreeViewMode mode)
        {
            IUIAutomationTreeWalker walker = null;

            switch (mode)
            {
                case TreeViewMode.Content:
                    walker = _uia.ContentViewWalker;
                    break;
                case TreeViewMode.Control:
                    walker = _uia.ControlViewWalker;
                    break;
                case TreeViewMode.Raw:
                    walker = _uia.RawViewWalker;
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
        public DesktopElement ElementFromPoint(int xPos, int yPos)
        {
            try
            {
                var uia = _uia.ElementFromPoint(new tagPOINT() { x = xPos, y = yPos });

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
        public A11yElement NormalizedElementFromPoint(int xPos, int yPos, TreeViewMode treeViewMode)
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
        public string GetPropertyProgrammaticName(int pid)
        {
            return _uia.GetPropertyProgrammaticName(pid);
        }

        /// <summary>
        /// Get DesktopElement from Desktop
        /// </summary>
        /// <returns></returns>
        public  DesktopElement ElementFromDesktop()
        {
            return new DesktopElement(_uia.GetRootElement());
        }

        private void EnsureContextConsistency(TreeWalkerDataContext dataContext)
        {
            if (dataContext == null) throw new ArgumentNullException(nameof(dataContext));
            if (!ReferenceEquals(dataContext.A11yAutomation, this))
            {
                throw new ArgumentException("TODO: Resource string about consistency", nameof(dataContext));
            }
        }
    }
}
