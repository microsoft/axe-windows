// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using AxeWindowsCLI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics;

namespace CLITests
{
    [TestClass]
    public class ProcessHelperUnitTests
    {
        const string TestProcessName = "SomeProcess";
        const int TestProcessId = -123;
        const string TestProcessIdAsString = "-123";

        Mock<IProcessAbstraction> _processAbstractionMock;
        Mock<IErrorCollector> _errorCollectorMock;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _processAbstractionMock = new Mock<IProcessAbstraction>(MockBehavior.Strict);
            _errorCollectorMock = new Mock<IErrorCollector>(MockBehavior.Strict);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Ctor_ProcessAbstractionIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(
                () => new ProcessHelper(null, _errorCollectorMock.Object));
            Assert.AreEqual("processAbstraction", e.ParamName);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Ctor_ErrorCollectorIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(
                () => new ProcessHelper(_processAbstractionMock.Object, null));
            Assert.AreEqual("errorCollector", e.ParamName);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Ctor_InputsAreValid_Succeeds()
        {
            ProcessHelper helper = new ProcessHelper(_processAbstractionMock.Object, _errorCollectorMock.Object);
            Assert.IsInstanceOfType(helper, typeof(ProcessHelper));
        }

        [TestMethod]
        [Timeout(1000)]
        public void FindProcessByName_GetProcessesReturnsNull_ReturnsInvalidProcessId()
        {
            string actualErrorMessage = null;
            Process[] processes = null;

            _processAbstractionMock.Setup(x => x.GetProcessesByName(TestProcessName))
                .Returns(processes);
            _errorCollectorMock.Setup(x => x.AddParameterError(It.IsAny<string>()))
                .Callback<string>((s) => actualErrorMessage = s);
            ProcessHelper helper = new ProcessHelper(_processAbstractionMock.Object, _errorCollectorMock.Object);

            int processId = helper.FindProcessByName(TestProcessName);

            Assert.AreEqual(IProcessHelper.InvalidProcessId, processId);
            Assert.AreNotEqual(TestProcessName, actualErrorMessage);
            Assert.IsTrue(actualErrorMessage.EndsWith(TestProcessName));
            _processAbstractionMock.VerifyAll();
            _errorCollectorMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void FindProcessByName_GetProcessesReturnsEmptyArray_ReturnsInvalidProcessId()
        {
            string actualErrorMessage = null;
            Process[] processes = new Process[0];

            _processAbstractionMock.Setup(x => x.GetProcessesByName(TestProcessName))
                .Returns(processes);
            _errorCollectorMock.Setup(x => x.AddParameterError(It.IsAny<string>()))
                .Callback<string>((s) => actualErrorMessage = s);
            ProcessHelper helper = new ProcessHelper(_processAbstractionMock.Object, _errorCollectorMock.Object);

            int processId = helper.FindProcessByName(TestProcessName);

            Assert.AreEqual(IProcessHelper.InvalidProcessId, processId);
            Assert.AreNotEqual(TestProcessName, actualErrorMessage);
            Assert.IsTrue(actualErrorMessage.EndsWith(" " + TestProcessName));
            _processAbstractionMock.VerifyAll();
            _errorCollectorMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void FindProcessByName_GetProcessesReturnsArrayOfTwoElements_ReturnsInvalidProcessId()
        {
            string actualErrorMessage = null;
            Process[] processes = new Process[2];

            _processAbstractionMock.Setup(x => x.GetProcessesByName(TestProcessName))
                .Returns(processes);
            _errorCollectorMock.Setup(x => x.AddParameterError(It.IsAny<string>()))
                .Callback<string>((s) => actualErrorMessage = s);
            ProcessHelper helper = new ProcessHelper(_processAbstractionMock.Object, _errorCollectorMock.Object);

            int processId = helper.FindProcessByName(TestProcessName);

            Assert.AreEqual(IProcessHelper.InvalidProcessId, processId);
            Assert.AreNotEqual(TestProcessName, actualErrorMessage);
            Assert.IsTrue(actualErrorMessage.EndsWith(" " + TestProcessName));
            _processAbstractionMock.VerifyAll();
            _errorCollectorMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void FindProcessByName_GetProcessesReturnsArrayOfOneElement_ReturnsExpectedProcessId()
        {
            Process process = Process.GetCurrentProcess();
            Process[] processes = new Process[] { process };

            _processAbstractionMock.Setup(x => x.GetProcessesByName(TestProcessName))
                .Returns(processes);
            ProcessHelper helper = new ProcessHelper(_processAbstractionMock.Object, _errorCollectorMock.Object);

            int processId = helper.FindProcessByName(TestProcessName);

            Assert.AreEqual(process.Id, processId);
            _processAbstractionMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void FindProcessById_GetProcessByIdReturnsThrowsArgumentException_ReturnsInvalidProcessName()
        {
            string actualErrorMessage = null;

            _processAbstractionMock.Setup(x => x.GetProcessById(TestProcessId))
                .Throws<ArgumentException>();
            _errorCollectorMock.Setup(x => x.AddParameterError(It.IsAny<string>()))
                .Callback<string>((s) => actualErrorMessage = s);
            ProcessHelper helper = new ProcessHelper(_processAbstractionMock.Object, _errorCollectorMock.Object);

            string processName = helper.FindProcessById(TestProcessId);

            Assert.AreEqual(IProcessHelper.InvalidProcessName, processName);
            Assert.AreNotEqual(TestProcessIdAsString, actualErrorMessage);
            Assert.IsTrue(actualErrorMessage.EndsWith(" " + TestProcessIdAsString));
            _processAbstractionMock.VerifyAll();
            _errorCollectorMock.VerifyAll();
        }
    }
}
