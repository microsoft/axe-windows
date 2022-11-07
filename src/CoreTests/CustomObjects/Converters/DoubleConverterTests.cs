// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.CustomObjects.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.CoreTests.CustomObjects.Converters
{
    [TestClass()]
    public class DoubleConverterTests
    {
        [TestMethod, Timeout(1000)]
        public void RenderTest()
        {
            Assert.AreEqual("0", new DoubleTypeConverter().Render(0.0));
        }

        [TestMethod, Timeout(1000)]
        public void NullRenderTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new DoubleTypeConverter().Render(null));
        }
    }
}
