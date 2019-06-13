// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;
using Axe.Windows.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.AutomationTests
{
    [TestClass]
    public class TargetElementLocatorUnitTests
    {
        [TestMethod]
        [Timeout (1000)]
        public void LocateElement_NoTargetSpecifiedInParameters_ThrowsAutomationException_ErrorAutomation007()
        {
            try
            {
                TargetElementLocator.LocateRootElement(-1);
            }
            catch (AxeWindowsAutomationException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Automation017:"));
            }
        }


        [TestMethod]
        [Timeout(1000)]
        [ExpectedException(typeof(AxeWindowsAutomationException))]
        public void LocateElement_SpecifiedPIDNotExist_ThrowsAutomationException_ErrorAutomation017()
        {
            try
            {
                TargetElementLocator.LocateRootElement(-1);
            }
            catch (AxeWindowsAutomationException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Automation017:"));
                throw;
            }
        }

    }
}
