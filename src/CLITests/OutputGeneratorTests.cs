﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using Axe.Windows.Rules;
using AxeWindowsCLI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace AxeWindowsCLITests
{
    [TestClass]
    public class OutputGeneratorTests
    {
        const string TestProcessName = "SuperDuperApp";
        const int TestProcessId = 2468;
        const string TestScanId = "ThisIsMyPrettyScan";
        const string TestA11yTestFile = @"c:\outputDirectory\scanId.a11ytest";

        const string AppTitleStart = "Axe.Windows Accessibility Scanner";
        const string ThirdPartyNoticeStart = "Opening Third Party Notices in default browser";
        const string ScanTargetIntro = "Scan Target:";
        const string ScanTargetProcessNameStart = " Process Name =";
        const string ScanTargetProcessIdStart = " Process ID =";
        const string ScanTargetRootWindowHandleStart = " HWND =";
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
        const string FrameworkIssueLinkOneParam = "  Potential Framework Issue: {0}";
        const string OutputFileStart = "Results were written ";
        const string UnableToComplete = "Unable to complete. ";
        const string NoFurtherData = "No further data is available.";
        const string ExceptionInfoStart = "The following exception was caught: ";

        private Mock<TextWriter> _writerMock;
        private Mock<IOptions> _optionsMock;
        private IOutputGenerator _testSubject;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _writerMock = new Mock<TextWriter>(MockBehavior.Strict);
            _optionsMock = new Mock<IOptions>(MockBehavior.Strict);

            _testSubject = new OutputGenerator(_writerMock.Object);
        }

        private void VerifyAllMocks()
        {
            _writerMock.VerifyAll();
            _optionsMock.VerifyAll();
        }

        private void SetOptions(VerbosityLevel verbosityLevel = VerbosityLevel.Default,
            string processName = null, int processId = -1,
            string scanId = null, IntPtr? scanRootWindowHandle = null)
        {
            _optionsMock.Setup(x => x.VerbosityLevel).Returns(verbosityLevel);
            _optionsMock.Setup(x => x.ProcessName).Returns(processName);
            _optionsMock.Setup(x => x.ProcessId).Returns(processId);
            _optionsMock.Setup(x => x.ScanId).Returns(scanId);
            if (processId > 0 || !string.IsNullOrEmpty(processName))
            {
                _optionsMock.Setup(x => x.ScanRootWindowHandle).Returns(scanRootWindowHandle.GetValueOrDefault(IntPtr.Zero));
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void Ctor_WriterIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(
                () => new OutputGenerator(null));
            Assert.AreEqual("writer", e.ParamName);
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteBanner_OptionsIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(
                () => _testSubject.WriteBanner(null));
            Assert.AreEqual("options", e.ParamName);
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteBanner_VerbosityIsQuiet_IsSilent()
        {
            _optionsMock.Setup(x => x.VerbosityLevel).Returns(VerbosityLevel.Quiet);

            _testSubject.WriteBanner(_optionsMock.Object);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteBanner_VerbosityIsDefault_NoProcessName_NoProcessId_NoScanId_WritesAppHeader()
        {
            SetOptions();
            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
            };
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);

            _testSubject.WriteBanner(_optionsMock.Object);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteBanner_VerbosityIsDefault_ProcessName_NoProcessId_NoScanId_WritesAppHeaderAndScanTarget()
        {
            SetOptions(processName: TestProcessName);
            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ScanTargetIntro, WriteSource.WriteStringOnly),
                new WriteCall(ScanTargetProcessNameStart, WriteSource.WriteOneParam),
                new WriteCall(null, WriteSource.WriteLineEmpty),
            };
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);

            _testSubject.WriteBanner(_optionsMock.Object);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteBanner_VerbosityIsDefault_NoProcessName_ProcessId_NoScanId_WritesAppHeaderAndScanTarget()
        {
            SetOptions(processId: TestProcessId);
            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ScanTargetIntro, WriteSource.WriteStringOnly),
                new WriteCall(ScanTargetProcessIdStart, WriteSource.WriteOneParam),
                new WriteCall(null, WriteSource.WriteLineEmpty),
            };
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);

            _testSubject.WriteBanner(_optionsMock.Object);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteBanner_VerbosityIsDefault_ProcessName_ProcessId_NoScanId_WritesAppHeaderAndScanTarget()
        {
            SetOptions(processName: TestProcessName, processId: TestProcessId);
            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ScanTargetIntro, WriteSource.WriteStringOnly),
                new WriteCall(ScanTargetProcessNameStart, WriteSource.WriteOneParam),
                new WriteCall(ScanTargetComma, WriteSource.WriteStringOnly),
                new WriteCall(ScanTargetProcessIdStart, WriteSource.WriteOneParam),
                new WriteCall(null, WriteSource.WriteLineEmpty),
            };
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);

            _testSubject.WriteBanner(_optionsMock.Object);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteBanner_VerbosityIsDefault_ProcessId_ScanRootWindowHandle_WritesHWND()
        {
            SetOptions(processId: TestProcessId, scanRootWindowHandle: new(47));
            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ScanTargetIntro, WriteSource.WriteStringOnly),
                new WriteCall(ScanTargetProcessIdStart, WriteSource.WriteOneParam),
                new WriteCall(ScanTargetComma, WriteSource.WriteStringOnly),
                new WriteCall(ScanTargetRootWindowHandleStart, WriteSource.WriteOneParam),
                new WriteCall(null, WriteSource.WriteLineEmpty),
            };
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);

            _testSubject.WriteBanner(_optionsMock.Object);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteBanner_VerbosityIsDefault_NoProcessName_NoProcessId_ScanId_WritesAppHeaderAndScanId()
        {
            SetOptions(scanId: TestScanId);
            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ScanIdStart, WriteSource.WriteLineOneParam),
            };
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);

            _testSubject.WriteBanner(_optionsMock.Object);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteBanner_CalledMultipleTimes_WritesOnlyOnce()
        {
            SetOptions();
            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
            };
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);

            _testSubject.WriteBanner(_optionsMock.Object);
            _testSubject.WriteBanner(_optionsMock.Object);
            _testSubject.WriteBanner(_optionsMock.Object);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteBanner_ShowThirdPartyNotices_WritesAppHeaderAndThirdPartyNotice()
        {
            const string testFile = @"c:\somePath\someFile.html";

            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ThirdPartyNoticeStart, WriteSource.WriteLineOneParam),
            };
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);

            _testSubject.WriteThirdPartyNoticeOutput(testFile);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        private static IReadOnlyDictionary<string, string> BuildTestProperties(int? propertyCount)
        {
            if (!propertyCount.HasValue)
                return null;

            Dictionary<string, string> properties = new Dictionary<string, string>();

            for (int loop = 0; loop < propertyCount.Value; loop++)
            {
                properties.Add(string.Format(CultureInfo.InvariantCulture, "Property{0}", loop), string.Format(CultureInfo.InvariantCulture, "Value{0}", loop));
            }

            return properties;
        }

        private static IEnumerable<string> BuildTestPatterns(int? patternCount)
        {
            if (!patternCount.HasValue)
                return null;

            List<string> patterns = new List<string>();

            for (int loop = 0; loop < patternCount.Value; loop++)
            {
                patterns.Add(string.Format(CultureInfo.InvariantCulture, "Pattern{0}", loop));
            }

            return patterns;
        }

        private static WindowScanOutput BuildTestScanResults(int errorCount = 0, string a11yTestFile = null,
            int? propertyCount = null, int? patternCount = null, string frameworkIssueLink = null)
        {
            List<ScanResult> errors = new List<ScanResult>(errorCount);

            for (int loop = 0; loop < errorCount; loop++)
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
                        FrameworkIssueLink = frameworkIssueLink,
                    },
                }); ;
            };

            return new WindowScanOutput
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
            _optionsMock.Setup(x => x.VerbosityLevel).Returns(VerbosityLevel.Quiet);
            WindowScanOutput scanResults = BuildTestScanResults();

            _testSubject.WriteOutput(_optionsMock.Object, scanResults, null);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ScanResultsWithErrors_VerbosityIsQuiet_IsSilent()
        {
            _optionsMock.Setup(x => x.VerbosityLevel).Returns(VerbosityLevel.Quiet);
            WindowScanOutput scanResults = BuildTestScanResults(errorCount: 1, a11yTestFile: TestA11yTestFile);

            _testSubject.WriteOutput(_optionsMock.Object, scanResults, null);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ScanResultsNoErrors_VerbosityIsDefault_WritesBannerAndSummary()
        {
            SetOptions();
            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorCountGeneralStart, WriteSource.WriteLineOneParam),
            };
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);
            WindowScanOutput scanResults = BuildTestScanResults();

            _testSubject.WriteOutput(_optionsMock.Object, scanResults, null);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ScanResultsOneError_VerbosityIsDefault_WritesBannerAndSummary()
        {
            SetOptions();
            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorCountOneErrorStart, WriteSource.WriteLineStringOnly),
                new WriteCall(OutputFileStart, WriteSource.WriteLineOneParam),
            };
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);
            WindowScanOutput scanResults = BuildTestScanResults(errorCount: 1, a11yTestFile: TestA11yTestFile);

            _testSubject.WriteOutput(_optionsMock.Object, scanResults, null);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ScanResultsMultipleErrors_VerbosityIsDefault_WritesBannerAndSummary()
        {
            SetOptions();
            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorCountGeneralStart, WriteSource.WriteLineOneParam),
                new WriteCall(OutputFileStart, WriteSource.WriteLineOneParam),
            };
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);
            WindowScanOutput scanResults = BuildTestScanResults(errorCount: 2, a11yTestFile: TestA11yTestFile);

            _testSubject.WriteOutput(_optionsMock.Object, scanResults, null);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ScanResultsMultipleErrors_NoPatterns_NoProperties_VerbosityIsVerbose_WritesBannerAndSummaryAndDetails()
        {
            SetOptions(verbosityLevel: VerbosityLevel.Verbose);
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
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);
            WindowScanOutput scanResults = BuildTestScanResults(errorCount: 2, a11yTestFile: TestA11yTestFile);

            _testSubject.WriteOutput(_optionsMock.Object, scanResults, null);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ScanResultsMultipleErrors_TwoPatterns_NoProperties_VerbosityIsVerbose_WritesBannerAndSummaryAndDetails()
        {
            SetOptions(verbosityLevel: VerbosityLevel.Verbose);
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
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);
            WindowScanOutput scanResults = BuildTestScanResults(errorCount: 2, a11yTestFile: TestA11yTestFile,
                patternCount: 2);

            _testSubject.WriteOutput(_optionsMock.Object, scanResults, null);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ScanResultsMultipleErrors_NoPatterns_TwoProperties_VerbosityIsVerbose_WritesBannerAndSummaryAndDetails()
        {
            SetOptions(verbosityLevel: VerbosityLevel.Verbose);
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
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);
            WindowScanOutput scanResults = BuildTestScanResults(errorCount: 2, a11yTestFile: TestA11yTestFile,
                propertyCount: 2);

            _testSubject.WriteOutput(_optionsMock.Object, scanResults, null);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ScanResultsMultipleErrors_TwoPatterns_TwoProperties_VerbosityIsVerbose_WritesBannerAndSummaryAndDetails()
        {
            SetOptions(verbosityLevel: VerbosityLevel.Verbose);
            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorCountGeneralStart, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerboseCountStart, WriteSource.WriteLineTwoParams),
                new WriteCall(FrameworkIssueLinkOneParam, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerbosePropertiesHeaderStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerbosePropertyPairStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerbosePropertyPairStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerbosePatternsHeaderStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerbosePatternIndex, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerbosePatternIndex, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerboseSeparatorStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerboseCountStart, WriteSource.WriteLineTwoParams),
                new WriteCall(FrameworkIssueLinkOneParam, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerbosePropertiesHeaderStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerbosePropertyPairStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerbosePropertyPairStart, WriteSource.WriteLineTwoParams),
                new WriteCall(ErrorVerbosePatternsHeaderStart, WriteSource.WriteLineStringOnly),
                new WriteCall(ErrorVerbosePatternIndex, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerbosePatternIndex, WriteSource.WriteLineOneParam),
                new WriteCall(ErrorVerboseSeparatorStart, WriteSource.WriteLineStringOnly),
                new WriteCall(OutputFileStart, WriteSource.WriteLineOneParam),
            };
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);
            WindowScanOutput scanResults = BuildTestScanResults(errorCount: 2, a11yTestFile: TestA11yTestFile,
                patternCount: 2, propertyCount: 2, frameworkIssueLink: "https://docs.microsoft.com");

            _testSubject.WriteOutput(_optionsMock.Object, scanResults, null);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_ParameterExceptionIsThrown_VerbosityIsQuiet_WritesBannerAndError()
        {
            const string errorMessage = "A parameter was bad";
            SetOptions(verbosityLevel: VerbosityLevel.Quiet);
            Exception caughtException = new ParameterException(errorMessage);
            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(UnableToComplete, WriteSource.WriteStringOnly),
                new WriteCall(errorMessage, WriteSource.WriteLineStringOnly)
            };
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);

            _testSubject.WriteOutput(_optionsMock.Object, null, caughtException);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void WriteOutput_NonParameterExceptionIsThrown_VerbosityIsQuiet_WritesBannerAndError()
        {
            Exception caughtException = new MissingMethodException();
            SetOptions(verbosityLevel: VerbosityLevel.Quiet);
            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(UnableToComplete, WriteSource.WriteStringOnly),
                new WriteCall(ExceptionInfoStart, WriteSource.WriteLineOneParam),
            };
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);

            _testSubject.WriteOutput(_optionsMock.Object, null, caughtException);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }

        [TestMethod]
        //[Timeout(1000)]
        public void WriteOutput_ScanResultsNull_CaughtExceptionNull_VerbosityIsQuiet_WritesBannerAndError()
        {
            SetOptions(verbosityLevel: VerbosityLevel.Quiet);
            WriteCall[] expectedCalls =
            {
                new WriteCall(AppTitleStart, WriteSource.WriteLineOneParam),
                new WriteCall(UnableToComplete, WriteSource.WriteStringOnly),
                new WriteCall(NoFurtherData, WriteSource.WriteLineStringOnly),
            };
            TextWriterVerifier textWriterVerifier = new TextWriterVerifier(_writerMock, expectedCalls);

            _testSubject.WriteOutput(_optionsMock.Object, null, null);

            textWriterVerifier.VerifyAll();
            VerifyAllMocks();
        }
    }
}
