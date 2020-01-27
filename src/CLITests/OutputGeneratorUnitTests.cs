// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using Axe.Windows.Rules;
using AxeWindowsCLI;
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
            WriteLineTwoParams,
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
        const string ErrorVerboseCountStart = "Error ";
        const string ErrorVerbosePropertiesHeaderStart = "  Element Properties";
        const string ErrorVerbosePropertyPairStart = "    {0} = {1}";
        const string ErrorVerbosePatternsHeaderStart = "  Element Patterns";
        const string ErrorVerbosePatternIndex = "    {0}";
        const string ErrorVerboseSeparatorStart = "---------------------";
        const string OutputFileStart = "Results were written ";
        const string ExceptionInfoStart = "The following exception was caught: ";
        const string ParameterErrorStart = "Parameter Error: ";

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

        private void MockWriteLineTwoParams()
        {
            _writerMock.Setup(x => x.WriteLine(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<object>()))
                .Callback<string, object, object>((s, _, __) => AddWriteCall(s, WriteSource.WriteLineTwoParams));
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
        public void WriteBanner_OptionsIsNull_ThrowsArgumentNullException()
        {
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(
                () => generator.WriteBanner(null));
            Assert.AreEqual("options", e.ParamName);
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteBanner_VerbosityIsQuiet_IsSilent()
        {
            _optionsMock.Setup(x => x.VerbosityLevel).Returns(VerbosityLevel.Quiet);
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);

            generator.WriteBanner(_optionsMock.Object);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteBanner_VerbosityIsDefault_NoProcessName_NoProcessId_NoScanId_WritesAppHeader()
        {
            SetOptions();
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);

            generator.WriteBanner(_optionsMock.Object);

            WriteCall[] expectedCalls = 
            { 
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteBanner_VerbosityIsDefault_ProcessName_NoProcessId_NoScanId_WritesAppHeaderAndScanTarget()
        {
            SetOptions(processName: TestProcessName);
            MockWriteStringOnly();
            MockWriteOneParam();
            MockWriteLineEmpty();
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);

            generator.WriteBanner(_optionsMock.Object);

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
        public void WriteBanner_VerbosityIsDefault_NoProcessName_ProcessId_NoScanId_WritesAppHeaderAndScanTarget()
        {
            SetOptions(processId: TestProcessId);
            MockWriteStringOnly();
            MockWriteOneParam();
            MockWriteLineEmpty();
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);

            generator.WriteBanner(_optionsMock.Object);

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
        public void WriteBanner_VerbosityIsDefault_ProcessName_ProcessId_NoScanId_WritesAppHeaderAndScanTarget()
        {
            SetOptions(processName: TestProcessName, processId: TestProcessId);
            MockWriteStringOnly();
            MockWriteOneParam();
            MockWriteLineEmpty();
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);

            generator.WriteBanner(_optionsMock.Object);

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
        public void WriteBanner_VerbosityIsDefault_NoProcessName_NoProcessId_ScanId_WritesAppHeaderAndScanId()
        {
            SetOptions(scanId: TestScanId);
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);

            generator.WriteBanner(_optionsMock.Object);

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
        public void WriteBanner_CalledMultipleTimes_WritesOnlyOnce()
        {
            SetOptions();
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);

            generator.WriteBanner(_optionsMock.Object);
            generator.WriteBanner(_optionsMock.Object);
            generator.WriteBanner(_optionsMock.Object);

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }

        private IReadOnlyDictionary<string, string> BuildTestProperties(int? propertyCount)
        {
            if (!propertyCount.HasValue)
                return null;

            Dictionary<string, string> properties = new Dictionary<string, string>();

            for (int loop = 0; loop < propertyCount.Value; loop++)
            {
                properties.Add(string.Format("Property{0}", loop), string.Format("Value{0}", loop));
            }

            return properties;
        }

        private IEnumerable<string> BuildTestPatterns(int? patternCount)
        {
            if (!patternCount.HasValue)
                return null;

            List<string> patterns = new List<string>();

            for (int loop = 0; loop < patternCount.Value; loop++)
            {
                patterns.Add(string.Format("Pattern{0}", loop));
            }

            return patterns;
        }

        private ScanResults BuildTestScanResults(int errorCount = 0, string a11yTestFile = null,
            int? propertyCount = null, int? patternCount = null)
        {
            List<ScanResult> errors = new List<ScanResult>(errorCount);

            for(int loop = 0; loop < errorCount; loop++)
            {
                errors.Add(new ScanResult
                {
                    Element = new ElementInfo
                    {
                        Properties = BuildTestProperties(propertyCount),
                        Patterns = BuildTestPatterns(patternCount),
                    },
                    Rule = new RuleInfo
                    {
                        Description = "Test Rule",
                    },
                }); ;
            };

            return new ScanResults
            {
                Errors = errors,
                ErrorCount = errors.Count,
                OutputFile = OutputFile.BuildFromA11yTestFile(a11yTestFile),
            };
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ScanResultsNoErrors_VerbosityIsQuiet_IsSilent()
        {
            _errorCollectorMock.Setup(x => x.ParameterErrors).Returns(new List<string>());
            _errorCollectorMock.Setup(x => x.Exceptions).Returns(new List<Exception>());
            _optionsMock.Setup(x => x.VerbosityLevel).Returns(VerbosityLevel.Quiet);
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);
            ScanResults scanResults = BuildTestScanResults();

            generator.WriteOutput(_optionsMock.Object, _errorCollectorMock.Object, scanResults);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ScanResultsWithErrors_VerbosityIsQuiet_IsSilent()
        {
            _errorCollectorMock.Setup(x => x.ParameterErrors).Returns(new List<string>());
            _errorCollectorMock.Setup(x => x.Exceptions).Returns(new List<Exception>());
            _optionsMock.Setup(x => x.VerbosityLevel).Returns(VerbosityLevel.Quiet);
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);
            ScanResults scanResults = BuildTestScanResults(errorCount: 1, a11yTestFile: TestA11yTestFile);

            generator.WriteOutput(_optionsMock.Object, _errorCollectorMock.Object, scanResults);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ScanResultsNoErrors_VerbosityIsDefault_WritesBannerAndSummary()
        {
            _errorCollectorMock.Setup(x => x.ParameterErrors).Returns(new List<string>());
            _errorCollectorMock.Setup(x => x.Exceptions).Returns(new List<Exception>());
            SetOptions();
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);
            ScanResults scanResults = BuildTestScanResults();

            generator.WriteOutput(_optionsMock.Object, _errorCollectorMock.Object, scanResults);

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
        public void WriteOutput_ScanResultsOneError_VerbosityIsDefault_WritesBannerAndSummary()
        {
            _errorCollectorMock.Setup(x => x.ParameterErrors).Returns(new List<string>());
            _errorCollectorMock.Setup(x => x.Exceptions).Returns(new List<Exception>());
            SetOptions();
            MockWriteLineOneParam();
            MockWriteLineStringOnly();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);
            ScanResults scanResults = BuildTestScanResults(errorCount: 1, a11yTestFile: TestA11yTestFile);

            generator.WriteOutput(_optionsMock.Object, _errorCollectorMock.Object, scanResults);

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
        public void WriteOutput_ScanResultsMultipleErrors_VerbosityIsDefault_WritesBannerAndSummary()
        {
            _errorCollectorMock.Setup(x => x.ParameterErrors).Returns(new List<string>());
            _errorCollectorMock.Setup(x => x.Exceptions).Returns(new List<Exception>());
            SetOptions();
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);
            ScanResults scanResults = BuildTestScanResults(errorCount: 2, a11yTestFile: TestA11yTestFile);

            generator.WriteOutput(_optionsMock.Object, _errorCollectorMock.Object, scanResults);

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorCountGeneralStart, WriteSource.WriteLineOneParam),
                new WriteCall(OutputFileStart, WriteSource.WriteLineOneParam),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ScanResultsMultipleErrors_NoPatterns_NoProperties_VerbosityIsVerbose_WritesBannerAndSummaryAndDetails()
        {
            _errorCollectorMock.Setup(x => x.ParameterErrors).Returns(new List<string>());
            _errorCollectorMock.Setup(x => x.Exceptions).Returns(new List<Exception>());
            SetOptions(verbosityLevel: VerbosityLevel.Verbose);
            MockWriteLineStringOnly();
            MockWriteLineOneParam();
            MockWriteLineTwoParams();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);
            ScanResults scanResults = BuildTestScanResults(errorCount: 2, a11yTestFile: TestA11yTestFile);

            generator.WriteOutput(_optionsMock.Object, _errorCollectorMock.Object, scanResults);

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorCountGeneralStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerboseCountStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerboseSeparatorStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerboseCountStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerboseSeparatorStart, WriteSource.WriteLineStringOnly),
                new WriteCall(OutputFileStart, WriteSource.WriteLineOneParam),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ScanResultsMultipleErrors_TwoPatterns_NoProperties_VerbosityIsVerbose_WritesBannerAndSummaryAndDetails()
        {
            _errorCollectorMock.Setup(x => x.ParameterErrors).Returns(new List<string>());
            _errorCollectorMock.Setup(x => x.Exceptions).Returns(new List<Exception>());
            SetOptions(verbosityLevel: VerbosityLevel.Verbose);
            MockWriteLineStringOnly();
            MockWriteLineOneParam();
            MockWriteLineTwoParams();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);
            ScanResults scanResults = BuildTestScanResults(errorCount: 2, a11yTestFile: TestA11yTestFile,
                patternCount: 2);

            generator.WriteOutput(_optionsMock.Object, _errorCollectorMock.Object, scanResults);

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorCountGeneralStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerboseCountStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerbosePatternsHeaderStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerbosePatternIndex, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerbosePatternIndex, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerboseSeparatorStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerboseCountStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerbosePatternsHeaderStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerbosePatternIndex, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerbosePatternIndex, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerboseSeparatorStart, WriteSource.WriteLineStringOnly),
                new WriteCall(OutputFileStart, WriteSource.WriteLineOneParam),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ScanResultsMultipleErrors_NoPatterns_TwoProperties_VerbosityIsVerbose_WritesBannerAndSummaryAndDetails()
        {
            _errorCollectorMock.Setup(x => x.ParameterErrors).Returns(new List<string>());
            _errorCollectorMock.Setup(x => x.Exceptions).Returns(new List<Exception>());
            SetOptions(verbosityLevel: VerbosityLevel.Verbose);
            MockWriteLineStringOnly();
            MockWriteLineOneParam();
            MockWriteLineTwoParams();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);
            ScanResults scanResults = BuildTestScanResults(errorCount: 2, a11yTestFile: TestA11yTestFile,
                propertyCount: 2);

            generator.WriteOutput(_optionsMock.Object, _errorCollectorMock.Object, scanResults);

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorCountGeneralStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerboseCountStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerbosePropertiesHeaderStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerbosePropertyPairStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerbosePropertyPairStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerboseSeparatorStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerboseCountStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerbosePropertiesHeaderStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerbosePropertyPairStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerbosePropertyPairStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerboseSeparatorStart, WriteSource.WriteLineStringOnly),
                new WriteCall(OutputFileStart, WriteSource.WriteLineOneParam),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ScanResultsMultipleErrors_TwoPatterns_TwoProperties_VerbosityIsVerbose_WritesBannerAndSummaryAndDetails()
        {
            _errorCollectorMock.Setup(x => x.ParameterErrors).Returns(new List<string>());
            _errorCollectorMock.Setup(x => x.Exceptions).Returns(new List<Exception>());
            SetOptions(verbosityLevel: VerbosityLevel.Verbose);
            MockWriteLineStringOnly();
            MockWriteLineOneParam();
            MockWriteLineTwoParams();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);
            ScanResults scanResults = BuildTestScanResults(errorCount: 2, a11yTestFile: TestA11yTestFile,
                patternCount: 2, propertyCount: 2);

            generator.WriteOutput(_optionsMock.Object, _errorCollectorMock.Object, scanResults);

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorCountGeneralStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerboseCountStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerbosePropertiesHeaderStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerbosePropertyPairStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerbosePropertyPairStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerbosePatternsHeaderStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerbosePatternIndex, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerbosePatternIndex, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerboseSeparatorStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerboseCountStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerbosePropertiesHeaderStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerbosePropertyPairStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerbosePropertyPairStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerbosePatternsHeaderStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerbosePatternIndex, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerbosePatternIndex, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerboseSeparatorStart, WriteSource.WriteLineStringOnly),
                new WriteCall(OutputFileStart, WriteSource.WriteLineOneParam),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ParameterErrorsExist_VerbosityIsQuiet_WritesBannerAndParameterErrors()
        {
            _errorCollectorMock.Setup(x => x.ParameterErrors).Returns(new List<string>
            {
                "Parameter Error 1",
                "Parameter Error 2",
            });
            _errorCollectorMock.Setup(x => x.Exceptions).Returns(new List<Exception>());
            SetOptions(verbosityLevel: VerbosityLevel.Quiet);
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);

            generator.WriteOutput(_optionsMock.Object, _errorCollectorMock.Object, null);

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ParameterErrorStart, WriteSource.WriteLineOneParam),
                new WriteCall(ParameterErrorStart, WriteSource.WriteLineOneParam),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ExceptionHasBeenThrown_VerbosityIsQuiet_WritesBannerAndExceptionInfo()
        {
            _errorCollectorMock.Setup(x => x.ParameterErrors).Returns(new List<string>());
            _errorCollectorMock.Setup(x => x.Exceptions).Returns(new List<Exception> { new MissingMethodException() });
            SetOptions(verbosityLevel: VerbosityLevel.Quiet);
            MockWriteLineOneParam();
            IOutputGenerator generator = new OutputGenerator(_writerMock.Object);

            generator.WriteOutput(_optionsMock.Object, _errorCollectorMock.Object, null);

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ExceptionInfoStart, WriteSource.WriteLineOneParam),
            };
            VerifyWriteCalls(expectedCalls);
            VerifyAllMocks();
        }
    }
}
