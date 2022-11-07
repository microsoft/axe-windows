// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.CoreTests.Types
{
    /// <summary>
    /// Tests for HeadingLevelType class
    /// </summary>
    [TestClass()]
    public class HeadingLevelTypeTests
    {
        [TestMethod()]
        public void HeadingLevelNone()
        {
            Assert.AreEqual("HeadingLevel_None (80050)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevelNone));
        }

        [TestMethod()]
        public void HeadingLevel1()
        {
            Assert.AreEqual("HeadingLevel1 (80051)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel1));
        }

        [TestMethod()]
        public void HeadingLevel2()
        {
            Assert.AreEqual("HeadingLevel2 (80052)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel2));
        }

        [TestMethod()]
        public void HeadingLevel3()
        {
            Assert.AreEqual("HeadingLevel3 (80053)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel3));
        }

        [TestMethod()]
        public void HeadingLevel4()
        {
            Assert.AreEqual("HeadingLevel4 (80054)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel4));
        }

        [TestMethod()]
        public void HeadingLevel5()
        {
            Assert.AreEqual("HeadingLevel5 (80055)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel5));
        }

        [TestMethod()]
        public void HeadingLevel6()
        {
            Assert.AreEqual("HeadingLevel6 (80056)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel6));
        }

        [TestMethod()]
        public void HeadingLevel7()
        {
            Assert.AreEqual("HeadingLevel7 (80057)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel7));
        }

        [TestMethod()]
        public void HeadingLevel8()
        {
            Assert.AreEqual("HeadingLevel8 (80058)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel8));
        }

        [TestMethod()]
        public void HeadingLevel9()
        {
            Assert.AreEqual("HeadingLevel9 (80059)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel9));
        }
    }
}
