// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Misc;
using Axe.Windows.Desktop.Types;
using System.Collections.Generic;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.EventHandlers
{
    public class StructureChangedEventListener : EventListenerBase, IUIAutomationStructureChangedEventHandler
    {
        /// <summary>
        /// Create an event handler and register it.
        /// </summary>
        public StructureChangedEventListener(CUIAutomation uia, IUIAutomationElement element, TreeScope scope, HandleUIAutomationEventMessage peDelegate) : base(uia, element, scope, EventType.UIA_StructureChangedEventId, peDelegate)
        {
            Init();
        }

        public override void Init()
        {
            IUIAutomation uia = IUIAutomation;
            if (uia != null)
            {
                uia.AddStructureChangedEventHandler(Element, Scope, null, this);
                IsHooked = true;
            }
        }

        public void HandleStructureChangedEvent(IUIAutomationElement sender, StructureChangeType changeType, int[] runtimeId)
        {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
            var m = EventMessage.GetInstance(EventType.UIA_StructureChangedEventId, sender);
#pragma warning restore CA2000

            if (m != null)
            {
                m.Properties = new List<KeyValuePair<string, dynamic>>
                {
                    new KeyValuePair<string, dynamic>("StructureChangeType", changeType),
                    new KeyValuePair<string, dynamic>("Runtime Id", runtimeId.ConvertInt32ArrayToString()),
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
                        IUIAutomation uia = IUIAutomation;
                        uia?.RemoveStructureChangedEventHandler(Element, this);
                    }
                }
            }
            base.Dispose(disposing);
        }
    }
}
