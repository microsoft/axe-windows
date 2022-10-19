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

        [TestMethod]
        [Timeout(1000)]
        public void DefaultContext_A11yAutomation_IsDefaultRegistrar()
        {
            var a11yAutomation = DesktopDataContext.DefaultContext.A11yAutomation;

            Assert.IsNotNull(a11yAutomation);
            Assert.AreSame(A11yAutomation.GetDefaultInstance(), a11yAutomation);
        }

        [TestMethod]
        [Timeout(1000)]
        public void DefaultContext_CancellationToken_IsCancellationTokenNone()
        {
            var cancellationToken = DesktopDataContext.DefaultContext.CancellationToken;

            Assert.AreEqual(CancellationToken.None, cancellationToken);
        }

        [TestMethod]
        [Timeout(1000)]
        public void CancellationToken_CancelSource_CancelsToken()
        {
            var source = new CancellationTokenSource();

            DesktopDataContext dataContext = new DesktopDataContext(null, null, source.Token);

            Assert.IsFalse(dataContext.CancellationToken.IsCancellationRequested);
            source.Cancel();
            Assert.IsTrue(dataContext.CancellationToken.IsCancellationRequested);
        }
    }
}
