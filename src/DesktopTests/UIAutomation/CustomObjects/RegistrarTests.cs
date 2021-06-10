// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using Interop.UIAutomationCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.DesktopTests.UIAutomation.CustomObjects
{
    [TestClass]
    public class RegistrarTests
    {
        // Note: We normally avoid Guid.NewGuid() to make tests more consistently reproducible. This class
        // intentionally uses Guid.NewGuid to reduce the risk of the UIA state contamination between test runs.

        [TestMethod]
        [Timeout(1000)]
        public void RegisterCustomProperty_ReturnsNonZeroValue()
        {
            using (Registrar r = new Registrar())
            {
                int dynamicId = r.RegisterCustomProperty(Guid.NewGuid(), "This is a name", UIAutomationType.UIAutomationType_Double);
                Assert.AreNotEqual(0, dynamicId);
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void RegisterCustomProperty_SamePropertyTwise_ReturnsSameNonZeroValue()
        {
            using (Registrar r = new Registrar())
            {
                Guid id = Guid.NewGuid();
                const string name = "My property name";
                const UIAutomationType type = UIAutomationType.UIAutomationType_Bool;

                int dynamicId1 = r.RegisterCustomProperty(id, name, type);
                int dynamicId2 = r.RegisterCustomProperty(id, name, type);
                Assert.AreNotEqual(0, dynamicId1);
                Assert.AreEqual(dynamicId1, dynamicId2);
            }
        }
    }
}
