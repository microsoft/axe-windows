// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.CustomObjects;
using Axe.Windows.Core.CustomObjects.Converters;
using Axe.Windows.Core.Enums;
using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using Interop.UIAutomationCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Axe.Windows.DesktopTests.UIAutomation.CustomObjects
{
    [TestClass]
    public class RegistrarTests
    {
        [TestMethod, Timeout(1000)]
        public void RegisterCustomProperty_CorrectDataFlow()
        {
            Guid testGuid = new Guid("312F7536-259A-47C7-B192-AA16352522C4");
            string testName = "CommentReplyCount";
            CustomProperty testConfig = new CustomProperty { Guid = testGuid, ProgrammaticName = testName, ConfigType = "int" };
            UIAutomationPropertyInfo testInfo = new UIAutomationPropertyInfo { guid = testGuid, pProgrammaticName = testName, type = UIAutomationType.UIAutomationType_Int };
            int testId = 43;
            Mock<IUIAutomationRegistrar> UIARegistrarMock = new Mock<IUIAutomationRegistrar>(MockBehavior.Strict);
            UIARegistrarMock.Setup(x => x.RegisterProperty(ref testInfo, out testId));
            int counter = 0;
            Action<int, ITypeConverter> testCallback = new Action<int, ITypeConverter>((id, converter) =>
            {
                Assert.AreEqual(testId, id);
                Assert.IsInstanceOfType(converter, typeof(IntTypeConverter));
                counter++;
            });

            Registrar r = new Registrar(UIARegistrarMock.Object, testCallback);

            r.RegisterCustomProperty(testConfig);

            Assert.AreEqual(1, counter); // Callback should only be called once
            UIARegistrarMock.VerifyAll();
        }

        [TestMethod, Timeout(1000)]
        public void CreateTypeConverterStringTest()
        {
            Assert.IsInstanceOfType(Registrar.CreateTypeConverter(CustomUIAPropertyType.String), typeof(StringTypeConverter));
        }

        [TestMethod, Timeout(1000)]
        public void CreateTypeConverterIntTest()
        {
            Assert.IsInstanceOfType(Registrar.CreateTypeConverter(CustomUIAPropertyType.Int), typeof(IntTypeConverter));
        }

        [TestMethod, Timeout(1000)]
        public void CreateTypeConverterBoolTest()
        {
            Assert.IsInstanceOfType(Registrar.CreateTypeConverter(CustomUIAPropertyType.Bool), typeof(BoolTypeConverter));
        }

        [TestMethod, Timeout(1000)]
        public void CreateTypeConverterDoubleTest()
        {
            Assert.IsInstanceOfType(Registrar.CreateTypeConverter(CustomUIAPropertyType.Double), typeof(DoubleTypeConverter));
        }

        [TestMethod, Timeout(1000)]
        public void CreateTypeConverterPointTest()
        {
            Assert.IsInstanceOfType(Registrar.CreateTypeConverter(CustomUIAPropertyType.Point), typeof(PointTypeConverter));
        }

        [TestMethod, Timeout(1000)]
        public void CreateTypeConverterElementTest()
        {
            Assert.IsInstanceOfType(Registrar.CreateTypeConverter(CustomUIAPropertyType.Element), typeof(ElementTypeConverter));
        }

        [TestMethod, Timeout(1000)]
        public void CreateTypeConverterUnsetTest()
        {
            Assert.ThrowsException<ArgumentException>(() => Registrar.CreateTypeConverter(CustomUIAPropertyType.Unset));
        }
    }
}
