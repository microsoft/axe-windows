// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation;
using Axe.Windows.Core.Bases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Axe.Windows.AutomationTests
{
    using ScanResults = ScanResults;

    [TestClass]
    public class SnapshotCommandUnitTests
    {
        private MockRepository _mockRepo;
        private Mock<IScanTools> _scanToolsMock;
        private Mock<ITargetElementLocator> _targetElementLocatorMock;
        private Mock<IAxeWindowsActions> _actionsMock;
        private Mock<INativeMethods> _nativeMethodsMock;
        private Mock<IOutputFileHelper> _outputFileHelperMock;
        private Mock<IScanResultsAssembler> _resultsAssemblerMock;
        private Config _minimalConfig;

        public SnapshotCommandUnitTests()
        {
            _mockRepo = new MockRepository(MockBehavior.Strict) { DefaultValue = DefaultValue.Mock };
            _minimalConfig = Config.Builder.ForProcessId(-1).Build();
        }

        [TestInitialize]
        public void TestInit()
        {
            _scanToolsMock = _mockRepo.Create<IScanTools>();
            _targetElementLocatorMock = _mockRepo.Create<ITargetElementLocator>();
            _actionsMock = _mockRepo.Create<IAxeWindowsActions>();
            _nativeMethodsMock = _mockRepo.Create<INativeMethods>();
            _nativeMethodsMock.Setup(x => x.SetProcessDPIAware()).Returns(false);
            _outputFileHelperMock = _mockRepo.Create<IOutputFileHelper>();
            _resultsAssemblerMock = _mockRepo.Create<IScanResultsAssembler>();
        }

        private void InitResultsCallback(ScanResults results)
        {
            _resultsAssemblerMock.Setup(x => x.AssembleScanResultsFromElement(It.IsAny<A11yElement>())).Returns(results);

            ScanResults tempResults = null;
            _actionsMock.Setup(x => x.Scan(It.IsAny<A11yElement>(), It.IsAny<ScanActionCallback<ScanResults>>()))
                .Callback<A11yElement, ScanActionCallback<ScanResults>>((e, cb) => tempResults = cb(e, Guid.Empty))
                .Returns(() => tempResults);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NullConfig_ThrowsException()
        {
            var action = new Action(() => SnapshotCommand.Execute(config: null, scanTools: _scanToolsMock.Object));
            var ex = Assert.ThrowsException<ArgumentNullException>(action);
            Assert.IsTrue(ex.Message.Contains("config"));
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NullScanTools_ThrowsException()
        {
            var action = new Action(() => SnapshotCommand.Execute(config: _minimalConfig, scanTools: null));
            var ex = Assert.ThrowsException<ArgumentNullException>(action);
            Assert.IsTrue(ex.Message.Contains("scanTools"));
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NullTargetElementLocator_ThrowsException()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns<ITargetElementLocator>(null);
            var action = new Action(() => SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object));
            var ex = Assert.ThrowsException<ArgumentException>(action);
            Assert.IsTrue(ex.Message.Contains("TargetElementLocator"));
            _scanToolsMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NullAxeWindowsActions_ThrowsException()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns<IAxeWindowsActions>(null);

            var action = new Action(() => SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object));
            var ex = Assert.ThrowsException<ArgumentException>(action);
            Assert.IsTrue(ex.Message.Contains("Actions"));
            _scanToolsMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NullNativeMethods_ThrowsException()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns<INativeMethods>(null);

            var action = new Action(() => SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object));
            var ex = Assert.ThrowsException<ArgumentException>(action);
            Assert.IsTrue(ex.Message.Contains("NativeMethods"));
            _scanToolsMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_TargetElementLocatorReturnsNull_PassesNullToScan()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns(_nativeMethodsMock.Object);

            _targetElementLocatorMock.Setup(x => x.LocateRootElement(It.IsAny<int>())).Returns<A11yElement>(null);
            _actionsMock.Setup(x => x.Scan(null, It.IsNotNull<ScanActionCallback<ScanResults>>())).Returns<ScanResults>(null);

            SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object);

            _scanToolsMock.VerifyAll();
            _nativeMethodsMock.VerifyAll();
            _actionsMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_TargetElementLocatorReceivesConfigProcessId()
        {
            const int expectedProcessId = 42;

            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns(_nativeMethodsMock.Object);

            _targetElementLocatorMock.Setup(x => x.LocateRootElement(expectedProcessId)).Returns<A11yElement>(null);
            _actionsMock.Setup(x => x.Scan(null, It.IsNotNull<ScanActionCallback<ScanResults>>())).Returns<ScanResults>(null);

            var config = Config.Builder.ForProcessId(expectedProcessId).Build();

            SnapshotCommand.Execute(config, _scanToolsMock.Object);

            _scanToolsMock.VerifyAll();
            _nativeMethodsMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
            _actionsMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_ActionsScan_IsCalledWithExpectedElement()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns(_nativeMethodsMock.Object);

            var element = new A11yElement();
            _targetElementLocatorMock.Setup(x => x.LocateRootElement(It.IsAny<int>())).Returns(element);

            _actionsMock.Setup(x => x.Scan(element, It.IsAny<ScanActionCallback<ScanResults>>())).Returns<ScanResults>(null);

            SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object);

            _scanToolsMock.VerifyAll();
            _nativeMethodsMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
            _actionsMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_ReturnsExpectedResults()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns(_nativeMethodsMock.Object);
            _scanToolsMock.Setup(x => x.ResultsAssembler).Returns(_resultsAssemblerMock.Object);

            _targetElementLocatorMock.Setup(x => x.LocateRootElement(It.IsAny<int>())).Returns(new A11yElement());

            var expectedResults = new ScanResults();
            InitResultsCallback(expectedResults);

            var actualResults = SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object);
            Assert.AreEqual(expectedResults, actualResults);

            _scanToolsMock.VerifyAll();
            _nativeMethodsMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
            _actionsMock.VerifyAll();
            _resultsAssemblerMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NullResultsAssembler_ThrowsException()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns(_nativeMethodsMock.Object);
            _scanToolsMock.Setup(x => x.ResultsAssembler).Returns<IScanResultsAssembler>(null);

            _targetElementLocatorMock.Setup(x => x.LocateRootElement(It.IsAny<int>())).Returns(new A11yElement());

            ScanResults tempResults = null;
            _actionsMock.Setup(x => x.Scan(It.IsAny<A11yElement>(), It.IsAny<ScanActionCallback<ScanResults>>()))
                .Callback<A11yElement, ScanActionCallback<ScanResults>>((e, cb) => tempResults = cb(e, Guid.Empty))
                .Returns(() => tempResults);

            var action = new Action(() => SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object));
            var ex = Assert.ThrowsException<ArgumentException>(action);
            Assert.IsTrue(ex.Message.Contains("ResultsAssembler"));

            _scanToolsMock.VerifyAll();
            _nativeMethodsMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
            _actionsMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NoErrors_NoOutputFiles()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns(_nativeMethodsMock.Object);
            _scanToolsMock.Setup(x => x.ResultsAssembler).Returns(_resultsAssemblerMock.Object);

            _targetElementLocatorMock.Setup(x => x.LocateRootElement(It.IsAny<int>())).Returns(new A11yElement());

            var expectedResults = new ScanResults();
            expectedResults.ErrorCount = 0;
            InitResultsCallback(expectedResults);

            // In addition to throwing an ArgumentNullException
            // The following call would cause mock exceptions for IAxeWindowsActions.CaptureScreenshot and IAxeWindowsActions.SaveA11yTestFile.
            // 
            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .Build();

            var actualResults = SnapshotCommand.Execute(config, _scanToolsMock.Object);
            Assert.IsNull(actualResults.OutputFile.A11yTest);

            _scanToolsMock.VerifyAll();
            _nativeMethodsMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
            _actionsMock.VerifyAll();
            _resultsAssemblerMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NullOutputFileHelper_ThrowsException()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns(_nativeMethodsMock.Object);
            _scanToolsMock.Setup(x => x.ResultsAssembler).Returns(_resultsAssemblerMock.Object);
            _scanToolsMock.Setup(x => x.OutputFileHelper).Returns<IOutputFileHelper>(null);
            _targetElementLocatorMock.Setup(x => x.LocateRootElement(It.IsAny<int>())).Returns(new A11yElement());
            var expectedResults = new ScanResults();
            expectedResults.ErrorCount = 1;
            InitResultsCallback(expectedResults);
            var action = new Action(() => SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object));
            var ex = Assert.ThrowsException<ArgumentException>(action);
            Assert.IsTrue(ex.Message.Contains("OutputFileHelper"));
            _scanToolsMock.VerifyAll();
            _nativeMethodsMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
            _actionsMock.VerifyAll();
            _resultsAssemblerMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NullOutputFilePath_ThrowsException()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns(_nativeMethodsMock.Object);
            _scanToolsMock.Setup(x => x.ResultsAssembler).Returns(_resultsAssemblerMock.Object);
            _scanToolsMock.Setup(x => x.OutputFileHelper).Returns(_outputFileHelperMock.Object);

            _targetElementLocatorMock.Setup(x => x.LocateRootElement(It.IsAny<int>())).Returns(new A11yElement());

            var expectedResults = new ScanResults();
            expectedResults.ErrorCount = 1;
            InitResultsCallback(expectedResults);

            _actionsMock.Setup(x => x.CaptureScreenshot(It.IsAny<Guid>()));

            _outputFileHelperMock.Setup(m => m.EnsureOutputDirectoryExists());
            _outputFileHelperMock.Setup(x => x.GetNewA11yTestFilePath()).Returns<string>(null);

            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .Build();

            var action = new Action(() => SnapshotCommand.Execute(config, _scanToolsMock.Object));
            var ex = Assert.ThrowsException<InvalidOperationException>(action);
            Assert.IsTrue(ex.Message.Contains("a11yTestOutputFile"));

            _scanToolsMock.VerifyAll();
            _nativeMethodsMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
            _outputFileHelperMock.VerifyAll();
            _actionsMock.VerifyAll();
            _resultsAssemblerMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_WithErrors_CreatesSnapshotAndA11yTestFile()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns(_nativeMethodsMock.Object);
            _scanToolsMock.Setup(x => x.ResultsAssembler).Returns(_resultsAssemblerMock.Object);
            _scanToolsMock.Setup(x => x.OutputFileHelper).Returns(_outputFileHelperMock.Object);

            _targetElementLocatorMock.Setup(x => x.LocateRootElement(It.IsAny<int>())).Returns(new A11yElement());

            var expectedResults = new ScanResults();
            expectedResults.ErrorCount = 75;
            InitResultsCallback(expectedResults);

            var expectedPath = "Test.file";

            _actionsMock.Setup(x => x.CaptureScreenshot(It.IsAny<Guid>()));
            _actionsMock.Setup(x => x.SaveA11yTestFile(expectedPath, It.IsAny<A11yElement>(), It.IsAny<Guid>()));

            _outputFileHelperMock.Setup(m => m.EnsureOutputDirectoryExists());
            _outputFileHelperMock.Setup(x => x.GetNewA11yTestFilePath()).Returns(expectedPath);

            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .Build();

            var actualResults = SnapshotCommand.Execute(config, _scanToolsMock.Object);
            Assert.AreEqual(75, actualResults.ErrorCount);
            Assert.AreEqual(expectedPath, actualResults.OutputFile.A11yTest);

            _scanToolsMock.VerifyAll();
            _nativeMethodsMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
            _actionsMock.VerifyAll();
            _resultsAssemblerMock.VerifyAll();
            _outputFileHelperMock.VerifyAll();
        }
    } // class
} // namespace
