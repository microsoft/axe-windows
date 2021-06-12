// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.CustomObjects.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.CoreTests.CustomObjects.Converters
{
    [TestClass()]
    public class ElementConverterTests
    {
        [TestMethod, Timeout(1000)]
        public void RenderTest()
        {
            A11yElement elem = new A11yElement();
            elem.Glimpse = "the glimpse";
            Assert.AreEqual("the glimpse", new ElementTypeConverter().Render(elem));
        }

        [TestMethod, Timeout(1000)]
        public void NullRenderTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ElementTypeConverter().Render(null));
        }
    }
}
