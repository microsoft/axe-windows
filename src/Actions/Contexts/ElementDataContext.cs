// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions.Enums;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Misc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Axe.Windows.Actions.Contexts
{
    /// <summary>
    ///  Class ElementDataContext
    ///  Contain Data from Element Context
    /// </summary>
    public class ElementDataContext : IDisposable
    {
        /// <summary>
        /// TreeWalker Mode;
        /// </summary>
        public TreeViewMode TreeMode { get; internal set; }

        /// <summary>
        /// Data Context Mode for this
        /// </summary>
        public DataContextMode Mode { get; internal set; }

        /// <summary>
        /// All elements in this data context
        /// Ancestors, descendants and selected element.
        /// </summary>
        public Dictionary<int, A11yElement> Elements { get; internal set; }

        /// <summary>
        /// Root Element in tree hierarchy. it is the top-most ancestor.
        /// </summary>
        public A11yElement RootElment { get; internal set; }

        /// <summary>
        /// Selected Element to populate this Data context
        /// </summary>
        public A11yElement Element { get; private set; }

        /// <summary>
        /// Focused element UniqueId in this data context
        /// typically it is used to set selected element in tree when an element is clicked from Automated checks.
        /// </summary>
        public int? FocusedElementUniqueId { get; set; }

        /// <summary>
        /// Current screenshot
        /// </summary>
        public Bitmap Screenshot { get; internal set; }

        /// <summary>
        /// Id of element which was used to grab the screenshot
        /// </summary>
        public int ScreenshotElementId { get; internal set; }

        /// <summary>
        /// Bounded counter (to track constraints for the maximum number of element)
        /// </summary>
        public BoundedCounter ElementCounter { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        internal ElementDataContext(A11yElement e, int maxElements)
        {
            Element = e;
            ElementCounter = new BoundedCounter(maxElements);
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Screenshot?.Dispose();
                    Screenshot = null;
                    Element = null;
                    if (Elements != null)
                    {
                        if (Mode == DataContextMode.Live)
                        {
                            // IUIAutomation can become non-responsive if Dispose is called in parallel.
                            // Explicitly Dispose the Element Values here to avoid this.
                            foreach (var e in Elements.Values)
                            {
                                e.Dispose();
                            }
                        }
                        else
                        {
                            // so far when it gets into test, it works OK.
                            // it will keep the same perf when switch back to Live from Test.
                            Elements.Values.AsParallel().ForAll(e => e.Dispose());
                        }

                        Elements.Clear();
                    }
                }

                _disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
