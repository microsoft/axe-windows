// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions.Contexts;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Misc;
using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.Types;
using Axe.Windows.Desktop.UIAutomation.EventHandlers;
using Axe.Windows.Desktop.UIAutomation.TreeWalkers;
using System;
using System.IO;

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
        EventListenerFactory _eventListenerFactory;
        readonly bool _isWin11;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="action"></param>
        public FocusTracker(Action<A11yElement> action) : base(action, DefaultActionContext.GetDefaultInstance())
        {
            _eventListenerFactory = new EventListenerFactory(null); // listen for all element. it works only for FocusChangedEvent
            _isWin11 = new Win32.Win32Helper().IsWindows11OrLater();
        }

        /// <summary>
        /// Stop or Pause
        /// </summary>
        public override void Stop()
        {
            if (_eventListenerFactory != null && IsStarted)
            {
                _eventListenerFactory.UnregisterAutomationEventListener(EventType.UIA_AutomationFocusChangedEventId);
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
                _eventListenerFactory?.RegisterAutomationEventListener(EventType.UIA_AutomationFocusChangedEventId, OnFocusChangedEventForSelectingElement);
                IsStarted = true;
            }
        }

        /// <summary>
        /// Handle Focus Change event
        /// </summary>
        /// <param name="message"></param>

        private void OnFocusChangedEventForSelectingElement(EventMessage message)
        {
            // only when focus is chosen for highlight
            if (message.EventId == EventType.UIA_AutomationFocusChangedEventId)
            {
                // exclude transient UI.
                if (IsStarted && message.Element != null)
                {
                    A11yElement element = GetElementBasedOnScope(message.Element);
                    if (element?.ControlTypeId != ControlType.UIA_ToolTipControlTypeId && !IsWin11TaskSwitcher(element))
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

        /// <summary>
        /// microsoft/accessibility-insights-windows#1610: The "task switching"
        /// interface that appears on Alt+Tab causes severe focus tracking
        /// interference, especially on Sun Valley.
        /// Therefore, this method detects whether this element is part of the
        /// task switcher so that it can be filtered from focus tracking.
        /// </summary>
        /// <param name="element">The element to check.</param>
        /// <returns>true if the element is part of the Alt+Tab task switching UI, false otherwise.</returns>
        private bool IsWin11TaskSwitcher(A11yElement element)
        {
            return (
                _isWin11
                && element != null
                && element.ProcessName == "explorer"
                && (
                    DoesAncestryMatchCondition(
                        element,
                        "XamlExplorerHostIslandWindow", // "PANE" WINDOW THAT SOMETIMES TAKES FOCUS WHEN initiating Alt+Tab
                        (DesktopElementAncestry anc) => anc.Items.Count == 1
                    )
                    || DoesAncestryMatchCondition(
                        element,
                        "ListViewItem", // Individual "task switching" item
                        (DesktopElementAncestry anc) => anc.Items.Count > 0 && anc.Items[0].AutomationId == "SwitchItemListControl"
                    )
                )
            );
        }

        private bool DoesAncestryMatchCondition(A11yElement element, string className, Func<DesktopElementAncestry, bool> f)
        {
            File.AppendAllText(@"c:\anc.txt", $"In DoesAncestryMatchCondition for {className}\n");
            if (element.ClassName == className)
            {
                File.AppendAllText(@"c:\anc.txt", "Class name match hit\n");
                DesktopElementAncestry ancestry = new DesktopElementAncestry(Axe.Windows.Core.Enums.TreeViewMode.Control, element, true);
                bool res = f(ancestry);
                File.AppendAllText(@"c:\anc.txt", $"Check returned {res}\n");
                ListHelper.DisposeAllItems(ancestry.Items);
                return res;
            }
            File.AppendAllText(@"c:\anc.txt", $"Class name match miss: {element.ClassName}\n");
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (_eventListenerFactory != null)
            {
                _eventListenerFactory.Dispose();
                _eventListenerFactory = null;
            }
            base.Dispose(disposing);
        }
    }
}
