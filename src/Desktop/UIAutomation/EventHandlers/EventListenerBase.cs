// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.EventHandlers
{
    /// <summary>
    /// Indicate the message from Event
    /// </summary>
    /// <param name="txt"></param>
    public delegate void HandleUIAutomationEventMessage(EventMessage em);

    public abstract class EventListenerBase : IDisposable
    {
        public int EventId { get; }
        public IUIAutomationElement Element { get; }

        public HandleUIAutomationEventMessage ListenEventMessage { get; }
        public TreeScope Scope { get; }
        public bool IsHooked { get; protected set; }
        private CUIAutomation _uiAutomation;
        private CUIAutomation8 _uiAutomation8;

        protected IUIAutomation IUIAutomation => _uiAutomation as IUIAutomation;
        protected IUIAutomation4 IUIAutomation4 => _uiAutomation8 as IUIAutomation4;
        protected IUIAutomation5 IUIAutomation5 => _uiAutomation8 as IUIAutomation5;
        protected IUIAutomation6 IUIAutomation6 => _uiAutomation8 as IUIAutomation6;

        /// <summary>
        /// Constructor to create an event handler (with CUIAutomation8) and register it.
        /// </summary>
        protected EventListenerBase(CUIAutomation8 uia8, IUIAutomationElement element, TreeScope scope, int eventId, HandleUIAutomationEventMessage peDelegate)
            : this(element, scope, eventId, peDelegate)
        {
            _uiAutomation8 = uia8;
        }

        /// <summary>
        /// Constructor to create an event handler (with CUIAutomation) and register it.
        /// </summary>
        protected EventListenerBase(CUIAutomation uia, IUIAutomationElement element, TreeScope scope, int eventId, HandleUIAutomationEventMessage peDelegate)
            : this(element, scope, eventId, peDelegate)
        {
            _uiAutomation = uia;
        }

        /// <summary>
        /// Private constructor to set common fields
        /// </summary>
        protected EventListenerBase(IUIAutomationElement element, TreeScope scope, int eventId, HandleUIAutomationEventMessage peDelegate)
        {
            EventId = eventId;
            Element = element;
            ListenEventMessage = peDelegate;
            Scope = scope;
        }

        /// <summary>
        /// Initialize event handler
        /// it should be called in side of derived class constructor.
        /// </summary>
        public virtual void Init()
        {
        }

        #region IDisposable Support
        protected bool disposedValue { get; private set; } // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (disposing && IsHooked)
                    {
                        _uiAutomation = null;
                        _uiAutomation8 = null;
                    }
                }

                disposedValue = true;
            }
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
