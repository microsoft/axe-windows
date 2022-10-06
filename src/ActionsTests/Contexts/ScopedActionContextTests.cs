// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.ActionsTests.Contexts
{
    [TestClass]
    public class ScopedActionContextTests
    {
        [TestMethod]
        [Timeout(1000)]
        public void DataManager_IsnotDefaultDataManager()
        {
            using (var actionContext = ScopedActionContext.CreateInstance())
            {
                Assert.IsNotNull(actionContext.DataManager);
                Assert.AreNotSame(actionContext.DataManager, DataManager.GetDefaultInstance());
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void SelectAction_IsNotDefaultSelectAction()
        {
            using (var actionContext = ScopedActionContext.CreateInstance())
            {
                Assert.IsNotNull(actionContext.SelectAction);
                Assert.AreNotSame(actionContext.SelectAction, SelectAction.GetDefaultInstance());
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void Registrar_IsNotDefaultSelectAction()
        {
            using (var actionContext = ScopedActionContext.CreateInstance())
            {
                Assert.IsNotNull(actionContext.Registrar);
                Assert.AreNotSame(actionContext.Registrar, Registrar.GetDefaultInstance());
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void CreateInstance_DifferentObjectsOnEachCall()
        {
            using (var actionContext1 = ScopedActionContext.CreateInstance())
            {
                using (var actionContext2 = ScopedActionContext.CreateInstance())
                {
                    AssertAreDifferentObjectsOfType<ScopedActionContext>(actionContext1, actionContext2);
                    AssertAreDifferentObjectsOfType<DataManager>(actionContext1.DataManager, actionContext2.DataManager);
                    AssertAreDifferentObjectsOfType<SelectAction>(actionContext1.SelectAction, actionContext2.SelectAction);
                    AssertAreDifferentObjectsOfType<Registrar>(actionContext1.Registrar, actionContext2.Registrar);
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
