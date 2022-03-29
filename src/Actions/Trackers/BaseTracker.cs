// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions.Enums;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Misc;
using Axe.Windows.Desktop.UIAutomation;
using System;
using System.Drawing;

namespace Axe.Windows.Actions.Trackers
{
    /// <summary>
    /// base class for Tracker
    /// </summary>
    public abstract class BaseTracker : IDisposable
    {
        internal Action<A11yElement> SetElement;

        internal int ProcessId;

        internal bool IsStarted;

        /// <summary>
        /// keep track the Selected element RuntimeId
        /// </summary>
        private string SelectedElementRuntimeId;
        private Rectangle? SelectedBoundingRectangle;
        private int SelectedControlTypeId;
        private string SelectedName;

        /// <summary>
        /// Set the scope of selection
        /// </summary>
        public SelectionScope Scope { get; internal set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="action"></param>
        protected BaseTracker(Action<A11yElement> action)
        {
            this.SetElement = action;
        }

#pragma warning disable CA1716 // Identifiers should not match keywords
        /// <summary>
        /// Stop or Pause selector
        /// </summary>
        public virtual void Stop()
#pragma warning restore CA1716 // Identifiers should not match keywords
        {
            // clean up selection
            this.SelectedElementRuntimeId = null;
            this.SelectedBoundingRectangle = null;
        }

        /// <summary>
        /// Start or Resume selector
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Get Element based on Scope
        /// if Scope is Element, returns the current element
        /// if Scope is App, find out App element and return the App element
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected A11yElement GetElementBasedOnScope(A11yElement e)
        {
            if (e != null && Scope == SelectionScope.App)
            {
                var el = A11yAutomation.GetAppElement(e);

                // if the original selection is Top most element of the app, it should not be released.
                if (e != el)
                {
                    e.Dispose();
                }

                return el;
            }

            return e;
        }

        /// <summary>
        /// Select the specified element if it meets all eligibilty requirements
        /// </summary>
        /// <param name="element">The potential element to select</param>
        /// <returns>true if the element was selected</returns>
        protected bool SelectElementIfItIsEligible(A11yElement element)
        {
            if (element != null && !element.IsRootElement()
                && !element.IsSameUIElement(SelectedElementRuntimeId, SelectedBoundingRectangle, SelectedControlTypeId, SelectedName))
            {
                SelectedElementRuntimeId = element.RuntimeId;
                SelectedBoundingRectangle = element.BoundingRectangle;
                SelectedControlTypeId = element.ControlTypeId;
                SelectedName = element.Name;
                SetElement?.Invoke(element);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clear currently selected element info.
        /// </summary>
        public virtual void Clear()
        {
            this.SelectedElementRuntimeId = null;
            this.SelectedBoundingRectangle = null;
        }

        #region IDisposable Support
        bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BaseSelector() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

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
