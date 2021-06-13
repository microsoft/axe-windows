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
                Config.ReadFromText(String.Empty);
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

        [TestMethod, Timeout(1000)]
        public void BadTypeConfigTest()
        {
            const string BadTypeConfig = "{\"properties\": [{\"guid\": \"4BB56516-F354-44CF-A5AA-96B52E968CFD\", \"programmaticName\": \"AreGridlinesVisible\", \"uiaType\": \"Excel\"}]}";
            try
            {
                Config.ReadFromText(BadTypeConfig);
                Assert.Fail("Failed to throw exception.");
            }
            catch (Exception) { }
        }

        [TestMethod, Timeout(1000)]
        public void MissingGuidConfigTest()
        {
            const string MissingGuidConfig = "{\"properties\": [{\"programmaticName\": \"AreGridlinesVisible\", \"uiaType\": \"bool\"}]}";
            try
            {
                Config.ReadFromText(MissingGuidConfig);
                Assert.Fail("Failed to throw exception.");
            }
            catch (Exception) { }
        }

        [TestMethod, Timeout(1000)]
        public void MissingNameConfigTest()
        {
            const string MissingNameConfig = "{\"properties\": [{\"guid\": \"4BB56516-F354-44CF-A5AA-96B52E968CFD\", \"uiaType\": \"bool\"}]}";
            try
            {
                Config.ReadFromText(MissingNameConfig);
                Assert.Fail("Failed to throw exception.");
            }
            catch (Exception) { }
        }

        [TestMethod, Timeout(1000)]
        public void MissingTypeConfigTest()
        {
            const string MissingTypeConfig = "{\"properties\": [{\"guid\": \"4BB56516-F354-44CF-A5AA-96B52E968CFD\", \"programmaticName\": \"AreGridlinesVisible\"}]}";
            try
            {
                Config.ReadFromText(MissingTypeConfig);
                Assert.Fail("Failed to throw exception.");
            }
            catch (Exception) { }
        }
    }
}
