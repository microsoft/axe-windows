// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace Axe.Windows.CoreTests.Misc
{
    [TestClass]
    public class PackageInfoTests
    {
        [TestMethod]
        public void PackageInfo_ExpectedInformationalVersion()
        {
            var assembly = Assembly.GetAssembly(typeof(PackageInfo));
            var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            Assert.IsNotNull(attribute?.InformationalVersion);
            Assert.AreEqual(PackageInfo.InformationalVersion, attribute.InformationalVersion);
        }
    } // class
} // namespace
