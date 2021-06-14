// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.CustomObjects;
using Axe.Windows.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Axe.Windows.CoreTests.CustomObjects
{
    [TestClass()]
    public class CustomPropertyTests
    {
        [TestMethod, Timeout(1000)]
        public void StringConfigTest()
        {
            CustomProperty prop = new CustomProperty();
            prop.ConfigType = "string";
            Assert.AreEqual(CustomUIAPropertyType.String, prop.Type);
        }

        [TestMethod, Timeout(1000)]
        public void IntConfigTest()
        {
            CustomProperty prop = new CustomProperty();
            prop.ConfigType = "int";
            Assert.AreEqual(CustomUIAPropertyType.Int, prop.Type);
        }

        [TestMethod, Timeout(1000)]
        public void BoolConfigTest()
        {
            CustomProperty prop = new CustomProperty();
            prop.ConfigType = "bool";
            Assert.AreEqual(CustomUIAPropertyType.Bool, prop.Type);
        }

        [TestMethod, Timeout(1000)]
        public void DoubleConfigTest()
        {
            CustomProperty prop = new CustomProperty();
            prop.ConfigType = "double";
            Assert.AreEqual(CustomUIAPropertyType.Double, prop.Type);
        }

        [TestMethod, Timeout(1000)]
        public void PointConfigTest()
        {
            CustomProperty prop = new CustomProperty();
            prop.ConfigType = "point";
            Assert.AreEqual(CustomUIAPropertyType.Point, prop.Type);
        }

        [TestMethod, Timeout(1000)]
        public void ElementConfigTest()
        {
            CustomProperty prop = new CustomProperty();
            prop.ConfigType = "element";
            Assert.AreEqual(CustomUIAPropertyType.Element, prop.Type);
        }
    }
}
