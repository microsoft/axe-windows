// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.CustomObjects;
using Axe.Windows.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.CoreTests.CustomObjects
{
    [TestClass()]
    public class ConfigTests
    {
        [TestMethod, Timeout(1000)]
        public void SimpleConfigTest()
        {
            const string SimpleConfig = "{\"properties\": [{\"guid\": \"4BB56516-F354-44CF-A5AA-96B52E968CFD\", \"programmaticName\": \"AreGridlinesVisible\", \"uiaType\": \"bool\"}]}";
            Config conf = Config.ReadFromText(SimpleConfig);
            Assert.AreEqual(1, conf.Properties.Length);
            CustomProperty prop = conf.Properties[0];
            Assert.AreEqual(new Guid("4BB56516-F354-44CF-A5AA-96B52E968CFD"), prop.Guid);
            Assert.AreEqual("AreGridlinesVisible", prop.ProgrammaticName);
            Assert.AreEqual("bool", prop.ConfigType);
            Assert.AreEqual(CustomUIAPropertyType.Bool, prop.Type);
        }

        [TestMethod, Timeout(1000)]
        public void EnumConfigTest()
        {
            const string EnumConfig = "{\"properties\": [{\"guid\": \"F065BAA7-2794-48B6-A927-193DA1540B84\", \"programmaticName\": \"ViewType\", \"uiaType\": \"enum\", \"values\": {\"1\": \"ViewSlide\"}}]}";
            Config conf = Config.ReadFromText(EnumConfig);
            Assert.AreEqual(1, conf.Properties.Length);
            CustomProperty prop = conf.Properties[0];
            Assert.AreEqual(new Guid("F065BAA7-2794-48B6-A927-193DA1540B84"), prop.Guid);
            Assert.AreEqual("ViewType", prop.ProgrammaticName);
            Assert.AreEqual("enum", prop.ConfigType);
            Assert.AreEqual(CustomUIAPropertyType.Enum, prop.Type);
            Assert.AreEqual("ViewSlide", prop.Values[1]);
        }

        [TestMethod, Timeout(1000)]
        public void NullConfigTest()
        {
            try
            {
                Config.ReadFromText(null);
                Assert.Fail("Failed to throw exception.");
            }
            catch (Exception) { }
        }

        [TestMethod, Timeout(1000)]
        public void EmptyStringConfigTest()
        {
            try
            {
                Config.ReadFromText(string.Empty);
                Assert.Fail("Failed to throw exception.");
            }
            catch (Exception) { }
        }

        [TestMethod, Timeout(1000)]
        public void BadGuidConfigTest()
        {
            const string BadGuidConfig = "{\"properties\": [{\"guid\": \"1999-10-22\", \"programmaticName\": \"AreGridlinesVisible\", \"uiaType\": \"bool\"}]}";
            try
            {
                Config.ReadFromText(BadGuidConfig);
                Assert.Fail("Failed to throw exception.");
            }
            catch (Exception) { }
        }
    }
}
