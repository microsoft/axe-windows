// Copyright (c) Microsoft. All rights reserved.
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UIAutomationClient;
#if ENABLE_SIGNING
[assembly:         InternalsVisibleTo("AutomationTests,PublicKey=002400000480000094000000060200000024000052534131000400000100010007d1fa57c4aed9f0a32e84aa0faefd0de9e8fd6aec8f87fb03766c834c99921eb23be79ad9d5dcc1dd9ad236132102900b723cf980957fc4e177108fc607774f29e8320e92ea05ece4e821c0a5efe8f1645c4c0c93c1ab99285d622caa652c1dfad63d745d6f2de5f17e5eaf0fc4963d261c8a12436518206dc093344d5ad293")]
[assembly:InternalsVisibleTo("DynamicProxyGenAssembly2,PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]    
#else
[assembly: InternalsVisibleTo("AutomationTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
#endif

namespace Axe.Windows.Desktop.UIAutomation
{
    /// <summary>
    /// Wrapper for CUIAutomation COM object
    /// </summary>
    public class A11yAutomation
    {
        static readonly Lazy<A11yAutomation> DefaultInstanceLazy = new Lazy<A11yAutomation>(() => CreateInstance());

        static readonly object LockObject = new object();

        internal static A11yAutomation GetDefaultInstance() => DefaultInstanceLazy.Value;

        /// <summary>
        /// The IUIAutomation object currently in use in this instance
        /// </summary>
        internal IUIAutomation UIAutomation { get; }

        private A11yAutomation(IUIAutomation uiAutomation)
        {
            UIAutomation = uiAutomation;
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
                foreach (var element in list)
                {
                    Marshal.ReleaseComObject(element);
                }
            }
            Marshal.ReleaseComObject(walker);
        }

        /// <summary>
        /// Get DesktopElements based on Process Id.
        /// </summary>
        /// <param name="pid">The <see cref="Process.Id"/> whoese elements should be retrieved.</param>
        /// <param name="rootWindowHandle">
        /// The window handle for the <see cref="IUIAutomationElement"/> that should be used
        /// as the root of the sub-tree to be scanned.
        /// </param>
        /// <param name="dataContext">The data context</param>
        /// <returns>return null if we fail to get elements by process Id</returns>
        public static IEnumerable<DesktopElement> ElementsFromProcessId(int pid, IntPtr rootWindowHandle, DesktopDataContext dataContext)
        {
            if (dataContext == null) throw new ArgumentNullException(nameof(dataContext));

            IUIAutomationElement root = null;
            IEnumerable<DesktopElement> elements = null;
            IList<IUIAutomationElement> matchingElements;

            try
            {
                // check whether process exists first.
                // if not, it will throw an ArgumentException
                using (var proc = Process.GetProcessById(pid))
                {
                    // This lock should not be needed, but E2E tests have revealed that a problem
                    // exists inside UIAutomation.GetRootElement, and this works around the problem.
                    lock (LockObject)
                    {
                        root = rootWindowHandle == IntPtr.Zero
                            ? dataContext.A11yAutomation.UIAutomation.GetRootElement()
                            : dataContext.A11yAutomation.UIAutomation.ElementFromHandle(rootWindowHandle);
                    }
                    matchingElements = dataContext.A11yAutomation.FindProcessMatchingChildrenOrGrandchildren(root, pid);
                    elements = ElementsFromUIAElements(matchingElements, dataContext);
                }
            }
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
        private static DesktopElement ElementFromUIAElement(IUIAutomationElement uia, DesktopDataContext dataContext)
        {
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
        /// <returns>An IEnumerable of <see cref="DesktopElement"/></returns>
        private static IEnumerable<DesktopElement> ElementsFromUIAElements(IList<IUIAutomationElement> elementList, DesktopDataContext dataContext)
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
                var e = ElementFromUIAElement(uiaElement, dataContext);
                if (e == null) continue;

                elements.Add(e);
            } // for each element

            return elements;
        }

        /// <summary>
        /// Get an App element from uia
        /// it is trace back to the top most ancestor with same process Id.
        /// </summary>
        /// <param name="e">A11yElement</param>
        /// <param name="dataContext">The data context</param>
        /// <returns></returns>
        internal static A11yElement GetAppElement(A11yElement e, DesktopDataContext dataContext)
        {
            if (dataContext == null) throw new ArgumentNullException(nameof(dataContext));

            var walker = dataContext.A11yAutomation.GetTreeWalker(TreeViewMode.Control);
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
        internal A11yElement GetNormalizedElement(A11yElement element, TreeViewMode treeViewMode)
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
        internal IUIAutomationTreeWalker GetTreeWalker(TreeViewMode mode)
        {
            IUIAutomationTreeWalker walker = null;

            switch (mode)
            {
                case TreeViewMode.Content:
                    walker = UIAutomation.ContentViewWalker;
                    break;
                case TreeViewMode.Control:
                    walker = UIAutomation.ControlViewWalker;
                    break;
                case TreeViewMode.Raw:
                    walker = UIAutomation.RawViewWalker;
                    break;
            }

            return walker;
        }

        /// <summary>
        /// Get DesktopElement from x, y position
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <param name="dataContext">The current data context</param>
        /// <returns></returns>
        private DesktopElement ElementFromPoint(int xPos, int yPos, DesktopDataContext dataContext)
        {
            try
            {
                var uia = UIAutomation.ElementFromPoint(new tagPOINT() { x = xPos, y = yPos });

                if (!DesktopElement.IsFromCurrentProcess(uia))
                {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
                    var e = new DesktopElement(uia, true, false);
#pragma warning restore CA2000
                    e.PopulateMinimumPropertiesForSelection(dataContext);

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
        /// <param name="dataContext">The data context</param>
        /// <returns></returns>
        internal static A11yElement NormalizedElementFromPoint(int xPos, int yPos, TreeViewMode treeViewMode, DesktopDataContext dataContext)
        {
            if (dataContext == null) throw new ArgumentNullException(nameof(dataContext));

            A11yAutomation a11yAutomation = dataContext.A11yAutomation;

            try
            {
                A11yElement element = a11yAutomation.ElementFromPoint(xPos, yPos, dataContext);

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
                    return a11yAutomation.GetNormalizedElement(element, treeViewMode);
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
    }
}
