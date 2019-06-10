// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Axe.Windows.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.AutomationTests
{
    [TestClass]
    public class ExecutionWrapperUnitTests
    {
        private class TestResult
        {
            public string Detail { get; }

            public bool IsError { get; }

            public TestResult(string detail, bool isError = false)
            {
                Detail = detail;
                IsError = isError;
            }
        }

        const string TestString = "He's dead, Jim!";

        [TestMethod]
        [Timeout (5000)]
        public void ExecuteCommand_CommandIsNull_ThrowsInnerNullReferenceException_Automation003InMessage()
        {
            try
            {
                ExecutionWrapper.ExecuteCommand<TestResult>(null);
            }
            catch (AxeWindowsAutomationException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(NullReferenceException));
                Assert.IsTrue(ex.Message.Contains(" Automation003:"));
            }
        }

        [TestMethod]
        [Timeout (1000)]
        public void ExecuteCommand_CommandThrowsNonAutomationException_WrapsInAutomationException_Automation003InMessage()
        {
            try
            {
                ExecutionWrapper.ExecuteCommand<TestResult>(
                    () =>
                    {
                        throw new ArgumentException(TestString);
                    });
            }
            catch (AxeWindowsAutomationException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException));
                Assert.IsTrue(ex.Message.Contains(" Automation003:"));
                Assert.IsTrue(ex.Message.Contains(TestString));
            }
        }

        [TestMethod]
        [Timeout (1000)]
        public void ExecuteCommand_CommandThrowsAutomationException_TestStringInMessage()
        {
            try
            {
                ExecutionWrapper.ExecuteCommand<TestResult>(
                () =>
                {
                    throw new AxeWindowsAutomationException(TestString);
                });
            }
            catch (AxeWindowsAutomationException ex)
            {
                Assert.AreEqual(TestString, ex.Message);
            }
        }

        [TestMethod]
        [Timeout (1000)]
        public void ExecuteCommand_CommandReturnsObject_SameObjectIsReturnedToCaller()
        {
            TestResult result = ExecutionWrapper.ExecuteCommand<TestResult>(
                () => new TestResult(TestString));

            Assert.AreEqual(TestString, result.Detail);
        }
    }
}
