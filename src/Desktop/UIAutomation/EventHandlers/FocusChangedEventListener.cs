// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Desktop.Types;
using System;
using System.Threading;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.EventHandlers
{
    public class FocusChangedEventListener : IDisposable, IUIAutomationFocusChangedEventHandler
    {
        public HandleUIAutomationEventMessage ListenEventMessage { get; }

        /// <summary>
        /// indicate whether event handler is hooked or not.
        /// </summary>
        bool IsHooked { get; set; }

        /// <summary>
        /// indicate whether listener is ready to process the data
        /// </summary>
        public bool ReadyToListen { get; set; }

        private IUIAutomation _uia;

        public FocusChangedEventListener(IUIAutomation uia, HandleUIAutomationEventMessage peDelegate)
        {
            _uia = uia ?? throw new ArgumentNullException(nameof(uia));
            ListenEventMessage = peDelegate;
            _uia.AddFocusChangedEventHandler(null, this);
            IsHooked = true;
        }

        public void HandleFocusChangedEvent(IUIAutomationElement sender)
        {
            if (ReadyToListen)
            {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
                var m = EventMessage.GetInstance(EventType.UIA_AutomationFocusChangedEventId, sender);
#pragma warning restore CA2000

                if (m != null)
                {
                    ListenEventMessage(m);
                }
            }
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (IsHooked && _uia != null)
                {
                    try
                    {
                        _uia.RemoveFocusChangedEventHandler(this);
                        _uia = null;
                    }
                    catch (ThreadAbortException)
                    {
                        // silently ignore exception since a ThreadAbortException is thrown at the process exit.
                    }
                }

                _disposedValue = true;
            }
        }

        ~FocusChangedEventListener()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
