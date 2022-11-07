// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Desktop.Types;
using Axe.Windows.Telemetry;
using Axe.Windows.Win32;
using System;
using System.Collections.Generic;
using System.Threading;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.EventHandlers
{
    /// <summary>
    /// Maintain the AutomationEvent handlers for UI interaction
    /// since different sets of event handlers can be created for each purpose, each factory keeps its own copy of CUIAutomation object.
    /// it will be released at disposal.
    /// </summary>
    public class EventListenerFactory : IDisposable
    {
        public FocusChangedEventListener EventListenerFocusChanged { get; private set; }
        public StructureChangedEventListener EventListenerStructureChanged { get; private set; }
        public PropertyChangedEventListener EventListenerPropertyChanged { get; private set; }
        public ChangesEventListener EventListenerChanges { get; private set; }
        public NotificationEventListener EventListenerNotification { get; private set; }
        public TextEditTextChangedEventListener EventListenerTextEditTextChanged { get; private set; }
        public ActiveTextPositionChangedEventListener EventListenerActiveTextPositionChanged { get; private set; }
        public Dictionary<int, EventListener> EventListeners { get; private set; }

        public TreeScope Scope { get; private set; }
        public CUIAutomation UIAutomation { get; private set; }
        public CUIAutomation8 UIAutomation8 { get; private set; }

        private readonly A11yElement _rootElement;

        const int ThreadExitGracePeriod = 2; // 2 seconds

        #region Message Processing part
        /// <summary>
        /// Message Queue to keep the request from callers on other thread(s)
        /// </summary>
        private readonly Queue<EventListenerFactoryMessage> _msgQueue = new Queue<EventListenerFactoryMessage>();

        private Thread _threadBackground;
        private AutoResetEvent _autoEventInit; // Event used to allow background thread to take any required initialization action.
        private AutoResetEvent _autoEventMsg; // Event used to notify the background thread that action is required.
        private AutoResetEvent _autoEventFinish; // Event used to notify the end of worker thread

        /// <summary>
        /// Start a worker thread to handle UIAutomation request on a dedicated thread
        /// </summary>
        private void StartWorkerThread()
        {
            // This sample doesn't expect to enter here with a background thread already initialized.
            if (_threadBackground != null)
            {
                return;
            }

            _autoEventFinish = new AutoResetEvent(false);
            // The main thread will notify the background thread later when it's time to close down.
            _autoEventMsg = new AutoResetEvent(false);

            // Create the background thread, and wait until it's ready to start working.
            _autoEventInit = new AutoResetEvent(false);
            ThreadStart threadStart = new ThreadStart(ProcessMessageQueue);

            _threadBackground = new Thread(threadStart);
            _threadBackground.SetApartmentState(ApartmentState.MTA); // The event handler must run on an MTA thread.
            _threadBackground.Start();

            _autoEventInit.WaitOne();
        }

        /// <summary>
        /// Worker method for private thread to process Message Queue
        /// </summary>
        private void ProcessMessageQueue()
        {
            UIAutomation = new CUIAutomation();

            // CUIAutomation8 was introduced in Windows 8, so don't try it on Windows 7.
            // Reference: https://msdn.microsoft.com/en-us/library/windows/desktop/hh448746(v=vs.85).aspx?f=255&MSPPError=-2147217396
            if (!Win32Helper.IsWindows7())
            {
                UIAutomation8 = new CUIAutomation8();
            }

            _autoEventInit.Set();

            // fCloseDown will be set true when the thread is to close down.
            bool fCloseDown = false;

            while (!fCloseDown)
            {
                // Wait here until we're told we have some work to do.
                _autoEventMsg.WaitOne();

                while (true)
                {
                    EventListenerFactoryMessage msgData;

                    // Note that none of the queue or message related action here is specific to UIA.
                    // Rather it is only a means for the main UI thread and the background MTA thread
                    // to communicate.

                    // Get a message from the queue of action-related messages.
                    lock (_msgQueue)
                    {
                        if (_msgQueue.Count != 0)
                        {
                            msgData = _msgQueue.Dequeue();
                        }
                        else
                        {
                            break;
                        }
                    }

                    switch (msgData.MessageType)
                    {
                        case EventListenerFactoryMessageType.FinishThread:
                            // The main UI thread is telling this background thread to close down.
                            fCloseDown = true;
                            break;
                        case EventListenerFactoryMessageType.RegisterEventListener:
                            RegisterEventListener(msgData);
                            break;
                        case EventListenerFactoryMessageType.UnregisterEventListener:
                            UnregisterEventListener(msgData);
                            break;
                        case EventListenerFactoryMessageType.UnregisterAllEventListeners:
                            UnregisterAllEventListener();
                            break;
                    }

                    msgData.Processed();
                }
            }

            _autoEventFinish.Set();

        }

        private void UnregisterAllEventListener()
        {
#pragma warning disable CA2000 // Call IDisposeable.Dispose()

            /// Need to find out a way to handle
            UnregisterEventListener(new EventListenerFactoryMessage
            {
                EventId = EventType.UIA_AutomationFocusChangedEventId
            });

            UnregisterEventListener(new EventListenerFactoryMessage
            {
                EventId = EventType.UIA_StructureChangedEventId
            });

            UnregisterEventListener(new EventListenerFactoryMessage
            {
                EventId = EventType.UIA_AutomationPropertyChangedEventId
            });

            UnregisterEventListener(new EventListenerFactoryMessage
            {
                EventId = EventType.UIA_NotificationEventId
            });

            UnregisterEventListener(new EventListenerFactoryMessage
            {
                EventId = EventType.UIA_TextEdit_TextChangedEventId
            });

            UnregisterEventListener(new EventListenerFactoryMessage
            {
                EventId = EventType.UIA_ActiveTextPositionChangedEventId
            });

            UnregisterEventListener(new EventListenerFactoryMessage
            {
                EventId = EventType.UIA_ChangesEventId
            });
#pragma warning restore CA2000

            HandleUIAutomationEventMessage listener = null;
            try
            {
                foreach (var e in EventListeners.Values)
                {
                    listener = e.ListenEventMessage;
                    e.Dispose();
                }
                EventListeners.Clear();
                if (listener != null)
                {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
                    var m = EventMessage.GetInstance(EventType.UIA_EventRecorderNotificationEventId, null);
#pragma warning restore CA2000

                    m.Properties = new List<KeyValuePair<string, dynamic>> { new KeyValuePair<string, dynamic>("Message", "Succeeded to unregister all event listeners.") };
                    listener(m);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                e.ReportException();

#pragma warning disable CA2000 // Call IDisposable.Dispose()
                var m = EventMessage.GetInstance(EventType.UIA_EventRecorderNotificationEventId, null);
#pragma warning restore CA2000

                m.Properties = new List<KeyValuePair<string, dynamic>> { new KeyValuePair<string, dynamic>("Message", $"Failed to unregister all listeners: {e.Message}") };
                listener(m);
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        /// <summary>
        /// Process Unregister event message
        /// </summary>
        /// <param name="msgData"></param>
        private void UnregisterEventListener(EventListenerFactoryMessage msgData)
        {
            HandleUIAutomationEventMessage listener = null;
            try
            {
                switch (msgData.EventId)
                {
                    case EventType.UIA_AutomationFocusChangedEventId:
                        if (EventListenerFocusChanged != null)
                        {
                            listener = EventListenerFocusChanged.ListenEventMessage;
                            EventListenerFocusChanged.Dispose();
                            EventListenerFocusChanged = null;
                        }
                        break;
                    case EventType.UIA_StructureChangedEventId:
                        if (EventListenerStructureChanged != null)
                        {
                            listener = EventListenerStructureChanged.ListenEventMessage;
                            EventListenerStructureChanged.Dispose();
                            EventListenerStructureChanged = null;
                        }
                        break;
                    case EventType.UIA_AutomationPropertyChangedEventId:
                        if (EventListenerPropertyChanged != null)
                        {
                            listener = EventListenerPropertyChanged.ListenEventMessage;
                            EventListenerPropertyChanged.Dispose();
                            EventListenerPropertyChanged = null;
                        }
                        break;
                    case EventType.UIA_TextEdit_TextChangedEventId:
                        if (EventListenerTextEditTextChanged != null)
                        {
                            listener = EventListenerTextEditTextChanged.ListenEventMessage;
                            EventListenerTextEditTextChanged.Dispose();
                            EventListenerTextEditTextChanged = null;
                        }
                        break;
                    case EventType.UIA_ChangesEventId:
                        if (EventListenerChanges != null)
                        {
                            listener = EventListenerChanges.ListenEventMessage;
                            EventListenerChanges.Dispose();
                            EventListenerChanges = null;
                        }
                        break;
                    case EventType.UIA_NotificationEventId:
                        if (EventListenerNotification != null)
                        {
                            listener = EventListenerNotification.ListenEventMessage;
                            EventListenerNotification.Dispose();
                            EventListenerNotification = null;
                        }
                        break;
                    case EventType.UIA_ActiveTextPositionChangedEventId:
                        if (EventListenerActiveTextPositionChanged != null)
                        {
                            listener = EventListenerActiveTextPositionChanged.ListenEventMessage;
                            EventListenerActiveTextPositionChanged.Dispose();
                            EventListenerActiveTextPositionChanged = null;
                        }
                        break;
                    default:
                        if (EventListeners.ContainsKey(msgData.EventId))
                        {
                            var l = EventListeners[msgData.EventId];
                            listener = l.ListenEventMessage;
                            EventListeners.Remove(msgData.EventId);
                            l.Dispose();
                        }
                        break;
                }

                if (listener != null)
                {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
                    var m = EventMessage.GetInstance(EventType.UIA_EventRecorderNotificationEventId, null);
#pragma warning restore CA2000

                    m.Properties = new List<KeyValuePair<string, dynamic>>()
                    {
                        new KeyValuePair<string, dynamic>("Message", "Succeeded to unregister a event listeners"),
                        new KeyValuePair<string, dynamic>("Event Id", msgData.EventId),
                        new KeyValuePair<string, dynamic>("Event Name", EventType.GetInstance().GetNameById(msgData.EventId)),
                    };
                    listener(m);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                e.ReportException();

#pragma warning disable CA2000 // Call IDisposable.Dispose()
                var m = EventMessage.GetInstance(EventType.UIA_EventRecorderNotificationEventId, null);
#pragma warning restore CA2000

                m.Properties = new List<KeyValuePair<string, dynamic>>()
                {
                    new KeyValuePair<string, dynamic>("Message", "Failed to unregister a event listeners"),
                    new KeyValuePair<string, dynamic>("Event Id", msgData.EventId),
                    new KeyValuePair<string, dynamic>("Event Name", EventType.GetInstance().GetNameById(msgData.EventId)),
                    new KeyValuePair<string, dynamic>("Error", e.Message)
                };

                listener(m);
                /// it is very unexpected situation.
                /// need to figure out a way to prevent it or handle it more gracefully
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        /// <summary>
        /// Process Register event message
        /// </summary>
        /// <param name="msgData"></param>
        private void RegisterEventListener(EventListenerFactoryMessage msgData)
        {
            try
            {
                EventMessage m = null;

                var win32Helper = new Win32Helper();

                switch (msgData.EventId)
                {
                    case EventType.UIA_AutomationFocusChangedEventId:
                        if (EventListenerFocusChanged == null)
                        {
                            var uia = (IUIAutomation)UIAutomation8 ?? UIAutomation;
                            EventListenerFocusChanged = new FocusChangedEventListener(uia, msgData.Listener);
                        }
                        break;
                    case EventType.UIA_StructureChangedEventId:
                        if (EventListenerStructureChanged == null)
                        {
                            EventListenerStructureChanged = new StructureChangedEventListener(UIAutomation, _rootElement.PlatformObject, Scope, msgData.Listener);
                        }
                        break;
                    case EventType.UIA_AutomationPropertyChangedEventId:
                        if (EventListenerPropertyChanged == null)
                        {
                            EventListenerPropertyChanged = new PropertyChangedEventListener(UIAutomation, _rootElement.PlatformObject, Scope, msgData.Listener, msgData.Properties);
                        }
                        break;
                    case EventType.UIA_TextEdit_TextChangedEventId:
                        if (EventListenerTextEditTextChanged == null)
                        {
                            EventListenerTextEditTextChanged = new TextEditTextChangedEventListener(UIAutomation8, _rootElement.PlatformObject, Scope, msgData.Listener);
                        }
                        break;
                    case EventType.UIA_ChangesEventId:
                        if (EventListenerChanges == null)
                        {
                            EventListenerChanges = new ChangesEventListener(UIAutomation8, _rootElement.PlatformObject, Scope, msgData.Listener);
                        }
                        break;
                    case EventType.UIA_NotificationEventId:
                        if (win32Helper.IsWindowsRS3OrLater())
                        {
                            if (EventListenerNotification == null)
                            {
                                EventListenerNotification = new NotificationEventListener(UIAutomation8, _rootElement.PlatformObject, Scope, msgData.Listener);
                            }
                        }
                        else
                        {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
                            m = EventMessage.GetInstance(EventType.UIA_EventRecorderNotificationEventId, null);
#pragma warning restore CA2000

                            m.Properties = new List<KeyValuePair<string, dynamic>>()
                            {
                                new KeyValuePair<string, dynamic>("Message", "Event listener registration is rejected."),
                                new KeyValuePair<string, dynamic>("Event Id", msgData.EventId),
                                new KeyValuePair<string, dynamic>("Event Name", EventType.GetInstance().GetNameById(msgData.EventId)),
                                new KeyValuePair<string, dynamic>("Reason", "Not supported platform"),
                            };
                            msgData.Listener(m);
                        }
                        break;
                    case EventType.UIA_ActiveTextPositionChangedEventId:
                        if (win32Helper.IsWindowsRS5OrLater())
                        {
                            if (EventListenerNotification == null)
                            {
                                EventListenerActiveTextPositionChanged = new ActiveTextPositionChangedEventListener(UIAutomation8, _rootElement.PlatformObject, Scope, msgData.Listener);
                            }
                        }
                        else
                        {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
                            m = EventMessage.GetInstance(EventType.UIA_EventRecorderNotificationEventId, null);
#pragma warning restore CA2000

                            m.Properties = new List<KeyValuePair<string, dynamic>>()
                            {
                                new KeyValuePair<string, dynamic>("Message", "Event listener registration is rejected."),
                                new KeyValuePair<string, dynamic>("Event Id", msgData.EventId),
                                new KeyValuePair<string, dynamic>("Event Name", EventType.GetInstance().GetNameById(msgData.EventId)),
                                new KeyValuePair<string, dynamic>("Reason", "Not supported platform"),
                            };
                            msgData.Listener(m);
                        }
                        break;
                    default:
                        if (EventListeners.ContainsKey(msgData.EventId) == false)
                        {
                            EventListeners.Add(msgData.EventId, new EventListener(UIAutomation, _rootElement.PlatformObject, Scope, msgData.EventId, msgData.Listener));
                        }
                        break;
                }

#pragma warning disable CA2000 // Call IDisposable.Dispose()
                m = EventMessage.GetInstance(EventType.UIA_EventRecorderNotificationEventId, null);
#pragma warning restore CA2000

                m.Properties = new List<KeyValuePair<string, dynamic>>()
                {
                    new KeyValuePair<string, dynamic>("Message", "Succeeded to register an event listener"),
                    new KeyValuePair<string, dynamic>("Event Id", msgData.EventId),
                    new KeyValuePair<string, dynamic>("Event Name", EventType.GetInstance().GetNameById(msgData.EventId)),
                };
                msgData.Listener(m);
                if (msgData.EventId == EventType.UIA_AutomationFocusChangedEventId)
                {
                    EventListenerFocusChanged.ReadyToListen = true;
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                e.ReportException();

#pragma warning disable CA2000 // Call IDisposable.Dispose()
                var m = EventMessage.GetInstance(EventType.UIA_EventRecorderNotificationEventId, null);
#pragma warning restore CA2000

                m.Properties = new List<KeyValuePair<string, dynamic>>()
                {
                    new KeyValuePair<string, dynamic>("Message", "Failed to register an event listener"),
                    new KeyValuePair<string, dynamic>("Event Id", msgData.EventId),
                    new KeyValuePair<string, dynamic>("Event Name", EventType.GetInstance().GetNameById(msgData.EventId)),
                    new KeyValuePair<string, dynamic>("Error", e.Message)
                };
                msgData.Listener(m);
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        /// <summary>
        /// Request Worker thread finish and wait for 2 seconds to finish. otherwise, there will be an exception.
        /// </summary>
        private void FinishWorkerThread()
        {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
            AddMessageToQueue(new EventListenerFactoryMessage() { MessageType = EventListenerFactoryMessageType.FinishThread });
#pragma warning restore CA2000

            _autoEventFinish.WaitOne(TimeSpan.FromSeconds(ThreadExitGracePeriod));
            if (_threadBackground.IsAlive)
            {
                try
                {
                    _threadBackground.Abort();
                }
                catch (PlatformNotSupportedException)
                { }  // nothing we can do. Just continue
            }
        }

        /// <summary>
        /// Add a new Factory Message
        /// </summary>
        /// <param name="msgData"></param>
        private void AddMessageToQueue(EventListenerFactoryMessage msgData)
        {
            // Request the lock, and block until it is obtained.
            lock (_msgQueue)
            {
                // When the lock is obtained, add an element.
                _msgQueue.Enqueue(msgData);
            }

            _autoEventMsg.Set();
        }
        #endregion

        public EventListenerFactory(A11yElement rootElement) : this(rootElement, ListenScope.Subtree) { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="peDelegate"></param>
        /// <param name="rootElement">can be null but it is only for global events like focusChanged</param>
        /// <param name="scope"></param>
        public EventListenerFactory(A11yElement rootElement, ListenScope scope)
        {
            _rootElement = rootElement;
            Scope = GetUIAScope(scope);
            EventListeners = new Dictionary<int, EventListener>();
            //Start worker thread
            StartWorkerThread();
        }

        private static TreeScope GetUIAScope(ListenScope listenScope)
        {
            switch (listenScope)
            {
                case ListenScope.Element:
                    return TreeScope.TreeScope_Element;
                case ListenScope.Descendants:
                    return TreeScope.TreeScope_Descendants;
                default:
                    return TreeScope.TreeScope_Subtree;
            }
        }

        /// <summary>
        /// Register a single event listener
        /// in case of property listening, since it is monolithic, you need to stop existing property listener first.
        /// the implicit cleanup is not defined.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="peDelegate"></param>
        /// <param name="properties">required only for PropertyChanged Event listening</param>
        public void RegisterAutomationEventListener(int eventId, HandleUIAutomationEventMessage peDelegate, int[] properties = null)
        {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
            AddMessageToQueue(new EventListenerFactoryMessage()
            {
                MessageType = EventListenerFactoryMessageType.RegisterEventListener,
                Listener = peDelegate,
                EventId = eventId,
                Properties = properties
            });
#pragma warning restore CA2000
        }

        /// <summary>
        /// Unregister a automation event listener
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="wait">wait to complete</param>
        public void UnregisterAutomationEventListener(int eventId, bool wait = false)
        {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
            var msg = new EventListenerFactoryMessage()
            {
                MessageType = EventListenerFactoryMessageType.UnregisterEventListener,
                EventId = eventId
            };
#pragma warning restore CA2000

            AddMessageToQueue(msg);

            if (wait)
            {
                msg.WaitForProcessed(2000);// expect to be done in 2 seconds.
            }
        }

        /// <summary>
        /// Unregister all event listeners
        /// </summary>
        public void UnregisterAllAutomationEventListners()
        {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
            var msg = new EventListenerFactoryMessage()
            {
                MessageType = EventListenerFactoryMessageType.UnregisterAllEventListeners,
            };
#pragma warning restore CA2000

            AddMessageToQueue(msg);

            msg.WaitForProcessed(2000);// expect to be done in 2 seconds.

        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    UnregisterAllAutomationEventListners();
                    FinishWorkerThread();
                    if (_autoEventInit != null)
                    {
                        _autoEventInit.Dispose();
                        _autoEventInit = null;
                    }
                    if (_autoEventFinish != null)
                    {
                        _autoEventFinish.Dispose(); ;
                        _autoEventFinish = null;
                    }
                    if (_autoEventMsg != null)
                    {
                        _autoEventMsg.Dispose();
                        _autoEventMsg = null;
                    }
                }

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AutomationEventHandlerFactory() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    #region private classes for messaging
    /// <summary>
    /// Message Type enum to support communication between UI Thread and Listener Factory Thread
    /// </summary>
    public enum EventListenerFactoryMessageType
    {
        Null,
        RegisterEventListener,
        UnregisterEventListener,
        UnregisterAllEventListeners,
        FinishThread
    };

    /// <summary>
    /// EventListener Message data
    /// it contains message type and other information for further action
    /// </summary>
    public class EventListenerFactoryMessage : IDisposable
    {
        internal EventListenerFactoryMessageType MessageType;
        internal int EventId;
        internal HandleUIAutomationEventMessage Listener;
        internal int[] Properties;

        private AutoResetEvent _autoEventProcessed; // set when the message is processed.

        public EventListenerFactoryMessage()
        {
            _autoEventProcessed = new AutoResetEvent(false);
        }

        /// <summary>
        /// Wait until message is processed.
        /// if not processed in given time, exception will be thrown.
        /// </summary>
        /// <param name="milliseconds"></param>
        public void WaitForProcessed(int milliseconds)
        {
            _autoEventProcessed.WaitOne(milliseconds);
        }

        /// <summary>
        /// signal to finish processing this message.
        /// </summary>
        internal void Processed()
        {
            _autoEventProcessed.Set();
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _autoEventProcessed.Dispose();
                    _autoEventProcessed = null;
                }

                _disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    };
    #endregion
}
