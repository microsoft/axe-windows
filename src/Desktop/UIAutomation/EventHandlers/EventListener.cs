// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.EventHandlers
{
    /// <summary>
    /// Generic UIAutomation event handler wrapper
    /// </summary>
    public class EventListener : EventListenerBase, IUIAutomationEventHandler
    {
        /// <summary>
        /// Create an event handler and register it.
        /// </summary>
        public EventListener(CUIAutomation uia, IUIAutomationElement element, TreeScope scope, int eventId, HandleUIAutomationEventMessage peDelegate) : base(uia, element, scope, eventId, peDelegate)
        {
            Init();
        }

        public override void Init()
        {
            IUIAutomation uia = IUIAutomation;
            if (uia != null)
            {
                uia.AddAutomationEventHandler(EventId, Element, Scope, null, this);
                IsHooked = true;
            }
        }

        public void HandleAutomationEvent(IUIAutomationElement sender, int eventId)
        {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
            var m = EventMessage.GetInstance(eventId, sender);
#pragma warning restore CA2000

            if (m != null)
            {
                ListenEventMessage(m);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!DisposedValue)
            {
                if (disposing)
                {
                    if (IsHooked)
                    {
                        IUIAutomation uia = IUIAutomation;
                        uia?.RemoveAutomationEventHandler(EventId, Element, this);
                    }
                }
            }
            base.Dispose(disposing);
        }
    }
}
