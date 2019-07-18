// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.SystemAbstractions;
using Axe.Windows.Win32;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Axe.Windows.Win32Tests
{
    [TestClass]
    public class Win32ApisUnitTests
    {
        private readonly Mock<IMicrosoftWin32Registry> _registryMock;
        private readonly Win32Helper _win32Helper;

        public Win32ApisUnitTests()
        {
            _registryMock = new Mock<IMicrosoftWin32Registry>(MockBehavior.Strict);
                _win32Helper = new Win32Helper(_registryMock.Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _registryMock.Reset();
        }

        private void SetupRegistryGetValue(params object[] returnValues)
        {
            var setup = _registryMock.SetupSequence(x => x.GetValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()));

            foreach (var value in returnValues)
                setup.Returns(value);
        }

        private void VerifyRegistryCalls(int callCount)
        {
            _registryMock.Verify((x => x.GetValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>())), Times.Exactly(callCount));
        }

        [TestMethod]
        public void IsWindowsRS3OrLater_OsValueIsMissing_ReturnsFalse()
        {
            SetupRegistryGetValue();

            Assert.IsFalse(_win32Helper.IsWindowsRS3OrLater());

            VerifyRegistryCalls(1);
        }

        [TestMethod]
        public void IsWindowsRS3OrLater_OsValueIsBadFormat_ReturnsFalse()
        {
            SetupRegistryGetValue("6.3 alpha");

            Assert.IsFalse(_win32Helper.IsWindowsRS3OrLater());

            VerifyRegistryCalls(1);
        }

        [TestMethod]
        public void IsWindowsRS3OrLater_OsValueIsLessThanWindows10_ReturnsFalse()
        {
            SetupRegistryGetValue("6.2");

            Assert.IsFalse(_win32Helper.IsWindowsRS3OrLater());

            VerifyRegistryCalls(1);
        }

        [TestMethod]
        public void IsWindowsRS3OrLater_OsValueIsGreaterThanWindows10_ReturnsTrue()
        {
            SetupRegistryGetValue("6.4");

            Assert.IsTrue(_win32Helper.IsWindowsRS3OrLater());

            VerifyRegistryCalls(1);
        }

        [TestMethod]
        public void IsWindowsRS3OrLater_BuildValueIsMissing_ReturnsFalse()
        {
            SetupRegistryGetValue("6.3");

            Assert.IsFalse(_win32Helper.IsWindowsRS3OrLater());

            VerifyRegistryCalls(2);
        }

        [TestMethod]
        public void IsWindowsRS3OrLater_BuildValueIsBadFormat_ReturnsFalse()
        {
            SetupRegistryGetValue("6.3", "16227-rc1");

            Assert.IsFalse(_win32Helper.IsWindowsRS3OrLater());

            VerifyRegistryCalls(2);
        }

        [TestMethod]
        public void IsWindowsRS3OrLater_BuildValueIsLessThanTarget_ReturnsFalse()
        {
            SetupRegistryGetValue("6.3", "16227");

            Assert.IsFalse(_win32Helper.IsWindowsRS3OrLater());

            VerifyRegistryCalls(2);
        }

        [TestMethod]
        public void IsWindowsRS3OrLater_BuildValueIsEqualToTarget_ReturnsTrue()
        {
            SetupRegistryGetValue("6.3", "16228");

            Assert.IsTrue(_win32Helper.IsWindowsRS3OrLater());

            VerifyRegistryCalls(2);
        }

        [TestMethod]
        public void IsWindowsRS3OrLater_BuildValueIsGreaterThanTarget_ReturnsTrue()
        {
            SetupRegistryGetValue("6.3", "16229");

            Assert.IsTrue(_win32Helper.IsWindowsRS3OrLater());

            VerifyRegistryCalls(2);
        }

        [TestMethod]
        public void IsWindowsRS5OrLater_OsValueIsMissingReturnsFalse()
        {
            SetupRegistryGetValue();

            Assert.IsFalse(_win32Helper.IsWindowsRS5OrLater());

            VerifyRegistryCalls(1);
        }

        [TestMethod]
        public void IsWindowsRS5OrLater_OsValueIsBadFormat_ReturnsFalse()
        {
            SetupRegistryGetValue("6.3 alpha");

            Assert.IsFalse(_win32Helper.IsWindowsRS5OrLater());

            VerifyRegistryCalls(1);
        }

        [TestMethod]
        public void IsWindowsRS5OrLater_OsValueIsLessThanWindows10_ReturnsFalse()
        {
            SetupRegistryGetValue("6.2");

            Assert.IsFalse(_win32Helper.IsWindowsRS5OrLater());

            VerifyRegistryCalls(1);
        }

        [TestMethod]
        public void IsWindowsRS5OrLater_OsValueIsGreaterThanWindows10_ReturnsTrue()
        {
            SetupRegistryGetValue("6.4");

            Assert.IsTrue(_win32Helper.IsWindowsRS5OrLater());

            VerifyRegistryCalls(1);
        }

        [TestMethod]
        public void IsWindowsRS5OrLater_BuildValueIsMissing_ReturnsFalse()
        {
            SetupRegistryGetValue("6.3");

            Assert.IsFalse(_win32Helper.IsWindowsRS5OrLater());

            VerifyRegistryCalls(2);
        }

        [TestMethod]
        public void IsWindowsRS5OrLater_BuildValueIsBadFormat_ReturnsFalse()
        {
            SetupRegistryGetValue("6.3", "17713-rc1");

            Assert.IsFalse(_win32Helper.IsWindowsRS5OrLater());

            VerifyRegistryCalls(2);
        }

        [TestMethod]
        public void IsWindowsRS5OrLater_BuildValueIsLessThanTarget_ReturnsFalse()
        {
            SetupRegistryGetValue("6.3", "17712");

            Assert.IsFalse(_win32Helper.IsWindowsRS5OrLater());

            VerifyRegistryCalls(2);
        }

        [TestMethod]
        public void IsWindowsRS5OrLater_BuildValueIsEqualToTarget_ReturnsTrue()
        {
            SetupRegistryGetValue("6.3", "17713");

            Assert.IsTrue(_win32Helper.IsWindowsRS5OrLater());

            VerifyRegistryCalls(2);
        }

        [TestMethod]
        public void IsWindowsRS5OrLater_BuildValueIsGreaterThanTarget_ReturnsTrue()
        {
            SetupRegistryGetValue("6.3", "17714");

            Assert.IsTrue(_win32Helper.IsWindowsRS5OrLater());

            VerifyRegistryCalls(2);
        }
    }
}
