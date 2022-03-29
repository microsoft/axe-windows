// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.CustomObjects.Converters;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.CoreTests.CustomObjects.Converters
{
    [TestClass()]
    public class PointConverterTests
    {
        [TestMethod, Timeout(1000)]
        public void RenderTest()
        {
            dynamic value = new double[2] { 42.0, 42.0 };
            Assert.AreEqual("[x=42,y=42]", new PointTypeConverter().Render(value));
        }

        [TestMethod, Timeout(1000)]
        public void NullRenderTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new PointTypeConverter().Render(null));
        }
    }
}
