// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.DesktopTests.UIAutomation.CustomObjects
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
            Assert.AreEqual("bool", prop.DataType);
        }

        [TestMethod, Timeout(1000)]
        public void EmptyConfigTest()
        {
            Assert.ThrowsException<ArgumentException>(() => Config.ReadFromText("{}"));
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
            Assert.ThrowsException<ArgumentException>(() => Config.ReadFromText(BadTypeConfig));
        }

        [TestMethod, Timeout(1000)]
        public void MissingGuidConfigTest()
        {
            const string MissingGuidConfig = "{\"properties\": [{\"programmaticName\": \"AreGridlinesVisible\", \"uiaType\": \"bool\"}]}";
            Assert.ThrowsException<ArgumentException>(() => Config.ReadFromText(MissingGuidConfig));
        }

        [TestMethod, Timeout(1000)]
        public void MissingNameConfigTest()
        {
            const string MissingNameConfig = "{\"properties\": [{\"guid\": \"4BB56516-F354-44CF-A5AA-96B52E968CFD\", \"uiaType\": \"bool\"}]}";
            Assert.ThrowsException<ArgumentException>(() => Config.ReadFromText(MissingNameConfig));
        }

        [TestMethod, Timeout(1000)]
        public void MissingTypeConfigTest()
        {
            const string MissingTypeConfig = "{\"properties\": [{\"guid\": \"4BB56516-F354-44CF-A5AA-96B52E968CFD\", \"programmaticName\": \"AreGridlinesVisible\"}]}";
            Assert.ThrowsException<ArgumentException>(() => Config.ReadFromText(MissingTypeConfig));
        }
    }
}