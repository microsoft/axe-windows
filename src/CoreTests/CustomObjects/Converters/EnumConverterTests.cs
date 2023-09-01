// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.CustomObjects.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Axe.Windows.CoreTests.CustomObjects.Converters
{
    [TestClass]
    public class EnumConverterTests
    {
        [TestMethod, Timeout(1000)]
        public void RenderTest()
        {
            Dictionary<int, string> vals = new Dictionary<int, string>
            {
                [42] = "success"
            };
            ITypeConverter tc = new EnumTypeConverter(vals);
            Assert.AreEqual("success (42)", tc.Render(42));
            Assert.AreEqual("Unknown (43)", tc.Render(43));
        }

        [TestMethod, Timeout(1000)]
        public void NullRenderTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new EnumTypeConverter(new Dictionary<int, string>()).Render(null));
        }
    }
}
