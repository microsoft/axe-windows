// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.UnitTestSharedLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.CoreTests.Bases
{
    /// <summary>
    /// Tests A11yPattern class
    /// </summary>
    [TestClass]
    public class A11yPatternTests
    {
        /// <summary>
        /// Test ToString and constructor for A11yPattern
        /// </summary>
        [TestMethod]
        public void ToStringTest()
        {
            A11yElement element = Utility.LoadA11yElementsFromJSON("Resources/A11yPatternTest.hier");

            Assert.AreEqual("SelectionPattern: False", element.Patterns[0].ToString());
            Assert.AreEqual("ScrollPattern: False", element.Patterns[1].ToString());
            Assert.AreEqual("ExpandCollapsePattern: 0", element.Patterns[2].ToString());
            Assert.AreEqual("ItemContainerPattern: ", element.Patterns[3].ToString());
            Assert.AreEqual("SynchronizedInputPattern: ", element.Patterns[4].ToString());
        }
    }
}
