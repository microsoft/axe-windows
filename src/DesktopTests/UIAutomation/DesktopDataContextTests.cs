// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.UIAutomation;
using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Axe.Windows.DesktopTests.UIAutomation
{
    [TestClass]
    public class DesktopDataContextTests
    {
        [TestMethod]
        [Timeout(1000)]
        public void DefaultContext_Registrar_IsDefaultRegistrar()
        {
            var registrar = DesktopDataContext.DefaultContext.Registrar;

            Assert.IsNotNull(registrar);
            Assert.AreSame(Registrar.GetDefaultInstance(), registrar);
        }
    }
}
