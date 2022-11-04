// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Misc;
using Axe.Windows.Desktop.Resources;
using Axe.Windows.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.TreeWalkers
{
    internal interface ITreeWalkerForLive
    {
        IList<A11yElement> Elements { get; }
        A11yElement RootElement { get; }
        void GetTreeHierarchy(A11yElement e, TreeViewMode mode, DesktopDataContext dataContext);
    }

    /// <summary>
    /// Wrapper for UIAutomation Tree Walker for Live mode.
    /// it is based on 2nd edition of TreeWalker
    /// </summary>
    internal class TreeWalkerForLive : ITreeWalkerForLive
    {
        /// <summary>
        /// List to keep all elements in tree walking(Ancestors, self and children)
        /// </summary>
        public IList<A11yElement> Elements { get; }

        /// <summary>
        /// Top root node in whole hierarchy
        /// </summary>
        public A11yElement RootElement { get; private set; }

        TreeViewMode WalkerMode;

        /// <summary>
        /// Last walk time span
        /// </summary>
        public TimeSpan LastWalkTime { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="element"></param>
        /// <param name="tf"></param>
        /// <param name="showAncestry"></param>
        public TreeWalkerForLive()
        {
            Elements = new List<A11yElement>();
        }

        /// <summary>
        /// Refresh tree node data with all children at once.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="mode"></param>
        /// <param name="dataContext"></param>
        public void GetTreeHierarchy(A11yElement e, TreeViewMode mode, DesktopDataContext dataContext)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var begin = DateTime.Now;
            WalkerMode = mode;

            //Set parent of Root explicitly for testing.
            A11yElement parent = null;
            var ancestry = new DesktopElementAncestry(WalkerMode, e, false, dataContext);
            parent = ancestry.Last;

            RootElement = ancestry.First;

            // clear children
            ListHelper.DisposeAllItemsAndClearList(e.Children);

            // populate selected element relationship and add it to list.
            e.Parent = ancestry.Last;
            e.TreeWalkerMode = WalkerMode; // set tree walker mode.
            e.UniqueId = 0; // it is the selected element which should be id 0.
            Elements.Add(e);

            PopulateChildrenTreeNode(e, ancestry.NextId, dataContext);

            // populate descendant Elements first in parallel
            Elements.AsParallel().ForAll(el =>
            {
                el.PopulateMinimumPropertiesForSelection(dataContext);
            });

            // Add ancestry into Elements list.
            foreach (var item in ancestry.Items)
            {
                Elements.Add(item);
            }

            LastWalkTime = DateTime.Now - begin;
        }

        /// <summary>
        /// Populate tree by retrieving all children at once.
        /// </summary>
        /// <param name="rootNode"></param>
        private void PopulateChildrenTreeNode(A11yElement rootNode, int startId, DesktopDataContext dataContext)
        {
            int childId = startId;

            IUIAutomationTreeWalker walker = dataContext.A11yAutomation.GetTreeWalker(WalkerMode);
            IUIAutomationElement child = (IUIAutomationElement)rootNode.PlatformObject;

            if (child != null)
            {
                try
                {
                    child = walker.GetFirstChildElement(child);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
                {
                    ex.ReportException();
                    child = null;
                    System.Diagnostics.Trace.WriteLine(ErrorMessages.TreeWalkerException.WithParameters(ex));
                }
#pragma warning restore CA1031 // Do not catch general exception types

                while (child != null)
                {
#pragma warning disable CA2000 // childNode will be disposed by the parent node
                    // Create child without populating basic property. it will be set all at once in parallel.
                    var childNode = new DesktopElement(child, true, false)
                    {
                        UniqueId = childId++
                    };
#pragma warning restore CA2000 // childNode will be disposed by the parent node

                    rootNode.Children.Add(childNode);

                    childNode.Parent = rootNode;
                    childNode.TreeWalkerMode = WalkerMode;

                    Elements.Add(childNode);
                    try
                    {
                        child = walker.GetNextSiblingElement(child);
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception ex)
                    {
                        ex.ReportException();
                        child = null;
                        System.Diagnostics.Trace.WriteLine(ErrorMessages.TreeWalkerException.WithParameters(ex));
                    }
#pragma warning restore CA1031 // Do not catch general exception types
                }
            }

            Marshal.ReleaseComObject(walker);
        }
    }
}
