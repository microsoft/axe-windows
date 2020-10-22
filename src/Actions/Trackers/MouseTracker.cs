// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Misc;
using Axe.Windows.Desktop.UIAutomation;
using Axe.Windows.Win32;
using System;
using System.Drawing;
using System.Timers;

namespace Axe.Windows.Actions.Trackers
{
    /// <summary>
    /// Class MouseSelector
    /// </summary>
    public class MouseTracker:BaseTracker
    {
        /// <summary>
        /// timer interval;
        /// </summary>
        const int DefaultTimerInterval = 25;

        /// <summary>
        /// Mouse selector interval
        /// </summary>
        public double IntervalMouseSelector
        {
            get
            {
                return this.timerMouse.Interval;
            }

            set
            {
                if (this.timerMouse != null)
                {
                    this.timerMouse.Interval = value;
                }
            }
        }

        /// <summary>
        /// Our current TreeViewMode
        /// </summary>
        public TreeViewMode TreeViewMode { get; set; }

        /// <summary>
        /// Mouse position of POI (point of intererst)
        /// </summary>
        Point POIPoint = Point.Empty;

        /// <summary>
        /// Mouse Point from last timer tick
        /// </summary>
        Point LastMousePoint = Point.Empty;

        /// <summary>
        /// Mouse timer
        /// </summary>
        Timer timerMouse;
        private readonly object _elementSetterLock = new object();

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="action"></param>
        public MouseTracker(Action<A11yElement> action) : base(action)
        {
            // set up mouse Timer
            this.timerMouse = new System.Timers.Timer(DefaultTimerInterval); // default but it will be set by config immediately.
            this.timerMouse.Elapsed += new ElapsedEventHandler(this.ontimerMouseElapsedEvent);
            this.timerMouse.AutoReset = false;// disable autoreset to do reset in timer handerl    
        }

        /// <summary>
        /// Stop or Pause mouse select action
        /// </summary>
        public override void Stop()
        {
            if (IsStarted == true)
            {
                this.timerMouse?.Stop();
                IsStarted = false;
            }

            base.Stop();
        }

        /// <summary>
        /// Start or Resume mouse select action
        /// </summary>
        public override void Start()
        {
            if (IsStarted == false)
            {
                this.timerMouse?.Start();
                IsStarted = true;
            }
        }

        /// <summary>
        /// Mouse timer event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ontimerMouseElapsedEvent(object sender, ElapsedEventArgs e)
        {
            lock (_elementSetterLock)
            {
                if (this.timerMouse != null && this.IsStarted)
                {
                    NativeMethods.GetCursorPos(out Point p);

                    if (LastMousePoint.Equals(p) && this.POIPoint.Equals(p) == false)
                    {
                        var element = GetElementBasedOnScope(A11yAutomation.NormalizedElementFromPoint(p.X, p.Y, this.TreeViewMode));

                        if (element != null && element.IsRootElement() == false && element.IsSameUIElement(this.SelectedElementRuntimeId, this.SelectedBoundingRectangle, this.SelectedControlTypeId, this.SelectedName) == false && !POIPoint.Equals(p))
                        {
                            this.SelectedElementRuntimeId = element.RuntimeId;
                            this.SelectedBoundingRectangle = element.BoundingRectangle;
                            this.SelectedControlTypeId = element.ControlTypeId;
                            this.SelectedName = element.Name;
                            this.SetElement?.Invoke(element);
                        }
                        else
                        {
                            element?.Dispose();
                            element = null;
                        }

                        POIPoint = p;
                    }

                    LastMousePoint = p;

                    this.timerMouse?.Start(); // make sure that it is enabled.
                }
            }
        }

        /// <summary>
        /// override clear logic
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            // clean up all points to make sure to start from scratch
            this.POIPoint = Point.Empty;
            this.LastMousePoint = Point.Empty;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (this.timerMouse != null)
            {
                this.timerMouse.Stop();
                this.timerMouse.Dispose();
                this.timerMouse = null;
            }

            base.Dispose(disposing);
        }
    }
}
