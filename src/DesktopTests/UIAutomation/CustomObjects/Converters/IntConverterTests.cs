// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.UIAutomation.CustomObjects.Converters;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.DesktopTests.UIAutomation.CustomObjects.Converters
{
    [TestClass()]
    public class IntConverterTests
    {
        [TestMethod, Timeout(1000)]
        public void RenderTest()
        {
            Assert.AreEqual("42", new IntTypeConverter().Render(42));
        }

        [TestMethod, Timeout(1000)]
        public void NullRenderTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new IntTypeConverter().Render(null));
        }
    }
}
