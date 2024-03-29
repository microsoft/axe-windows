﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.UnitTestSharedLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.CoreTests.Bases
{
    /// <summary>
    /// Tests A11yPatternProperty class
    /// </summary>
    [TestClass]
    public class A11yPatternPropertyTests
    {
        /// <summary>
        /// Checks NodeValue string output
        /// </summary>
        [TestMethod]
        public void NodeValueTest()
        {
            A11yElement element = Utility.LoadA11yElementsFromJSON("Resources/A11yPatternTest.hier");

            Assert.AreEqual("CanSelectMultiple = False", element.Patterns[0].Properties[0].NodeValue);
            Assert.AreEqual("IsSelectionRequired = False", element.Patterns[0].Properties[1].NodeValue);

            Assert.AreEqual("HorizontallyScrollable = False", element.Patterns[1].Properties[0].NodeValue);
            Assert.AreEqual("HorizontalScrollPercent  = -1", element.Patterns[1].Properties[1].NodeValue);
            Assert.AreEqual("HorizontalViewSize = 100", element.Patterns[1].Properties[2].NodeValue);
            Assert.AreEqual("VerticallyScrollable = False", element.Patterns[1].Properties[3].NodeValue);
            Assert.AreEqual("VerticalScrollPercent = -1", element.Patterns[1].Properties[4].NodeValue);
            Assert.AreEqual("VerticalViewSize = 100", element.Patterns[1].Properties[5].NodeValue);
            Assert.AreEqual("ExpandCollapseState = 0", element.Patterns[2].Properties[0].NodeValue);
        }
    }
}
