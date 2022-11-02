// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions.Contexts;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.Types;
using Axe.Windows.Desktop.UIAutomation.EventHandlers;
using System;

namespace Axe.Windows.Actions.Trackers
{
    /// <summary>
    /// Class FocusSelector
    /// </summary>
    public class FocusTracker : BaseTracker
    {
        /// <summary>
        /// Event Handler
        /// </summary>
        EventListenerFactory EventHandler;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="action"></param>
        public FocusTracker(Action<A11yElement> action) : base(action, DefaultActionContext.GetDefaultInstance())
        {
            EventHandler = new EventListenerFactory(null); // listen for all element. it works only for FocusChangedEvent
        }

        /// <summary>
        /// Stop or Pause
        /// </summary>
        public override void Stop()
        {
            if (EventHandler != null && IsStarted)
            {
                EventHandler.UnregisterAutomationEventListener(EventType.UIA_AutomationFocusChangedEventId);
                IsStarted = false;
            }
            base.Stop();
        }

        /// <summary>
        /// Start or Resume
        /// </summary>
        public override void Start()
        {
            if (IsStarted == false)
            {
                EventHandler?.RegisterAutomationEventListener(EventType.UIA_AutomationFocusChangedEventId, onFocusChangedEventForSelectingElement);
                IsStarted = true;
            }
        }

        /// <summary>
        /// Handle Focus Change event
        /// </summary>
        /// <param name="message"></param>

        private void onFocusChangedEventForSelectingElement(EventMessage message)
        {
            // only when focus is chosen for highlight
            if (message.EventId == EventType.UIA_AutomationFocusChangedEventId)
            {
                // exclude tooltip since it is transient UI.
                if (IsStarted && message.Element != null)
                {
                    var element = GetElementBasedOnScope(message.Element);
                    if (element?.ControlTypeId != ControlType.UIA_ToolTipControlTypeId)
                    {
                        SelectElementIfItIsEligible(element);
                    }
                }
                else
                {
                    message.Dispose();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (EventHandler != null)
            {
                EventHandler.Dispose();
                EventHandler = null;
            }
            base.Dispose(disposing);
        }
    }
}
