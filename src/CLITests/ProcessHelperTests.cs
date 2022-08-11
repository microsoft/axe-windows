// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using AxeWindowsCLI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics;

namespace AxeWindowsCLITests
{
    [TestClass]
    public class ProcessHelperTests
    {
        const string TestProcessName = "SomeProcess";
        const int TestProcessId = -123;

        Mock<IProcessAbstraction> _processAbstractionMock;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _processAbstractionMock = new Mock<IProcessAbstraction>(MockBehavior.Strict);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Ctor_ProcessAbstractionIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(
                () => new ProcessHelper(null));
            Assert.AreEqual("processAbstraction", e.ParamName);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Ctor_InputsAreValid_Succeeds()
        {
            ProcessHelper helper = new ProcessHelper(_processAbstractionMock.Object);
            Assert.IsInstanceOfType(helper, typeof(ProcessHelper));
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessIdFromName_GetProcessesThrowsException_ThrowsParameterException()
        {
            _processAbstractionMock.Setup(x => x.GetProcessesByName(TestProcessName))
                .Throws<ArgumentException>();
            ProcessHelper helper = new ProcessHelper(_processAbstractionMock.Object);

            ParameterException e = Assert.ThrowsException<ParameterException>(
                () => helper.ProcessIdFromName(TestProcessName));

            Assert.AreEqual("Unable to find process with name SomeProcess", e.Message);
            _processAbstractionMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessIdFromName_GetProcessesReturnsNull_ThrowsParameterException()
        {
            Process[] processes = null;

            _processAbstractionMock.Setup(x => x.GetProcessesByName(TestProcessName))
                .Returns(processes);
            ProcessHelper helper = new ProcessHelper(_processAbstractionMock.Object);

            ParameterException e = Assert.ThrowsException<ParameterException>(
                () => helper.ProcessIdFromName(TestProcessName));

            Assert.AreEqual("Unable to find process with name SomeProcess", e.Message);
            _processAbstractionMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessIdFromName_GetProcessesReturnsEmptyArray_ThrowsParameterException()
        {
            Process[] processes = new Process[0];

            _processAbstractionMock.Setup(x => x.GetProcessesByName(TestProcessName))
                .Returns(processes);
            ProcessHelper helper = new ProcessHelper(_processAbstractionMock.Object);

            ParameterException e = Assert.ThrowsException<ParameterException>(
                () => helper.ProcessIdFromName(TestProcessName));

            Assert.AreEqual("Unable to find process with name SomeProcess", e.Message);
            _processAbstractionMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessIdFromName_GetProcessesReturnsArrayOfTwoElements_ThrowsParameterException()
        {
            Process[] processes = new Process[2];

            _processAbstractionMock.Setup(x => x.GetProcessesByName(TestProcessName))
                .Returns(processes);
            ProcessHelper helper = new ProcessHelper(_processAbstractionMock.Object);

            ParameterException e = Assert.ThrowsException<ParameterException>(
                () => helper.ProcessIdFromName(TestProcessName));

            Assert.AreEqual("Found multiple processes with name SomeProcess", e.Message);
            _processAbstractionMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessIdFromName_GetProcessesReturnsArrayOfOneElement_ReturnsExpectedProcessId()
        {
            Process process = Process.GetCurrentProcess();
            Process[] processes = new Process[] { process };

            _processAbstractionMock.Setup(x => x.GetProcessesByName(TestProcessName))
                .Returns(processes);
            ProcessHelper helper = new ProcessHelper(_processAbstractionMock.Object);

            int processId = helper.ProcessIdFromName(TestProcessName);

            Assert.AreEqual(process.Id, processId);
            _processAbstractionMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessNameFromId_GetProcessByIdReturnsThrowsException_ThrowsParamterException()
        {
            Exception expectedException = new MissingMethodException();
            _processAbstractionMock.Setup(x => x.GetProcessById(TestProcessId))
                .Throws(expectedException);
            ProcessHelper helper = new ProcessHelper(_processAbstractionMock.Object);

            ParameterException e = Assert.ThrowsException<ParameterException>(
                () => helper.ProcessNameFromId(TestProcessId));

            Assert.AreEqual("Unable to find process with id -123", e.Message);
            Assert.AreEqual(expectedException, e.InnerException);
            _processAbstractionMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessNameFromId_GetProcessByIdReturnsProcess_ReturnsName()
        {
            Process currentProcess = Process.GetCurrentProcess();
            Exception expectedException = new MissingMethodException();
            _processAbstractionMock.Setup(x => x.GetProcessById(TestProcessId))
                .Returns(currentProcess);
            ProcessHelper helper = new ProcessHelper(_processAbstractionMock.Object);

            var processName = helper.ProcessNameFromId(TestProcessId);

            Assert.AreEqual(currentProcess.ProcessName, processName);
            _processAbstractionMock.VerifyAll();
        }
    }
}
