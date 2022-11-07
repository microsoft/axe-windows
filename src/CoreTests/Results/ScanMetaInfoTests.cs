// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Results;
using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Axe.Windows.CoreTests.Results
{
    [TestClass()]
    public class ScanMetaInfoTests
    {
        /// <summary>
        /// Test with PropertyId Set
        /// </summary>
        [TestMethod()]
        public void ScanMetaInfoTest()
        {
            A11yElement e = new A11yElement
            {
                Properties = new Dictionary<int, A11yProperty>
                {
                    { PropertyType.UIA_ControlTypePropertyId, new A11yProperty(PropertyType.UIA_ControlTypePropertyId, ControlType.UIA_ButtonControlTypeId) },
                    { PropertyType.UIA_FrameworkIdPropertyId, new A11yProperty(PropertyType.UIA_FrameworkIdPropertyId, "WPF") }
                }
            };

            var s = new ScanMetaInfo(e, PropertyType.UIA_AccessKeyPropertyId);

            Assert.AreEqual(PropertyType.UIA_AccessKeyPropertyId, s.PropertyId);
            Assert.AreEqual("Button", s.ControlType);
            Assert.AreEqual("WPF", s.UIFramework);
        }

        /// <summary>
        /// Test without PropertyId
        /// </summary>
        [TestMethod()]
        public void ScanMetaInfoTest2()
        {
            A11yElement e = new A11yElement
            {
                Properties = new Dictionary<int, A11yProperty>
                {
                    { PropertyType.UIA_ControlTypePropertyId, new A11yProperty(PropertyType.UIA_ControlTypePropertyId, ControlType.UIA_AppBarControlTypeId) },
                    { PropertyType.UIA_FrameworkIdPropertyId, new A11yProperty(PropertyType.UIA_FrameworkIdPropertyId, "Win32") }
                }
            };

            var s = new ScanMetaInfo(e);

            Assert.AreEqual(0, s.PropertyId);
            Assert.AreEqual("AppBar", s.ControlType);
            Assert.AreEqual("Win32", s.UIFramework);
        }
    }
}
