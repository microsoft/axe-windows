// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.ActionsTests.Contexts
{
    [TestClass]
    public class DefaultActionContextTests
    {
        [TestMethod]
        [Timeout(1000)]
        public void DataManager_IsDefaultDataManager()
        {
            var dataManager = DefaultActionContext.GetDefaultInstance().DataManager;
            Assert.IsNotNull(dataManager);
            Assert.AreSame(DataManager.GetDefaultInstance(), dataManager);
        }

        [TestMethod]
        [Timeout(1000)]
        public void SelectAction_IsDefaultSelectAction()
        {
            var selectAction = DefaultActionContext.GetDefaultInstance().SelectAction;
            Assert.IsNotNull(selectAction);
            Assert.AreSame(SelectAction.GetDefaultInstance(), selectAction);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Registrar_IsDefaultSelectAction()
        {
            var registrar = DefaultActionContext.GetDefaultInstance().Registrar;
            Assert.IsNotNull(registrar);
            Assert.AreSame(Registrar.GetDefaultInstance(), registrar);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Dispose_DoesNotChangeObjects()
        {
            var dataManager = DefaultActionContext.GetDefaultInstance().DataManager;
            var selectAction = DefaultActionContext.GetDefaultInstance().SelectAction;
            var registrar = DefaultActionContext.GetDefaultInstance().Registrar;

            DefaultActionContext.GetDefaultInstance().Dispose();

            Assert.AreSame(dataManager, DefaultActionContext.GetDefaultInstance().DataManager);
            Assert.AreSame(selectAction, DefaultActionContext.GetDefaultInstance().SelectAction);
            Assert.AreSame(registrar, DefaultActionContext.GetDefaultInstance().Registrar);
        }
    }
}
