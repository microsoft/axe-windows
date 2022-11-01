// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Desktop.Types;
using System.Collections.Generic;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.EventHandlers
{
    /// <summary>
    /// Changes event listener.
    /// It is place holder. it is not hooked up yet since AccEvent doesn't support it yet.
    /// </summary>
    public class ChangesEventListener : EventListenerBase, IUIAutomationChangesEventHandler
    {
        private static readonly int[] ChangeTypes = InitChangeTypes();

        private static int[] InitChangeTypes()
        {
            // Ids based on expected values for UiaChangeInfo.uiaId
            // https://docs.microsoft.com/en-us/windows/win32/api/uiautomationcore/ns-uiautomationcore-uiachangeinfo
            // Note: property ids are not included as they are already handled by the property changed event handler.

            var changeTypes = new List<int>
            {
                ChangeInfoType.UIA_SummaryChangeId
            };
            changeTypes.AddRange(TextAttributeType.GetInstance().Values);
            changeTypes.AddRange(AnnotationType.GetInstance().Values);
            changeTypes.AddRange(Styles.StyleId.GetInstance().Values);

            return changeTypes.ToArray();
        }

        /// <summary>
        /// Create an event handler and register it.
        /// </summary>
        public ChangesEventListener(CUIAutomation8 uia8, IUIAutomationElement element, TreeScope scope, HandleUIAutomationEventMessage peDelegate) : base(uia8, element, scope, EventType.UIA_ChangesEventId, peDelegate)
        {
            Init();
        }

        public override void Init()
        {
            IUIAutomation4 uia4 = IUIAutomation4;
            if (uia4 != null)
            {
                uia4.AddChangesEventHandler(Element, Scope, ref ChangeTypes[0], ChangeTypes.Length, null, this);
                IsHooked = true;
            }
        }

        public void HandleChangesEvent(IUIAutomationElement sender, ref UiaChangeInfo uiaChanges, int changesCount)
        {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
            var m = EventMessage.GetInstance(EventId, sender);
#pragma warning restore CA2000

            if (m != null)
            {
                ListenEventMessage(m);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (IsHooked)
                    {
                        IUIAutomation4 uia4 = IUIAutomation4;
                        if (uia4 != null)
                        {
                            uia4.RemoveChangesEventHandler(Element, this);
                        }
                    }
                }
            }
            base.Dispose(disposing);
        }
    }
}
