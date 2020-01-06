// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.Core.Types.Tests
{
    /// <summary>
    /// For PropertyTypes and PlatformPropertyTypes
    /// </summary>
    [TestClass()]
    public class PropertyTypesTest
    {
        [TestMethod()]
        public void CheckExists()
        {
            Assert.AreEqual(true, PropertyType.GetInstance().Exists(PropertyType.UIA_AnnotationPattern_AnnotationTypeNamePropertyId));
            Assert.AreEqual(false, PropertyType.GetInstance().Exists(0));
        }

        [TestMethod()]
        public void CheckGetNameById()
        {
            Assert.AreEqual("AnnotationPattern.AnnotationTypeName", PropertyType.GetInstance().GetNameById(PropertyType.UIA_AnnotationPattern_AnnotationTypeNamePropertyId));
        }

        [TestMethod()]
        public void CheckExistsPlatform()
        {
            Assert.AreEqual(true, PlatformPropertyType.GetInstance().Exists(PlatformPropertyType.Platform_ProcessNamePropertyId));
            Assert.AreEqual(false, PlatformPropertyType.GetInstance().Exists(0));
        }

        [TestMethod()]
        public void CheckGetNameByIdPlatform()
        {
            Assert.AreEqual("ProcessName", PlatformPropertyType.GetInstance().GetNameById(PlatformPropertyType.Platform_ProcessNamePropertyId));
        }

        [TestMethod()]
        public void HeadingLevelType_HeadingLevelNone()
        {
            Assert.AreEqual("HeadingLevel_None(80050)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevelNone));
        }

        [TestMethod()]
        public void HeadingLevelType_HeadingLevel1()
        {
            Assert.AreEqual("HeadingLevel1(80051)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel1));
        }

        [TestMethod()]
        public void HeadingLevelType_HeadingLevel2()
        {
            Assert.AreEqual("HeadingLevel2(80052)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel2));
        }

        [TestMethod()]
        public void HeadingLevelType_HeadingLevel3()
        {
            Assert.AreEqual("HeadingLevel3(80053)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel3));
        }

        [TestMethod()]
        public void HeadingLevelType_HeadingLevel4()
        {
            Assert.AreEqual("HeadingLevel4(80054)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel4));
        }

        [TestMethod()]
        public void HeadingLevelType_HeadingLevel5()
        {
            Assert.AreEqual("HeadingLevel5(80055)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel5));
        }

        [TestMethod()]
        public void HeadingLevelType_HeadingLevel6()
        {
            Assert.AreEqual("HeadingLevel6(80056)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel6));
        }

        [TestMethod()]
        public void HeadingLevelType_HeadingLevel7()
        {
            Assert.AreEqual("HeadingLevel7(80057)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel7));
        }

        [TestMethod()]
        public void HeadingLevelType_HeadingLevel8()
        {
            Assert.AreEqual("HeadingLevel8(80058)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel8));
        }

        [TestMethod()]
        public void HeadingLevelType_HeadingLevel9()
        {
            Assert.AreEqual("HeadingLevel9(80059)", HeadingLevelType.GetInstance().GetNameById(HeadingLevelType.HeadingLevel9));
        }
    }
}
