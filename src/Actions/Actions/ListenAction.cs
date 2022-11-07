// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions.Attributes;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Actions.Enums;
using Axe.Windows.Desktop.Types;
using Axe.Windows.Desktop.UIAutomation.EventHandlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.Actions
{
    /// <summary>
    /// Class LstenAction
    /// to listen and record events from elements
    /// </summary>
    [InteractionLevel(UxInteractionLevel.NoUxInteraction)]
    public class ListenAction : IDisposable
    {
        EventListenerFactory EventListener { get; set; }
        /// <summary>
        /// External event listener. it should be called if it is not null.
        /// </summary>
        public HandleUIAutomationEventMessage ExternalListener { get; }

        public Guid Id { get; }

        bool _isRunning;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="ec"></param>
        /// <param name="listener"></param>
        private ListenAction(ListenScope listenScope, ElementContext ec, HandleUIAutomationEventMessage listener)
        {
            Id = Guid.NewGuid();
            EventListener = new EventListenerFactory(ec.Element, listenScope);
            ExternalListener = listener;
        }

        /// <summary>
        /// Start recording events
        /// </summary>
        public void Start(IEnumerable<int> eventIDs, IEnumerable<int> propertyIDs)
        {
            _isRunning = true;

            InitIndividualEventListeners(eventIDs);
            InitPropertyChangeListener(propertyIDs);
        }

        /// <summary>
        /// Start Property change Event listener
        /// </summary>
        private void InitPropertyChangeListener(IEnumerable<int> propertyIds)
        {
            if (propertyIds == null) return;
            if (!propertyIds.Any()) return;

            EventListener.RegisterAutomationEventListener(EventType.UIA_AutomationPropertyChangedEventId, OnEventFired, propertyIds.ToArray());
        }

        /// <summary>
        /// Start Individual Event Listener
        /// </summary>
        private void InitIndividualEventListeners(IEnumerable<int> eventIds)
        {
            if (eventIds == null) return;

            foreach (var id in eventIds)
            {
                EventListener.RegisterAutomationEventListener(id, OnEventFired);
            }
        }

        /// <summary>
        /// request stopping event recording and wait until it is fully done.
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            EventListener.UnregisterAllAutomationEventListners();
        }

        /// <summary>
        /// Listener for fired event
        /// need to be thread safe to add element in EventLogs
        /// </summary>
        /// <param name="message"></param>
        private void OnEventFired(EventMessage message)
        {
            if (_isRunning)
            {
                ExternalListener?.Invoke(message);
            }
        }

        #region static members
        /// <summary>
        /// Dictionary of all live listenAction instances
        /// </summary>
        static readonly Dictionary<Guid, ListenAction> AllListenActions = new Dictionary<Guid, ListenAction>();

        /// <summary>
        /// Create new Instance of ListenAction in the default DataManager instance. This does not support
        /// using a non-default DataManager
        /// </summary>
        /// <param name="config"></param>
        /// <param name="ecId"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        public static Guid CreateInstance(ListenScope listenScope, Guid ecId, HandleUIAutomationEventMessage listener)
        {
            var ec = DataManager.GetDefaultInstance().GetElementContext(ecId);
            var la = new ListenAction(listenScope, ec, listener);

            AllListenActions.Add(la.Id, la);

            return la.Id;
        }

        /// <summary>
        /// Get ListenAction instance
        /// </summary>
        /// <param name="laId">ListenAction Id</param>
        /// <returns></returns>
        public static ListenAction GetInstance(Guid laId)
        {
            return AllListenActions[laId];
        }

        /// <summary>
        /// Release Listen Action
        /// </summary>
        /// <param name="laId"></param>
        public static void ReleaseInstance(Guid laId)
        {
            var la = AllListenActions[laId];
            la.Dispose();
            AllListenActions.Remove(laId);
        }

        /// <summary>
        /// Release all ListenActions
        /// </summary>
        public static void ReleaseAll()
        {
            AllListenActions.Values.AsParallel().ForAll(la => la.Dispose());
            AllListenActions.Clear();
        }
        #endregion

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    EventListener.Dispose();
                    EventListener = null;
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
    }
}
