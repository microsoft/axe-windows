// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Desktop.UIAutomation;
using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Axe.Windows.ActionsTests.Contexts
{
    [TestClass]
    public class ScopedActionContextTests
    {
        [TestMethod]
        [Timeout(1000)]
        public void DataManager_IsNotDefaultDataManager()
        {
            using (var actionContext = ScopedActionContext.CreateInstance(CancellationToken.None))
            {
                Assert.IsNotNull(actionContext.DataManager);
                Assert.AreNotSame(actionContext.DataManager, DataManager.GetDefaultInstance());
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void SelectAction_IsNotDefaultSelectAction()
        {
            using (var actionContext = ScopedActionContext.CreateInstance(CancellationToken.None))
            {
                Assert.IsNotNull(actionContext.SelectAction);
                Assert.AreNotSame(actionContext.SelectAction, SelectAction.GetDefaultInstance());
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void Registrar_IsNotDefaultSelectAction()
        {
            using (var actionContext = ScopedActionContext.CreateInstance(CancellationToken.None))
            {
                Assert.IsNotNull(actionContext.Registrar);
                Assert.AreNotSame(actionContext.Registrar, Registrar.GetDefaultInstance());
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void DesktopDataContext_IsNotDefaultDesktopDataContext()
        {
            using (var actionContext = ScopedActionContext.CreateInstance(CancellationToken.None))
            {
                Assert.IsNotNull(actionContext.DesktopDataContext);
                Assert.AreNotSame(actionContext.DesktopDataContext, DesktopDataContext.DefaultContext);
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void DesktopDataContextRegistrar_IsSameAsRegistrar()
        {
            using (var actionContext = ScopedActionContext.CreateInstance(CancellationToken.None))
            {
                Assert.AreSame(actionContext.DesktopDataContext.Registrar, actionContext.Registrar);
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void DesktopDataContextCancellationToken_CancelSource_CancelsToken()
        {
            var source = new CancellationTokenSource();

            using (var actionContext = ScopedActionContext.CreateInstance(source.Token))
            {
                Assert.IsFalse(actionContext.DesktopDataContext.CancellationToken.IsCancellationRequested);
                source.Cancel();
                Assert.IsTrue(actionContext.DesktopDataContext.CancellationToken.IsCancellationRequested);
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void CreateInstance_DifferentObjectsOnEachCall()
        {
            using (var actionContext1 = ScopedActionContext.CreateInstance(CancellationToken.None))
            {
                using (var actionContext2 = ScopedActionContext.CreateInstance(CancellationToken.None))
                {
                    AssertAreDifferentObjectsOfType<ScopedActionContext>(actionContext1, actionContext2);
                    AssertAreDifferentObjectsOfType<DataManager>(actionContext1.DataManager, actionContext2.DataManager);
                    AssertAreDifferentObjectsOfType<SelectAction>(actionContext1.SelectAction, actionContext2.SelectAction);
                    AssertAreDifferentObjectsOfType<Registrar>(actionContext1.Registrar, actionContext2.Registrar);
                    AssertAreDifferentObjectsOfType<DesktopDataContext>(actionContext1.DesktopDataContext, actionContext2.DesktopDataContext);
                }
            }
        }

        private void AssertAreDifferentObjectsOfType<T>(object object1, object object2)
        {
            Assert.IsInstanceOfType(object1, typeof(T));
            Assert.IsInstanceOfType(object2, typeof(T));

            Assert.IsNotNull(object1);
            Assert.IsNotNull(object2);
            Assert.AreNotSame(object1, object2);
        }
    }
}
