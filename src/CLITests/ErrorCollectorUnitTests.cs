// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using AxeWindowsScanner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CLITests
{
    [TestClass]
    public class ErrorCollectorUnitTests
    {
        const string TrivialValue = "   \n   \n\t\n";
        const string ValidValue = "Some\n Valid\n Value";
        static readonly string[] ValidValueArray = { "Value 1", "Value 2", "Value 3" };

        IErrorCollector _errorCollector;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _errorCollector = new ErrorCollector();
        }

        private void ValidateListSizes(int expectedParameterErrors = 0, int expectedScanErrors = 0,
            int expectedExceptions = 0)
        {
            Assert.AreEqual(expectedParameterErrors, _errorCollector.ParameterErrors.Count);
            Assert.AreEqual(expectedScanErrors, _errorCollector.ScanErrors.Count);
            Assert.AreEqual(expectedExceptions, _errorCollector.Exceptions.Count);
        }

        [TestMethod]
        [Timeout(1000)]
        public void ValidInitialListSizes()
        {
            ValidateListSizes();
        }

        [TestMethod]
        [Timeout(1000)]
        public void AddParameterError_ValueIsTrivial_ThrowsArgumentException()
        {
            ArgumentException e = Assert.ThrowsException<ArgumentException>(
                () => _errorCollector.AddParameterError(TrivialValue));
            Assert.AreEqual("error", e.ParamName);
        }

        [TestMethod]
        [Timeout(1000)]
        public void AddParameterError_ValueIsValid_StoresValueCorrectly()
        {
            _errorCollector.AddParameterError(ValidValue);
            Assert.AreEqual(ValidValue, _errorCollector.ParameterErrors[0]);
            ValidateListSizes(expectedParameterErrors: 1);
        }


        [TestMethod]
        [Timeout(1000)]
        public void AddParameterError_MutipleValidValues_StoresValuesCorrectly()
        {
            foreach (string value in ValidValueArray)
            {
                _errorCollector.AddParameterError(value);
            }

            for (int loop = 0; loop < ValidValueArray.Length; loop++)
            {
                Assert.AreEqual(ValidValueArray[loop], _errorCollector.ParameterErrors[loop]);
            }
            ValidateListSizes(expectedParameterErrors: ValidValueArray.Length);
        }

        [TestMethod]
        [Timeout(1000)]
        public void AddScanError_ValueIsTrivial_ThrowsArgumentException()
        {
            ArgumentException e = Assert.ThrowsException<ArgumentException>(
                () => _errorCollector.AddScanError(TrivialValue));
            Assert.AreEqual("error", e.ParamName);
        }

        [TestMethod]
        [Timeout(1000)]
        public void AddScanError_ValueIsValid_StoresValueCorrectly()
        {
            _errorCollector.AddScanError(ValidValue);
            Assert.AreEqual(ValidValue, _errorCollector.ScanErrors[0]);
            ValidateListSizes(expectedScanErrors: 1);
        }


        [TestMethod]
        [Timeout(1000)]
        public void AddScanError_MutipleValidValues_StoresValuesCorrectly()
        {
            foreach (string value in ValidValueArray)
            {
                _errorCollector.AddScanError(value);
            }

            for (int loop = 0; loop < ValidValueArray.Length; loop++)
            {
                Assert.AreEqual(ValidValueArray[loop], _errorCollector.ScanErrors[loop]);
            }
            ValidateListSizes(expectedScanErrors: ValidValueArray.Length);
        }

        [TestMethod]
        [Timeout(1000)]
        public void AddException_ValueIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(
                () => _errorCollector.AddException(null));
            Assert.AreEqual("exception", e.ParamName);
        }

        [TestMethod]
        [Timeout(1000)]
        public void AddException_ValueIsValid_StoresValueCorrectly()
        {
            Exception expectedException = new InvalidCastException();
            _errorCollector.AddException(expectedException);
            Assert.AreEqual(expectedException, _errorCollector.Exceptions[0]);
            ValidateListSizes(expectedExceptions: 1);
        }


        [TestMethod]
        [Timeout(1000)]
        public void AddException_MutipleValidValues_StoresValuesCorrectly()
        {
            Exception[] expectedExceptions =
            {
                new InvalidOperationException(),
                new MissingMethodException(),
                new FormatException()
            };

            foreach (Exception exception in expectedExceptions)
            {
                _errorCollector.AddException(exception);
            }

            for (int loop = 0; loop < expectedExceptions.Length; loop++)
            {
                Assert.AreEqual(expectedExceptions[loop], _errorCollector.Exceptions[loop]);
            }
            ValidateListSizes(expectedExceptions: expectedExceptions.Length);
        }
    }
}
