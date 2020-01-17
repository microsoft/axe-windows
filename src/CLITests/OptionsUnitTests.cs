// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using AxeWindowsScanner;
using CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace CLITests
{
    [TestClass]
    public class OptionsUnitTests
    {
        const int ExpectedParseSuccess = 123;
        const int ExpectedParseFailure = 456;
        const int UnexpectedFailure = 789;

        const string ProcessIdKey = "--processid";
        const string ProcessNameKey = "--processname";
        const string VerbosityKey = "--verbosity";
        const string ScanIdKey = "--scanid";

        const string TestProcessName = "MyProcess";
        const int TestProcessId = 42;

        private IErrorCollector _savedErrorCollector;
        private IProcessHelper _savedProcessHelper;
        private Mock<IErrorCollector> _errorCollectorMock;
        private Mock<IProcessHelper> _processHelperMock;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _errorCollectorMock = new Mock<IErrorCollector>(MockBehavior.Strict);
            _processHelperMock = new Mock<IProcessHelper>(MockBehavior.Strict);

            _savedErrorCollector = Options.ErrorCollector;
            _savedProcessHelper = Options.ProcessHelper;

            Options.ErrorCollector = _errorCollectorMock.Object;
            Options.ProcessHelper = _processHelperMock.Object;
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            Options.ErrorCollector = _savedErrorCollector;
            Options.ProcessHelper = _savedProcessHelper;
        }

        private int FailIfCalled(Options options)
        {
            Assert.Fail("This method should never be called");
            return UnexpectedFailure;
        }

        private int FailIfCalled(IEnumerable<Error> errors)
        {
            Assert.Fail("This method should never be called");
            return UnexpectedFailure;
        }

        private int ExpectErrorType(IEnumerable<Error> errors, ErrorType expectedError)
        {
            List<Error> errorList = errors.ToList<Error>();
            Assert.AreEqual(1, errorList.Count);
            Assert.AreEqual(expectedError, errorList[0].Tag);
            return ExpectedParseFailure;
        }

        private int ValidateOptions(Options options, string processName = TestProcessName,
            int processId = TestProcessId, string outputDirectory = null, string scanId = null,
            VerbosityLevel verbosityLevel = VerbosityLevel.Default)
        {
            Assert.AreEqual(processName, options.ProcessName);
            Assert.AreEqual(processId, options.ProcessId);
            Assert.AreEqual(scanId, options.ScanId);
            Assert.AreEqual(outputDirectory, options.OutputDirectory);
            Assert.AreEqual(verbosityLevel, options.VerbosityLevel);
            return ExpectedParseSuccess;
        }

        [TestMethod]
        [Timeout(1000)]
        public void ParseArguments_NoTarget_FailsWithGroupError()
        {
            _processHelperMock.Setup(x => x.FindProcessByName(null))
                .Returns(0);
            string[] args = { };

            int parseResult = Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    FailIfCalled,
                    e => ExpectErrorType(e, ErrorType.MissingGroupOptionError));

            Assert.AreEqual(ExpectedParseFailure, parseResult);
            _processHelperMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ParseArguments_TargetByProcesssName_SucceedsWithExpectedData()
        {
            _processHelperMock.Setup(x => x.FindProcessByName(TestProcessName))
                .Returns(TestProcessId);
            string[] args = { ProcessNameKey, TestProcessName };

            int parseResult = Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (o) => ValidateOptions(o),
                    FailIfCalled);

            Assert.AreEqual(ExpectedParseSuccess, parseResult);
            _processHelperMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ParseArguments_TargetByProcesssId_SucceedsWithExpectedData()
        {
            _processHelperMock.Setup(x => x.FindProcessById(TestProcessId))
                .Returns(TestProcessName);
            string[] args = { ProcessIdKey, TestProcessId.ToString() };

            int parseResult = Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (o) => ValidateOptions(o),
                    FailIfCalled);

            Assert.AreEqual(ExpectedParseSuccess, parseResult);
            _processHelperMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ParseArguments_TargetIsSet_ScanIdIsSet_SucceedsWithCorrectScanId()
        {
            const string expectedScanId = "Scan123";

            _processHelperMock.Setup(x => x.FindProcessById(TestProcessId))
                .Returns(TestProcessName);
            string[] args = { ProcessIdKey, TestProcessId.ToString(), ScanIdKey, expectedScanId };

            int parseResult = Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (o) => ValidateOptions(o, scanId: expectedScanId),
                    FailIfCalled);

            Assert.AreEqual(ExpectedParseSuccess, parseResult);
            _processHelperMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ParseArguments_TargetIsSet_VerbosityIsQuiet_SucceedsWithQuietVerbosity()
        {
            const string verbosityArg = "quiet";

            _processHelperMock.Setup(x => x.FindProcessById(TestProcessId))
                .Returns(TestProcessName);
            string[] args = { ProcessIdKey, TestProcessId.ToString(), VerbosityKey, verbosityArg };

            int parseResult = Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (o) => ValidateOptions(o, verbosityLevel: VerbosityLevel.Quiet),
                    FailIfCalled);

            Assert.AreEqual(ExpectedParseSuccess, parseResult);
            _processHelperMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ParseArguments_TargetIsSet_VerbosityIsDefault_SucceedsWithDefaultVerbosity()
        {
            const string verbosityArg = "default";

            _processHelperMock.Setup(x => x.FindProcessById(TestProcessId))
                .Returns(TestProcessName);
            string[] args = { ProcessIdKey, TestProcessId.ToString(), VerbosityKey, verbosityArg };

            int parseResult = Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (o) => ValidateOptions(o, verbosityLevel: VerbosityLevel.Default),
                    FailIfCalled);

            Assert.AreEqual(ExpectedParseSuccess, parseResult);
            _processHelperMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ParseArguments_TargetIsSet_VerbosityIsVerbose_SucceedsWithVerboseVerbosity()
        {
            const string verbosityArg = "verbose";

            _processHelperMock.Setup(x => x.FindProcessById(TestProcessId))
                .Returns(TestProcessName);
            string[] args = { ProcessIdKey, TestProcessId.ToString(), VerbosityKey, verbosityArg };

            int parseResult = Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (o) => ValidateOptions(o, verbosityLevel: VerbosityLevel.Verbose),
                    FailIfCalled);

            Assert.AreEqual(ExpectedParseSuccess, parseResult);
            _processHelperMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ParseArguments_TargetIsSet_VerbosityIsUnrecognized_SucceedsWithErrorParameter()
        {
            const string verbosityArg = "Unrecognized";
            string parameterError = string.Empty;

            _processHelperMock.Setup(x => x.FindProcessById(TestProcessId))
                .Returns(TestProcessName);
            _errorCollectorMock.Setup(x => x.AddParameterError(It.IsAny<string>()))
                .Callback<string>((s) => parameterError = s);
            string[] args = { ProcessIdKey, TestProcessId.ToString(), VerbosityKey, verbosityArg };

            int parseResult = Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (o) => ValidateOptions(o, verbosityLevel: VerbosityLevel.Default),
                    FailIfCalled);

            Assert.AreEqual(ExpectedParseSuccess, parseResult);
            Assert.IsTrue(parameterError.EndsWith(verbosityArg));

            _processHelperMock.VerifyAll();
            _errorCollectorMock.VerifyAll();
        }
    }
}
