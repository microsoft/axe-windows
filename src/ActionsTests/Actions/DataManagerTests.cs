// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.ActionsTests.Actions
{
    [TestClass]
    public class DataManagerTests
    {
        [TestMethod]
        [Timeout(1000)]
        public void CreateInstance_Returns_DataManager()
        {
            using (var dataManager = DataManager.CreateInstance())
            {
                Assert.IsNotNull(dataManager);
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void CreateInstance_CallTwice_ReturnsDifferentObjects()
        {
            using (var dataManager1 = DataManager.CreateInstance())
            {
                using (var dataManager2 = DataManager.CreateInstance())
                {
                    Assert.IsNotNull(dataManager1);
                    Assert.IsNotNull(dataManager2);
                    Assert.AreNotSame(dataManager1, dataManager2);
                }
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void CreateInstance_GetDefaultInstance_ReturnsDifferentObjects()
        {
            using (var dataManager1 = DataManager.CreateInstance())
            {
                var dataManager2 = DataManager.GetDefaultInstance();

                Assert.IsNotNull(dataManager1);
                Assert.IsNotNull(dataManager2);
                Assert.AreNotSame(dataManager1, dataManager2);
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void GetDefaultInstance_CallTwice_ReturnsSameObjects()
        {
            var dataManager1 = DataManager.GetDefaultInstance();
            var dataManager2 = DataManager.GetDefaultInstance();

            Assert.IsNotNull(dataManager1);
            Assert.IsNotNull(dataManager2);
            Assert.AreSame(dataManager1, dataManager2);
        }

        [TestMethod]
        [Timeout(1000)]
        public void GetDefaultInstance_CallTwiceWithClearInBetween_ReturnsDifferentObjects()
        {
            var dataManager1 = DataManager.GetDefaultInstance();
            DataManager.ClearDefaultInstance();
            var dataManager2 = DataManager.GetDefaultInstance();

            Assert.IsNotNull(dataManager1);
            Assert.IsNotNull(dataManager2);
            Assert.AreNotSame(dataManager1, dataManager2);
        }
    }
}
