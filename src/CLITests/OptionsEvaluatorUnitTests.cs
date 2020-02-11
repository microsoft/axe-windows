// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using AxeWindowsCLI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace CLITests
{
    [TestClass]
    public class OptionEvaluatorUnitTests
    {
        const string TestProcessName = "MyProcess";
        const int TestProcessId = 42;

        private Mock<IProcessHelper> _processHelperMock;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _processHelperMock = new Mock<IProcessHelper>(MockBehavior.Strict);
        }

        private void VerifyAllMocks()
        {
            _processHelperMock.VerifyAll();
        }

        private void ValidateOptions(IOptions options, string processName = TestProcessName,
            int processId = TestProcessId, string outputDirectory = null, string scanId = null,
            VerbosityLevel verbosityLevel = VerbosityLevel.Default)
        {
            Assert.AreEqual(processName, options.ProcessName);
            Assert.AreEqual(processId, options.ProcessId);
            Assert.AreEqual(scanId, options.ScanId);
            Assert.AreEqual(outputDirectory, options.OutputDirectory);
            Assert.AreEqual(verbosityLevel, options.VerbosityLevel);
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessInputs_RawInputsIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(() => OptionsEvaluator.ProcessInputs(
                null, _processHelperMock.Object));
            Assert.AreEqual("rawInputs", e.ParamName);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessInputs_ProcessHelperIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(() => OptionsEvaluator.ProcessInputs(
                new Options(), null));
            Assert.AreEqual("processHelper", e.ParamName);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessInputs_DefaultValues_ThrowsParameterException()
        {
            Options input = new Options();
            ParameterException e = Assert.ThrowsException<ParameterException>(() =>
                OptionsEvaluator.ProcessInputs(input, _processHelperMock.Object));
            Assert.AreEqual("Please specify either processId or processName on the command line", e.Message);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessInputs_ProcessIdIsSpecified_FindsProcessById()
        {
            _processHelperMock.Setup(x => x.ProcessNameFromId(TestProcessId)).Returns(TestProcessName);
            Options input = new Options
            {
                ProcessId = TestProcessId,
            };
            ValidateOptions(OptionsEvaluator.ProcessInputs(input, _processHelperMock.Object),
                processId: TestProcessId);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessInputs_ProcessNameIsSpecifiedWithNoExtension_FindsProcessByName()
        {
            const string fullProcessName = @"c:\foo\bar\myprocess";
            const string reducedProcessName = "myprocess";
            _processHelperMock.Setup(x => x.ProcessIdFromName(reducedProcessName)).Returns(TestProcessId);
            Options input = new Options
            {
                ProcessName = fullProcessName,
            };
            ValidateOptions(OptionsEvaluator.ProcessInputs(input, _processHelperMock.Object),
                processId: TestProcessId, processName: reducedProcessName);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessInputs_ProcessNameIsSpecifiedWithExeExtension_FindsProcessByName()
        {
            const string fullProcessName = @"c:\foo\bar\myprocess.exe";
            const string reducedProcessName = "myprocess";
            _processHelperMock.Setup(x => x.ProcessIdFromName(reducedProcessName)).Returns(TestProcessId);
            Options input = new Options
            {
                ProcessName = fullProcessName,
            };
            ValidateOptions(OptionsEvaluator.ProcessInputs(input, _processHelperMock.Object),
                processId: TestProcessId, processName: reducedProcessName);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessInputs_ProcessNameIsSpecifiedWithAppExtension_FindsProcessByName()
        {
            const string fullProcessName = @"c:\foo\bar\myprocess.app";
            const string reducedProcessName = "myprocess.app";
            _processHelperMock.Setup(x => x.ProcessIdFromName(reducedProcessName)).Returns(TestProcessId);
            Options input = new Options
            {
                ProcessName = fullProcessName,
            };
            ValidateOptions(OptionsEvaluator.ProcessInputs(input, _processHelperMock.Object),
                processId: TestProcessId, processName: reducedProcessName);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessInputs_SpecifiesOutputDirectory_RetainsOutputDirectory()
        {
            const string testOutputDirectory = @"C:\Test\Output\Directory";

            _processHelperMock.Setup(x => x.ProcessIdFromName(TestProcessName)).Returns(TestProcessId);
            Options input = new Options
            {
                ProcessName = TestProcessName,
                OutputDirectory = testOutputDirectory,
            };
            ValidateOptions(OptionsEvaluator.ProcessInputs(input, _processHelperMock.Object),
                processId: TestProcessId, outputDirectory: testOutputDirectory);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessInputs_SpecifiesScanId_RetainsOutputDirectory()
        {
            const string testScanId = "SuperScan";
            _processHelperMock.Setup(x => x.ProcessIdFromName(TestProcessName)).Returns(TestProcessId);
            Options input = new Options
            {
                ProcessName = TestProcessName,
                ScanId = testScanId,
            };
            ValidateOptions(OptionsEvaluator.ProcessInputs(input, _processHelperMock.Object),
                processId: TestProcessId, scanId: testScanId);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessInputs_SpecifiesInvalidVerbosity_ThrowsParameterException()
        {
            const string verbosity = "Not_A_Valid_Value";
            _processHelperMock.Setup(x => x.ProcessIdFromName(TestProcessName)).Returns(TestProcessId);
            Options input = new Options
            {
                ProcessName = TestProcessName,
                Verbosity = verbosity,
            };
            ParameterException e = Assert.ThrowsException<ParameterException>(() =>
                OptionsEvaluator.ProcessInputs(input, _processHelperMock.Object));
            Assert.AreEqual("Invalid verbosity level: Not_A_Valid_Value", e.Message);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessInputs_SpecifiesQuiet_SetsQuietVerbosity()
        {
            const string verbosity = "quiet";
            _processHelperMock.Setup(x => x.ProcessIdFromName(TestProcessName)).Returns(TestProcessId);
            Options input = new Options
            {
                ProcessName = TestProcessName,
                Verbosity = verbosity,
            };
            ValidateOptions(OptionsEvaluator.ProcessInputs(input, _processHelperMock.Object),
                processId: TestProcessId, verbosityLevel: VerbosityLevel.Quiet);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessInputs_SpecifiesDefault_SetsDefaultVerbosity()
        {
            const string verbosity = "dEfAuLt";
            _processHelperMock.Setup(x => x.ProcessIdFromName(TestProcessName)).Returns(TestProcessId);
            Options input = new Options
            {
                ProcessName = TestProcessName,
                Verbosity = verbosity,
            };
            ValidateOptions(OptionsEvaluator.ProcessInputs(input, _processHelperMock.Object),
                processId: TestProcessId);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ProcessInputs_SpecifiesVerbose_SetsQuietVerbosity()
        {
            const string verbosity = "VERBOSE";
            _processHelperMock.Setup(x => x.ProcessIdFromName(TestProcessName)).Returns(TestProcessId);
            Options input = new Options
            {
                ProcessName = TestProcessName,
                Verbosity = verbosity,
            };
            ValidateOptions(OptionsEvaluator.ProcessInputs(input, _processHelperMock.Object),
                processId: TestProcessId, verbosityLevel: VerbosityLevel.Verbose);
            VerifyAllMocks();
        }
    }
}
