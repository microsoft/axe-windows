// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.SystemAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.SystemAbstractionsTests
{
    using Path = System.IO.Path;

    [TestClass]
    public class SystemUnitTests
    {
        private readonly ISystem _system = SystemFactory.CreateSystem();

        [TestMethod]
        [Timeout(1000)]
        public void DateTime_Matches()
        {
            var now1 = _system.DateTime.Now;
            var now2 = System.DateTime.Now;

            var timeSpan = now2.Subtract(now1);

            Assert.IsTrue(timeSpan.TotalSeconds <= 1);
        }

        [TestMethod]
        [Timeout(1000)]
        public void CurrentDirectory_Matches()
        {
            Assert.AreEqual(System.Environment.CurrentDirectory, _system.Environment.CurrentDirectory);
        }

        [TestMethod]
        [Timeout(1000)]
        public void DirectoryExists_ReturnsExpected()
        {
            Assert.IsTrue(_system.IO.Directory.Exists(System.Environment.CurrentDirectory));
            Assert.IsFalse(_system.IO.Directory.Exists(@"c:\no way this directory exists"));
        }

        [TestMethod]
        [Timeout(1000)]
        public void CreateDirectory_CreatesDirectory()
        {
            var dirPath = Path.Combine(System.Environment.CurrentDirectory, @"No way this directory exists");
            Assert.IsFalse(System.IO.Directory.Exists(dirPath));

            var dirInfo = _system.IO.Directory.CreateDirectory(dirPath);

            Assert.IsTrue(System.IO.Directory.Exists(dirPath));
            Assert.AreEqual(dirPath, dirInfo.FullName);

            System.IO.Directory.Delete(dirPath);
        }
    } // class
} // namespace
