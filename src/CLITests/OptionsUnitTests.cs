// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using AxeWindowsCLI;
using CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CLITests
{
    [TestClass]
    public class OptionUnitTests
    {
        const int ExpectedParseSuccess = 123;
        const int UnexpectedFailure = 789;

        const string ProcessIdKey = "--processid";
        const string ProcessNameKey = "--processname";
        const string VerbosityKey = "--verbosity";
        const string ScanIdKey = "--scanid";
        const string ShowThirdPartyNoticesKey = "--showthirdpartynotices";

        const string TestProcessName = "MyProcess";
        const int TestProcessId = 42;

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

        private int ValidateOptions(Options options, string processName = null,
            int processId = 0, string outputDirectory = null, string scanId = null,
            string verbosity = null, bool showThirdPartyNotices = false)
        {
            Assert.AreEqual(processName, options.ProcessName);
            Assert.AreEqual(processId, options.ProcessId);
            Assert.AreEqual(scanId, options.ScanId);
            Assert.AreEqual(outputDirectory, options.OutputDirectory);
            Assert.AreEqual(verbosity, options.Verbosity);
            Assert.AreEqual(VerbosityLevel.Default, options.VerbosityLevel);
            Assert.AreEqual(showThirdPartyNotices, options.ShowThirdPartyNotices);
            return ExpectedParseSuccess;
        }

        [TestMethod]
        [Timeout(1000)]
        public void ParseArguments_TargetByProcesssName_SucceedsWithCorrectProcessName()
        {
            string[] args = { ProcessNameKey, TestProcessName };

            int parseResult = Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (o) => ValidateOptions(o, processName: TestProcessName),
                    FailIfCalled);

            Assert.AreEqual(ExpectedParseSuccess, parseResult);
        }

        [TestMethod]
        [Timeout(1000)]
        public void ParseArguments_TargetByProcesssId_SucceedsWithCorrectProcessId()
        {
            string[] args = { ProcessIdKey, TestProcessId.ToString() };

            int parseResult = Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (o) => ValidateOptions(o, processId: TestProcessId),
                    FailIfCalled);

            Assert.AreEqual(ExpectedParseSuccess, parseResult);
        }

        [TestMethod]
        [Timeout(1000)]
        public void ParseArguments_ScanIdIsSet_SucceedsWithCorrectScanId()
        {
            const string expectedScanId = "Some Scan";

            string[] args = { ScanIdKey, expectedScanId };

            int parseResult = Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (o) => ValidateOptions(o, scanId: expectedScanId),
                    FailIfCalled);

            Assert.AreEqual(ExpectedParseSuccess, parseResult);
        }

        [TestMethod]
        [Timeout(1000)]
        public void ParseArguments_VerbosityIsSet_SucceedsWithCorrectVerbosity()
        {
            const string verbosityArg = "Some Verbosity";

            string[] args = { VerbosityKey, verbosityArg };

            int parseResult = Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (o) => ValidateOptions(o, verbosity: verbosityArg),
                    FailIfCalled);

            Assert.AreEqual(ExpectedParseSuccess, parseResult);
        }

        [TestMethod]
        [Timeout(1000)]
        public void ParseArguments_ShowThirdPartyNotices_SucceedsWithThirdPartyNoticesSet()
        {
            string[] args = { ShowThirdPartyNoticesKey };

            int parseResult = Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (o) => ValidateOptions(o, showThirdPartyNotices: true),
                    FailIfCalled);

            Assert.AreEqual(ExpectedParseSuccess, parseResult);
        }
    }
}
