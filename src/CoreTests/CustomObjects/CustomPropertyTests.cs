// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.CustomObjects;
using Axe.Windows.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.CoreTests.CustomObjects
{
    [TestClass()]
    public class CustomPropertyTests
    {
        [TestMethod, Timeout(1000)]
        public void DefaultsPropertyTest()
        {
            CustomProperty prop = new CustomProperty();
            Assert.AreEqual(Guid.Empty, prop.Guid);
            Assert.AreEqual(null, prop.ProgrammaticName);
            Assert.AreEqual(null, prop.ConfigType);
            Assert.AreEqual(CustomUIAPropertyType.Unset, prop.Type);
            Assert.AreEqual(null, prop.Values);
        }

        [TestMethod, Timeout(1000)]
        public void BadTypePropertyTest()
        {
            CustomProperty prop = new CustomProperty();
            Assert.ThrowsException<ArgumentException>(() => prop.ConfigType = "Excel");
        }

        [TestMethod, Timeout(1000)]
        public void StringPropertyTest()
        {
            CustomProperty prop = new CustomProperty();
            prop.ConfigType = "string";
            Assert.AreEqual(CustomUIAPropertyType.String, prop.Type);
        }

        [TestMethod, Timeout(1000)]
        public void IntPropertyTest()
        {
            CustomProperty prop = new CustomProperty();
            prop.ConfigType = "int";
            Assert.AreEqual(CustomUIAPropertyType.Int, prop.Type);
        }

        [TestMethod, Timeout(1000)]
        public void BoolPropertyTest()
        {
            CustomProperty prop = new CustomProperty();
            prop.ConfigType = "bool";
            Assert.AreEqual(CustomUIAPropertyType.Bool, prop.Type);
        }

        [TestMethod, Timeout(1000)]
        public void DoublePropertyTest()
        {
            CustomProperty prop = new CustomProperty();
            prop.ConfigType = "double";
            Assert.AreEqual(CustomUIAPropertyType.Double, prop.Type);
        }

        [TestMethod, Timeout(1000)]
        public void PointPropertyTest()
        {
            CustomProperty prop = new CustomProperty();
            prop.ConfigType = "point";
            Assert.AreEqual(CustomUIAPropertyType.Point, prop.Type);
        }

        [TestMethod, Timeout(1000)]
        public void ElementPropertyTest()
        {
            CustomProperty prop = new CustomProperty();
            prop.ConfigType = "element";
            Assert.AreEqual(CustomUIAPropertyType.Element, prop.Type);
        }

        [TestMethod, Timeout(1000)]
        public void EnumPropertyTest()
        {
            CustomProperty prop = new CustomProperty();
            prop.ConfigType = "enum";
            Assert.AreEqual(CustomUIAPropertyType.Enum, prop.Type);
        }
    }
}
