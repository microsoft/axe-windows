// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.Types;
using System.Collections.Generic;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.EventHandlers
{
    /// <summary>
    /// NotificationEvent listener.
    /// this event is available from Win10 RS3
    /// </summary>
    public class NotificationEventListener : EventListenerBase, IUIAutomationNotificationEventHandler
    {
        /// <summary>
        /// Create an event handler and register it.
        /// </summary>
        public NotificationEventListener(CUIAutomation8 uia8, IUIAutomationElement element, TreeScope scope, HandleUIAutomationEventMessage peDelegate) : base(uia8, element, scope, EventType.UIA_NotificationEventId, peDelegate)
        {
            Init();
        }

        public override void Init()
        {
            IUIAutomation5 uia5 = IUIAutomation5;
            if (uia5 != null)
            {
                uia5.AddNotificationEventHandler(Element, Scope, null, this);
                IsHooked = true;
            }
        }

#pragma warning disable CA1725 // Parameter names should match base declaration
        public void HandleNotificationEvent(IUIAutomationElement sender, NotificationKind notificationKind, NotificationProcessing notificationProcessing, string displayString, string activityId)
#pragma warning restore CA1725 // Parameter names should match base declaration
        {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
            var m = EventMessage.GetInstance(EventId, sender);
#pragma warning restore CA2000

            if (m != null)
            {
                m.Properties = new List<KeyValuePair<string, dynamic>>
                {
                    new KeyValuePair<string, dynamic>("NotificationKind", notificationKind.ToString()),
                    new KeyValuePair<string, dynamic>("NotificationProcessing", notificationProcessing.ToString()),
                    new KeyValuePair<string, dynamic>("Display", displayString),
                    new KeyValuePair<string, dynamic>("ActivityId", activityId),
                };
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
                        IUIAutomation5 uia5 = IUIAutomation5;
                        if (uia5 != null)
                        {
                            uia5.RemoveNotificationEventHandler(Element, this);
                        }
                    }
                }
            }
            base.Dispose(disposing);
        }
    }
}
