// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.CustomObjects.Converters;
using Axe.Windows.Core.Types;
using Axe.Windows.UnitTestSharedLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.CoreTests.Bases
{
    /// <summary>
    /// Tests A11yProperty class
    /// </summary>
    [TestClass()]
    public class A11yPropertyTests
    {
        /// <summary>
        /// Tests A11yProperty.ToString() through A11yProperty.TextValue.
        /// Also tests constructor.
        /// </summary>
        [TestMethod()]
        public void ToStringTest()
        {
            A11yElement element = Utility.LoadA11yElementsFromJSON("Resources/A11yPropertyTest.hier");
            string kpVal;

            kpVal = element.Properties[PropertyType.UIA_ControlTypePropertyId].ToString();
            Assert.AreEqual("Text(50020)", kpVal);

            kpVal = element.Properties[PropertyType.UIA_RuntimeIdPropertyId].ToString();
            Assert.AreEqual("[7,1F48,24D4850]", kpVal);

            kpVal = element.Properties[PropertyType.UIA_BoundingRectanglePropertyId].ToString();
            Assert.AreEqual("[l=1285,t=91,r=1368,b=116]", kpVal);

            kpVal = element.Properties[PropertyType.UIA_OrientationPropertyId].ToString();
            Assert.AreEqual("None(0)", kpVal);

            kpVal = element.Properties[PropertyType.UIA_LabeledByPropertyId].ToString();
            Assert.AreEqual("Test", kpVal);

            kpVal = element.Properties[PropertyType.UIA_HasKeyboardFocusPropertyId].ToString();
            Assert.AreEqual("False", kpVal);
        }

        [TestMethod()]
        public void SimpleCustomPropertyTest()
        {
            const int testId = 42;
            dynamic testValue = 1;
            const string expectedRendering = "1";
            A11yProperty.RegisterCustomProperty(testId, new IntTypeConverter());
            A11yProperty kp = new A11yProperty(testId, testValue);
            Assert.AreEqual(expectedRendering, kp.ToString());
        }

        [TestMethod()]
        public void RegisterTwiceCustomPropertyTest()
        {
            const int testId = 42;
            dynamic testValue = 1;
            const string expectedRendering = "1";
            A11yProperty.RegisterCustomProperty(testId, new PointTypeConverter()); // First registration, should be dropped.
            A11yProperty.RegisterCustomProperty(testId, new IntTypeConverter()); // Second registration, last one wins.
            A11yProperty kp = new A11yProperty(testId, testValue);
            Assert.AreEqual(expectedRendering, kp.ToString());
        }
    }
}
