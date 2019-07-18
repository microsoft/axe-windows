// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.SystemAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Moq;

namespace SystemAbstractionsTests
{
[TestClass]
    public class MicrosoftUnitTests
    {
        const string WindowsVersionRegKey = @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion";

        private readonly IMicrosoft _microsoft = MicrosoftFactory.CreateMicrosoft();

        [TestMethod]
        [Timeout(1000)]
        public void RegistryGetValue_Matches()
        {
            var expectedValue = Registry.GetValue(WindowsVersionRegKey, "CurrentVersion", "");
            var actualValue = _microsoft.Win32.Registry.GetValue(WindowsVersionRegKey, "CurrentVersion", "");

            Assert.AreEqual(expectedValue, actualValue);
        }
    } // class
} // namespace
