// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using AxeWindowsScanner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;

namespace CLITests
{
    [TestClass]
    public class OutputGeneratorUnitTests
    {
        enum WriteSource
        {
            WriteStringOnly,
            WriteOneParam,
            WriteLineEmpty,
            WriteLineStringOnly,
            WriteLineOneParam,
        }

        class WriteCall
        {
            public string Format { get; }
            public WriteSource Source { get; }

            public WriteCall(string format, WriteSource source)
            {

                if (source == WriteSource.WriteLineEmpty)
                {
                    Assert.IsNull(format);
                }
                else
                {
                    Assert.IsNotNull(format);
                }
                Format = format;
                Source = source;
            }
        }

        const string TestProcessName = "SuperDuperApp";
        const int TestProcessId = 2468;
        const string TestScanId = "ThisIsMyPrettyScan";
        const string TestA11yTestFile = @"c:\outputDirectory\scanId.a11ytest";

        const string AppTitleStart = "Axe.Windows Accessibility Scanner";
        const string ScanTargetIntro = "Scan Target:";
        const string ScanTargetProcessNameStart = " Process Name =";
        const string ScanTargetProcessIdStart = " Process ID =";
        const string ScanTargetComma = ",";
        const string ScanIdStart = "Scan Id =";
        const string ErrorCountGeneralStart = "{0} errors ";
        const string ErrorCountOneErrorStart = "1 error ";
        const string OutputFileStart = "Results were written ";

        private Mock<TextWriter> _writerMock;
        private Mock<IOptions> _optionsMock;
        private Mock<IErrorCollector> _errorCollectorMock;
        private List<WriteCall> _actualWriteCalls;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _writerMock = new Mock<TextWriter>(MockBehavior.Strict);
            _optionsMock = new Mock<IOptions>(MockBehavior.Strict);
            _errorCollectorMock = new Mock<IErrorCollector>(MockBehavior.Strict);
            _actualWriteCalls = new List<WriteCall>();
        }

        private void VerifyAllMocks()
        {
            _writerMock.VerifyAll();
            _optionsMock.VerifyAll();
            _errorCollectorMock.VerifyAll();
        }

        private void AddWriteCall(string format, WriteSource source)
        {
            _actualWriteCalls.Add(new WriteCall(format, source));
        }

        private void MockWriteStringOnly()
        {
            _writerMock.Setup(x => x.Write(It.IsAny<string>()))
                .Callback<string>((s) => AddWriteCall(s, WriteSource.WriteStringOnly));
        }

        private void MockWriteOneParam()
        {
            _writerMock.Setup(x => x.Write(It.IsAny<string>(), It.IsAny<object>()))
                .Callback<string, object>((s, _) => AddWriteCall(s, WriteSource.WriteOneParam));
        }

        private void MockWriteLineEmpty()
        {
            _writerMock.Setup(x => x.WriteLine())
                .Callback(() => AddWriteCall(null, WriteSource.WriteLineEmpty));
        }

        private void MockWriteLineStringOnly()
        {
            _writerMock.Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>((s) => AddWriteCall(s, WriteSource.WriteLineStringOnly));
        }

        private void MockWriteLineOneParam()
        {
            _writerMock.Setup(x => x.WriteLine(It.IsAny<string>(), It.IsAny<object>()))
                .Callback<string, object>((s, _) => AddWriteCall(s, WriteSource.WriteLineOneParam));
        }

        private void VerifyWriteCalls(IEnumerable<WriteCall> expectedCalls)
        {
            int verified = 0;

            foreach (WriteCall expectedCall in expectedCalls)
            {
                WriteCall actualCall = _actualWriteCalls[verified];
                Assert.AreEqual(expectedCall.Source, actualCall.Source, "Actual Format = " + actualCall.Format);
                if (expectedCall.Source == WriteSource.WriteLineEmpty)
                {
                    Assert.IsNull(expectedCall.Format);
                    Assert.IsNull(actualCall.Format);
                }
                else
                {
                    Assert.IsTrue(actualCall.Format.StartsWith(expectedCall.Format), "Actual Format = " + actualCall.Format);
                }
                verified++;
            }

            Assert.AreEqual(verified, _actualWriteCalls.Count);
        }

        private void SetOptions(VerbosityLevel verbosityLevel = VerbosityLevel.Default,
            string processName = IProcessHelper.InvalidProcessName, int processId = IProcessHelper.InvalidProcessId,
            string scanId = null)
        {
            _optionsMock.Setup(x => x.VerbosityLevel).Returns(verbosityLevel);
            _optionsMock.Setup(x => x.ProcessName).Returns(processName);
            _optionsMock.Setup(x => x.ProcessId).Returns(processId);
            _optionsMock.Setup(x => x.ScanId).Returns(scanId);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Ctor_WriterIsNull_ThrowsArgumnentNullException()
        {
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(
                () => new OutputGenerator(null));
            Assert.AreEqual("writer", e.ParamName);
        }

        [TestMethod]
        [Timeout(1000)]
        public void ShowBanner_OptionsIsNull_ThrowsArgumentNullException()
        {
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(
                () => generator.ShowBanner(null));
            Assert.AreEqual("options", e.ParamName);
        }

        [TestMethod]
        [Timeout(1000)]
        public void ShowBanner_VerbosityIsQuiet_IsSilent()
        {
            _optionsMock.Setup(x => x.VerbosityLevel).Returns(VerbosityLevel.Quiet);
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);

            generator.ShowBanner(_optionsMock.Object);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ShowBanner_VerbosityIsDefault_NoProcessName_NoProcessId_NoScanId_WritesAppHeader()
        {
            SetOptions();
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);

            generator.ShowBanner(_optionsMock.Object);

            WriteCall[] expectedCalls = 
            { 
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ShowBanner_VerbosityIsDefault_ProcessName_NoProcessId_NoScanId_WritesAppHeaderAndScanTarget()
        {
            SetOptions(processName: TestProcessName);
            MockWriteStringOnly();
            MockWriteOneParam();
            MockWriteLineEmpty();
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);

            generator.ShowBanner(_optionsMock.Object);

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ScanTargetIntro, WriteSource.WriteStringOnly),
                new WriteCall(ScanTargetProcessNameStart, WriteSource.WriteOneParam),
                new WriteCall(null, WriteSource.WriteLineEmpty),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ShowBanner_VerbosityIsDefault_NoProcessName_ProcessId_NoScanId_WritesAppHeaderAndScanTarget()
        {
            SetOptions(processId: TestProcessId);
            MockWriteStringOnly();
            MockWriteOneParam();
            MockWriteLineEmpty();
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);

            generator.ShowBanner(_optionsMock.Object);

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ScanTargetIntro, WriteSource.WriteStringOnly),
                new WriteCall(ScanTargetProcessIdStart, WriteSource.WriteOneParam),
                new WriteCall(null, WriteSource.WriteLineEmpty),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ShowBanner_VerbosityIsDefault_ProcessName_ProcessId_NoScanId_WritesAppHeaderAndScanTarget()
        {
            SetOptions(processName: TestProcessName, processId: TestProcessId);
            MockWriteStringOnly();
            MockWriteOneParam();
            MockWriteLineEmpty();
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);

            generator.ShowBanner(_optionsMock.Object);

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ScanTargetIntro, WriteSource.WriteStringOnly),
                new WriteCall(ScanTargetProcessNameStart, WriteSource.WriteOneParam),
                new WriteCall(ScanTargetComma, WriteSource.WriteStringOnly),
                new WriteCall(ScanTargetProcessIdStart, WriteSource.WriteOneParam),
                new WriteCall(null, WriteSource.WriteLineEmpty),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ShowBanner_VerbosityIsDefault_NoProcessName_NoProcessId_ScanId_WritesAppHeaderAndScanId()
        {
            SetOptions(scanId: TestScanId);
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);

            generator.ShowBanner(_optionsMock.Object);

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ScanIdStart, WriteSource.WriteLineOneParam),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ShowBanner_CalledMultipleTimes_ShowsOnlyOnce()
        {
            SetOptions();
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);

            generator.ShowBanner(_optionsMock.Object);
            generator.ShowBanner(_optionsMock.Object);
            generator.ShowBanner(_optionsMock.Object);

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }

        private ScanResults BuildTestScanResults(int errorCount = 0, string a11yTestFile = null)
        {
            List<ScanResult> errors = new List<ScanResult>(errorCount);

            for(int loop = 0; loop < errorCount; loop++)
            {
                errors.Add(new ScanResult());
            };

            return new ScanResults
            {
                Errors = errors,
                ErrorCount = errors.Count,
                OutputFile = new OutputFile { A11yTest = a11yTestFile },
            };
        }

        [TestMethod]
        [Timeout(1000)]
        public void ShowOutput_ScanFoundNoErrors_VerbosityIsQuiet_IsSilent()
        {
            _optionsMock.Setup(x => x.VerbosityLevel).Returns(VerbosityLevel.Quiet);
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);
            ScanResults scanResults = BuildTestScanResults();

            generator.ShowOutput((int)ExitCode.ScanFoundNoErrors,
                _optionsMock.Object, _errorCollectorMock.Object, scanResults);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ShowOutput_ScanFoundErrors_VerbosityIsQuiet_IsSilent()
        {
            _optionsMock.Setup(x => x.VerbosityLevel).Returns(VerbosityLevel.Quiet);
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);
            ScanResults scanResults = BuildTestScanResults(errorCount: 1, a11yTestFile: TestA11yTestFile);

            generator.ShowOutput((int)ExitCode.ScanFoundErrors,
                _optionsMock.Object, _errorCollectorMock.Object, scanResults);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ShowOutput_ScanFoundNoErrors_VerbosityIsDefault_WritesBannerAndSummary()
        {
            SetOptions();
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);
            ScanResults scanResults = BuildTestScanResults(errorCount: 0);

            generator.ShowOutput((int)ExitCode.ScanFoundNoErrors,
                _optionsMock.Object, _errorCollectorMock.Object, scanResults);

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorCountGeneralStart, WriteSource.WriteLineOneParam),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ShowOutput_ScanFoundOneError_VerbosityIsDefault_WritesBannerAndSummary()
        {
            SetOptions();
            MockWriteLineOneParam();
            MockWriteLineStringOnly();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);
            ScanResults scanResults = BuildTestScanResults(errorCount: 1, a11yTestFile: TestA11yTestFile);

            generator.ShowOutput((int)ExitCode.ScanFoundNoErrors,
                _optionsMock.Object, _errorCollectorMock.Object, scanResults);

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorCountOneErrorStart, WriteSource.WriteLineStringOnly),
                new WriteCall(OutputFileStart, WriteSource.WriteLineOneParam),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void ShowOutput_ScanFoundMoreThanOneError_VerbosityIsDefault_WritesBannerAndSummary()
        {
            SetOptions();
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);
            ScanResults scanResults = BuildTestScanResults(errorCount: 2, a11yTestFile: TestA11yTestFile);

            generator.ShowOutput((int)ExitCode.ScanFoundNoErrors,
                _optionsMock.Object, _errorCollectorMock.Object, scanResults);

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorCountGeneralStart, WriteSource.WriteLineOneParam),
                new WriteCall(OutputFileStart, WriteSource.WriteLineOneParam),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }
    }
}
