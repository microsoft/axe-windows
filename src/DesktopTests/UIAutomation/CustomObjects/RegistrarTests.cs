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
            Guid propertyGuid = new Guid("312F7536-259A-47C7-B192-AA16352522C4");
            string propertyName = "CommentReplyCount";
            CustomProperty propertyToRegister = new CustomProperty { Guid = propertyGuid, ProgrammaticName = propertyName, ConfigType = "int" };
            UIAutomationPropertyInfo expectedPropertyInfo = new UIAutomationPropertyInfo { guid = propertyGuid, pProgrammaticName = propertyName, type = UIAutomationType.UIAutomationType_Int };
            int propertyId = 43;
            Mock<IUIAutomationRegistrar> uiaRegistrarMock = new Mock<IUIAutomationRegistrar>(MockBehavior.Strict);
            uiaRegistrarMock.Setup(x => x.RegisterProperty(ref expectedPropertyInfo, out propertyId));
            int counter = 0;
            Action<int, ITypeConverter> testCallback = new Action<int, ITypeConverter>((id, converter) =>
            {
                Assert.AreEqual(propertyId, id);
                Assert.IsInstanceOfType(converter, typeof(IntTypeConverter));
                counter++;
            });

            Registrar r = new Registrar(uiaRegistrarMock.Object, testCallback);

            r.RegisterCustomProperty(propertyToRegister);

            Assert.AreEqual(1, counter); // Callback should only be called once
            Assert.AreEqual(propertyToRegister, r.IdsToCustomProperties[propertyId]);
            uiaRegistrarMock.VerifyAll();
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
