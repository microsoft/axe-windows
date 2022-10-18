// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions.Contexts;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Exceptions;
using Axe.Windows.Core.Misc;
using Axe.Windows.Desktop.UIAutomation;
using System;
using System.Runtime.InteropServices;
using UIAutomationClient;

namespace Axe.Windows.Actions.Trackers
{
    /// <summary>
    /// Implements UIA tree navigation logic. Currently used by the SelectAction class.
    /// The class is named to match the MouseTracker and FocusTracker classes, also used by SelectAction.
    /// This class attempts to find the requested nearby element. If successful, the SelectAction class is notified.
    /// If unsuccessful, the TreeNavigationFailedException is thrown.
    /// </summary>
    public class TreeTracker : BaseTracker
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "SelectAction")]
        readonly SelectAction SelectAction = null;
        internal TreeViewMode TreeViewMode { get; set; } = TreeViewMode.Raw;
        private readonly object _movementLock = new object();

        public TreeTracker(Action<A11yElement> action, SelectAction selectAction)
            : base(action, DefaultActionContext.GetDefaultInstance())
        {
            this.SelectAction = selectAction;
        }

        public override void Start()
        { }

        public override void Stop()
        {
            base.Stop();
        }

        public void MoveToParent()
        {
            MoveTo(this.GetParent);
        }

        private IUIAutomationElement GetParent(IUIAutomationTreeWalker treeWalker, IUIAutomationElement element)
        {
            if (element == null) return null;

            return treeWalker?.GetParentElement(element);
        }

        public void MoveToFirstChild()
        {
            MoveTo(this.GetFirstChild);
        }

        private IUIAutomationElement GetFirstChild(IUIAutomationTreeWalker treeWalker, IUIAutomationElement element)
        {
            if (element == null) return null;

            return treeWalker?.GetFirstChildElement(element);
        }

        public void MoveToLastChild()
        {
            MoveTo(this.GetLastChild);
        }

        private IUIAutomationElement GetLastChild(IUIAutomationTreeWalker treeWalker, IUIAutomationElement element)
        {
            if (element == null) return null;

            return treeWalker?.GetLastChildElement(element);
        }

        public void MoveToNextSibling()
        {
            MoveTo(this.GetNextSibling);
        }

        private IUIAutomationElement GetNextSibling(IUIAutomationTreeWalker treeWalker, IUIAutomationElement element)
        {
            if (element == null) return null;

            return treeWalker?.GetNextSiblingElement(element);
        }

        public void MoveToPreviousSibling()
        {
            MoveTo(this.GetPreviousSibling);
        }

        private IUIAutomationElement GetPreviousSibling(IUIAutomationTreeWalker treeWalker, IUIAutomationElement element)
        {
            if (element == null) return null;

            return treeWalker?.GetPreviousSiblingElement(element);
        }

        private delegate IUIAutomationElement GetElementDelegate(IUIAutomationTreeWalker treeWalker, IUIAutomationElement element);

        /// <summary>
        /// Call this function to find a nearby element and update SelectAction if successful.
        /// This function is the starting point for the navigation logic of this class.
        /// </summary>
        /// <param name="getElementMethod">
        /// a delegate used to find the next nearby element.
        /// </param>
        private void MoveTo(GetElementDelegate getElementMethod)
        {
            lock (_movementLock)
            {
                _MoveTo(getElementMethod);
            }
        }

        /// <summary>
        /// Do not call this function directly. Instead, call MoveTo().
        /// </summary>
        /// <param name="getElementMethod"></param>
        private void _MoveTo(GetElementDelegate getElementMethod)
        {
            var element = GetNearbyElement(getElementMethod);
            if (element == null) throw new TreeNavigationFailedException();

#pragma warning disable CA2000 // Call IDisposable.Dispose()
            var desktopElement = new DesktopElement(element, true, false);
#pragma warning restore CA2000

            desktopElement.PopulateMinimumPropertiesForSelection();
            if (desktopElement.IsRootElement() == false)
            {
                this.SelectAction?.SetCandidateElement(desktopElement);
                this.SelectAction?.Select();
            }
            else
            {
                // if it is desktop, release it.
                desktopElement.Dispose();
                throw new TreeNavigationFailedException();
            }
        }

        private IUIAutomationElement GetNearbyElement(GetElementDelegate getNextElement)
        {
            var currentElement = GetCurrentElement();
            if (currentElement == null) return null;

            var treeWalker = ActionContext.DesktopDataContext.A11yAutomation.GetTreeWalker(this.TreeViewMode);

            var nextElement = getNextElement?.Invoke(treeWalker, currentElement);

            if (nextElement == null)
            {
                Marshal.ReleaseComObject(treeWalker);
                return null;
            }

            // make sure that we skip an element from current process while walking tree.
            // this code should be hit only at App level. but for sure.
            if (DesktopElement.IsFromCurrentProcess(nextElement))
            {
                var tmp = nextElement;

                nextElement = getNextElement?.Invoke(treeWalker, nextElement);

                // since element is not in use, release.
                Marshal.ReleaseComObject(tmp);
            }

            Marshal.ReleaseComObject(treeWalker);

            return nextElement;
        }

        private IUIAutomationElement GetCurrentElement()
        {
            return this.SelectAction?.POIElementContext?.Element?.PlatformObject as IUIAutomationElement;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    } // class
} // namespace
