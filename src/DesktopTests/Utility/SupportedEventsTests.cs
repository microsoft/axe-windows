// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.Types;
using Axe.Windows.Desktop.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Axe.Windows.DesktopTests.Utility
{
    [TestClass]
    public class SupportedEventsTests
    {
        /// <summary>
        /// Testing whether the mappings contain / don't contain correct button events
        /// </summary>
        [TestMethod]
        public void EventTypeMappingsTest()
        {
            var mappings = SupportedEvents.EventTypeMappings.ToList();
            Assert.IsTrue(mappings.Contains((ControlType.UIA_ButtonControlTypeId, EventType.UIA_Invoke_InvokedEventId, PatternType.UIA_InvokePatternId)));
            Assert.IsFalse(mappings.Contains((ControlType.UIA_ButtonControlTypeId, EventType.UIA_InputDiscardedEventId, null)));
        }
    }
}
