// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.Types;
using System.Collections.Generic;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.EventHandlers
{
    public class PropertyChangedEventListener : EventListenerBase, IUIAutomationPropertyChangedEventHandler
    {
        readonly int[] _propertyArray;

        /// <summary>
        /// Create an event handler and register it.
        /// </summary>
        public PropertyChangedEventListener(CUIAutomation uia, IUIAutomationElement element, TreeScope scope, HandleUIAutomationEventMessage peDelegate, int[] properties) : base(uia, element, scope, EventType.UIA_AutomationPropertyChangedEventId, peDelegate)
        {
            _propertyArray = properties;
            Init();
        }

        public override void Init()
        {
            IUIAutomation uia = IUIAutomation;
            if (uia != null)
            {
                uia.AddPropertyChangedEventHandler(Element, Scope, null, this, _propertyArray);
                IsHooked = true;
            }
        }

        public void HandlePropertyChangedEvent(IUIAutomationElement sender, int propertyId, object newValue)
        {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
            var m = EventMessage.GetInstance(EventId, sender);
#pragma warning restore CA2000

            if (m != null)
            {
                m.Properties = new List<KeyValuePair<string, dynamic>>
                {
                   new KeyValuePair<string, dynamic>("Property Id", propertyId),
                   new KeyValuePair<string, dynamic>("Property Name", PropertyType.GetInstance().GetNameById(propertyId)),
                };
                if (newValue != null)
                {
                    m.Properties.Add(new KeyValuePair<string, dynamic>(newValue.GetType().Name, newValue));
                }

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
                        if (uia != null)
                        {
                            uia.RemovePropertyChangedEventHandler(Element, this);
                        }
                    }
                }
            }
            base.Dispose(disposing);
        }
    }
}
