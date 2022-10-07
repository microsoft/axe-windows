// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using Axe.Windows.Desktop.UIAutomation.TreeWalkers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Axe.Windows.DesktopTests.UIAutomation.TreeWalkers
{
    [TestClass]
    public class TreeWalkerDataContextTests
    {
        [TestMethod]
        [Timeout(1000)]
        public void DefaultContext_Registrar_IsDefaultRegistrar()
        {
            var registrar = TreeWalkerDataContext.DefaultContext.Registrar;

            Assert.IsNotNull(registrar);
            Assert.AreSame(Registrar.GetDefaultInstance(), registrar);
        }
    }
}
