// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.UIAutomation.CustomObjects.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.DesktopTests.UIAutomation.CustomObjects.Converters
{
    [TestClass()]
    public class BoolConverterTests
    {
        [TestMethod, Timeout(1000)]
        public void TrueRenderTest()
        {
            Assert.AreEqual("True", new BoolTypeConverter().Render(true));
        }

        [TestMethod, Timeout(1000)]
        public void FalseRenderTest()
        {
            Assert.AreEqual("False", new BoolTypeConverter().Render(false));
        }

        [TestMethod, Timeout(1000)]
        public void NullRenderTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new BoolTypeConverter().Render(null));
        }
    }
}
