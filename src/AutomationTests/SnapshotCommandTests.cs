// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions.Contexts;
using Axe.Windows.Automation;
using Axe.Windows.Core.Bases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.AutomationTests
{
    using WindowScanOutput = WindowScanOutput;

    [TestClass]
    public class SnapshotCommandTests
    {
        private Mock<IScanTools> _scanToolsMock;
        private Mock<ITargetElementLocator> _targetElementLocatorMock;
        private Mock<IAxeWindowsActions> _actionsMock;
        private Mock<IDPIAwareness> _dpiAwarenessMock;
        private Mock<IOutputFileHelper> _outputFileHelperMock;
        private Mock<IScanResultsAssembler> _resultsAssemblerMock;
        private readonly Config _minimalConfig;

        public SnapshotCommandTests()
        {
            _minimalConfig = Config.Builder.ForProcessId(-1).Build();
        }

        [TestInitialize]
        public void TestInit()
        {
            _scanToolsMock = new Mock<IScanTools>(MockBehavior.Strict);
            _targetElementLocatorMock = new Mock<ITargetElementLocator>(MockBehavior.Strict);
            _actionsMock = new Mock<IAxeWindowsActions>(MockBehavior.Strict);
            _dpiAwarenessMock = new Mock<IDPIAwareness>(MockBehavior.Strict);
            _outputFileHelperMock = new Mock<IOutputFileHelper>(MockBehavior.Strict);
            _resultsAssemblerMock = new Mock<IScanResultsAssembler>(MockBehavior.Strict);
        }

        private void VerifyAllMocks()
        {
            _scanToolsMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
            _actionsMock.VerifyAll();
            _dpiAwarenessMock.VerifyAll();
            _outputFileHelperMock.VerifyAll();
            _resultsAssemblerMock.VerifyAll();
        }

        private void InitResultsCallback(WindowScanOutput results)
        {
            _resultsAssemblerMock.Setup(x => x.AssembleWindowScanOutputFromElement(It.IsAny<A11yElement>())).Returns(results);

            WindowScanOutput tempResults = null;
            _actionsMock.Setup(x => x.Scan(It.IsAny<A11yElement>(), It.IsAny<ScanActionCallback<WindowScanOutput>>(), It.IsAny<IActionContext>()))
                .Callback<A11yElement, ScanActionCallback<WindowScanOutput>, IActionContext>((e, cb, _) => tempResults = cb(e, Guid.Empty))
                .Returns(() => tempResults);
        }

        private IEnumerable<A11yElement> CreateMockElementArray()
        {
            var elements = new List<A11yElement>();

            for (int i = 0; i < 3; ++i)
                elements.Add(new A11yElement());

            return elements;
        }

        private void SetupDpiAwarenessMock(object dataFromEnable)
        {
            _dpiAwarenessMock.Setup(x => x.Enable()).Returns(dataFromEnable);
            _dpiAwarenessMock.Setup(x => x.Restore(dataFromEnable));
            _scanToolsMock.Setup(x => x.DpiAwareness).Returns(_dpiAwarenessMock.Object);
        }

        private void SetupScanToolsMock(bool withResultsAssembler = true, bool withOutputFileHelper = false)
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            if (withResultsAssembler)
            {
                _scanToolsMock.Setup(x => x.ResultsAssembler).Returns(_resultsAssemblerMock.Object);
            }
            if (withOutputFileHelper)
            {
                _scanToolsMock.Setup(x => x.OutputFileHelper).Returns(_outputFileHelperMock.Object);
            }
        }

        private void SetupActionsMock(string expectedPath = "")
        {
            _actionsMock.Setup(x => x.CaptureScreenshot(It.IsAny<Guid>(), It.IsAny<IActionContext>()));
            switch (expectedPath)
            {
                case null:
                    // Don't set the mock
                    break;
                case "":
                    // Accept any string
                    _actionsMock.Setup(x => x.SaveA11yTestFile(It.IsAny<string>(), It.IsAny<A11yElement>(), It.IsAny<Guid>(), It.IsAny<IActionContext>()));
                    break;
                default:
                    // Accept only the expected path
                    _actionsMock.Setup(x => x.SaveA11yTestFile(expectedPath, It.IsAny<A11yElement>(), It.IsAny<Guid>(), It.IsAny<IActionContext>()));
                    break;
            }
        }

        private void SetupTargetElementLocatorMock(int processId = -1, bool overrideElements = false, IEnumerable<A11yElement> elements = null)
        {
            var overriddenElements = overrideElements ? elements : CreateMockElementArray();
            _targetElementLocatorMock.Setup(x => x.LocateRootElements(processId, It.IsAny<IActionContext>())).Returns(overriddenElements);
        }

        private void SetupOutputFileHelperMock(string filePath = "Test.File")
        {
            _outputFileHelperMock.Setup(x => x.EnsureOutputDirectoryExists());
            switch (filePath)
            {
                case null:
                    // Don't set the mock
                    break;
                case "null":
                    // Return null
                    _outputFileHelperMock.Setup(x => x.GetNewA11yTestFilePath(It.IsAny<Func<string, string>>())).Returns((string)null);
                    break;
                default:
                    // set filePath
                    _outputFileHelperMock.Setup(x => x.GetNewA11yTestFilePath(It.IsAny<Func<string, string>>())).Returns((Func<string, string> decorator) => decorator(filePath));
                    break;
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NullConfig_ThrowsException()
        {
            var action = new Action(() => SnapshotCommand.Execute(config: null, scanTools: _scanToolsMock.Object));
            var ex = Assert.ThrowsException<ArgumentNullException>(action);
            Assert.IsTrue(ex.Message.Contains("config"));

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NullScanTools_ThrowsException()
        {
            var action = new Action(() => SnapshotCommand.Execute(config: _minimalConfig, scanTools: null));
            var ex = Assert.ThrowsException<ArgumentNullException>(action);
            Assert.IsTrue(ex.Message.Contains("scanTools"));

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NullTargetElementLocator_ThrowsException()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns<ITargetElementLocator>(null);
            var action = new Action(() => SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object));
            var ex = Assert.ThrowsException<ArgumentException>(action);
            Assert.IsTrue(ex.Message.Contains("TargetElementLocator"));

            VerifyAllMocks();
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

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NullDpiAwareness_ThrowsException()
        {
            SetupScanToolsMock(withResultsAssembler: false);
            _scanToolsMock.Setup(x => x.DpiAwareness).Returns<IDPIAwareness>(null);

            var action = new Action(() => SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object));
            var ex = Assert.ThrowsException<ArgumentException>(action);
            Assert.IsTrue(ex.Message.Contains("DpiAwareness"));

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_TargetElementLocatorReturnsNull_PassesNullToScan()
        {
            SetupScanToolsMock(withResultsAssembler: false);
            _scanToolsMock.Setup(x => x.DpiAwareness).Returns(_dpiAwarenessMock.Object);
            SetupTargetElementLocatorMock(overrideElements: true, elements: null);

            SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_TargetElementLocatorReceivesConfigProcessId()
        {
            const int expectedProcessId = 42;

            SetupScanToolsMock(withResultsAssembler: false);
            SetupDpiAwarenessMock(null);
            SetupTargetElementLocatorMock(processId: expectedProcessId);

            _actionsMock.Setup(x => x.Scan(It.IsNotNull<A11yElement>(), It.IsNotNull<ScanActionCallback<WindowScanOutput>>(), It.IsAny<IActionContext>())).Returns<WindowScanOutput>(null);

            var config = Config.Builder.ForProcessId(expectedProcessId).Build();

            SnapshotCommand.Execute(config, _scanToolsMock.Object);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_ActionsScanWithDpiAwarenessObject_IsCalledWithExpectedElements()
        {
            var elements = CreateMockElementArray();
            SetupScanToolsMock(withResultsAssembler: false);
            SetupDpiAwarenessMock("This is an arbitrary object");
            SetupTargetElementLocatorMock(overrideElements: true, elements: elements);

            _actionsMock.Setup(x => x.Scan(
                elements.First(), It.IsAny<ScanActionCallback<WindowScanOutput>>(), It.IsAny<IActionContext>())).Returns<WindowScanOutput>(null);

            SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_ActionsScan_IsCalledWithExpectedElements()
        {
            var elements = CreateMockElementArray();
            SetupScanToolsMock(withResultsAssembler: false);
            SetupDpiAwarenessMock(null);
            SetupTargetElementLocatorMock(overrideElements: true, elements: elements);
            _actionsMock.Setup(x => x.Scan(
                elements.First(), It.IsAny<ScanActionCallback<WindowScanOutput>>(), It.IsAny<IActionContext>())).Returns<WindowScanOutput>(null);

            SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_ReturnsExpectedResults_SingleRoot()
        {
            var elements = CreateMockElementArray().ToArray()[0..1];
            SetupScanToolsMock();
            SetupDpiAwarenessMock(null);
            SetupTargetElementLocatorMock(overrideElements: true, elements: elements);

            var expectedResults = new WindowScanOutput();
            InitResultsCallback(expectedResults);

            var actualResults = SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object);
            foreach (var actualResult in actualResults)
            {
                Assert.AreEqual(expectedResults, actualResult);
            }

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_ReturnsExpectedResults_MultiRoot()
        {
            SetupScanToolsMock(withOutputFileHelper: true);
            SetupTargetElementLocatorMock();
            SetupDpiAwarenessMock(null);
            SetupActionsMock();
            SetupOutputFileHelperMock();

            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .WithMultipleScanRootsEnabled()
                .Build();

            var expectedResults = new WindowScanOutput();
            InitResultsCallback(expectedResults);

            var actualResults = SnapshotCommand.Execute(config, _scanToolsMock.Object);
            foreach (var actualResult in actualResults)
            {
                Assert.AreEqual(expectedResults, actualResult);
            }

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NullResultsAssembler_ThrowsException()
        {
            SetupScanToolsMock(withResultsAssembler: false);
            SetupTargetElementLocatorMock();
            _scanToolsMock.Setup(x => x.ResultsAssembler).Returns<IScanResultsAssembler>(null);
            SetupDpiAwarenessMock(null);

            WindowScanOutput tempResults = null;
            _actionsMock.Setup(x => x.Scan(It.IsAny<A11yElement>(), It.IsAny<ScanActionCallback<WindowScanOutput>>(), It.IsAny<IActionContext>()))
                .Callback<A11yElement, ScanActionCallback<WindowScanOutput>, IActionContext>((e, cb, _) => tempResults = cb(e, Guid.Empty))
                .Returns(() => tempResults);

            var action = new Action(() => SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object));
            var ex = Assert.ThrowsException<ArgumentException>(action);
            Assert.IsTrue(ex.Message.Contains("ResultsAssembler"));

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NoErrors_SingleRootNoOutputFiles()
        {
            var elements = CreateMockElementArray().ToArray()[0..1];
            SetupScanToolsMock();
            SetupDpiAwarenessMock(null);
            SetupTargetElementLocatorMock(overrideElements: true, elements: elements);

            var expectedResults = new WindowScanOutput
            {
                ErrorCount = 0
            };
            InitResultsCallback(expectedResults);

            // In addition to throwing an ArgumentNullException
            // The following call would cause mock exceptions for IAxeWindowsActions.CaptureScreenshot and IAxeWindowsActions.SaveA11yTestFile.
            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .Build();

            var actualResults = SnapshotCommand.Execute(config, _scanToolsMock.Object);
            Assert.IsNull(actualResults.First().OutputFile.A11yTest);
            Assert.AreEqual(1, actualResults.Count);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NoErrors_MultiRootHasOutputFiles()
        {
            SetupScanToolsMock(withOutputFileHelper: true);
            SetupTargetElementLocatorMock();
            SetupDpiAwarenessMock(null);
            SetupActionsMock();
            SetupOutputFileHelperMock();

            var expectedResults = new WindowScanOutput
            {
                ErrorCount = 0
            };
            InitResultsCallback(expectedResults);

            _resultsAssemblerMock.Setup(x => x.AssembleWindowScanOutputFromElement(It.IsAny<A11yElement>())).Returns(() => new WindowScanOutput() { ErrorCount = 1 });

            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .WithMultipleScanRootsEnabled()
                .Build();

            var actualResults = SnapshotCommand.Execute(config, _scanToolsMock.Object);
            var expectedPaths = new string[]
            {
                "Test.File_1_of_3",
                "Test.File_2_of_3",
                "Test.File_3_of_3",
            };
            Assert.AreEqual(1, actualResults.First().ErrorCount);
            var fileNames = actualResults.Select(result => result.OutputFile.A11yTest).OrderBy(x => x).ToArray();
            for (int i = 0; i < expectedPaths.Length; i++)
            {
                Assert.AreEqual(expectedPaths[i], fileNames[i]);
            }

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NullOutputFileHelper_ThrowsException()
        {
            SetupScanToolsMock();
            _scanToolsMock.Setup(x => x.OutputFileHelper).Returns<IOutputFileHelper>(null);
            SetupTargetElementLocatorMock();
            SetupDpiAwarenessMock(null);
            var expectedResults = new WindowScanOutput
            {
                ErrorCount = 1
            };
            InitResultsCallback(expectedResults);

            var action = new Action(() => SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object));

            var ex = Assert.ThrowsException<ArgumentException>(action);
            Assert.IsTrue(ex.Message.Contains("OutputFileHelper"));

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NullOutputFilePath_ThrowsException()
        {
            SetupScanToolsMock(withOutputFileHelper: true);
            SetupTargetElementLocatorMock();
            SetupDpiAwarenessMock(null);
            SetupActionsMock(expectedPath: null);
            SetupOutputFileHelperMock(filePath: "null");

            var expectedResults = new WindowScanOutput
            {
                ErrorCount = 1
            };
            InitResultsCallback(expectedResults);

            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .Build();

            var action = new Action(() => SnapshotCommand.Execute(config, _scanToolsMock.Object));

            var ex = Assert.ThrowsException<InvalidOperationException>(action);
            Assert.IsTrue(ex.Message.Contains("a11yTestOutputFile"));

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_WithErrors_SingleFileNamedCorrectly()
        {
            const string expectedPath = "Test.File";
            var elements = CreateMockElementArray().ToArray()[0..1];

            SetupScanToolsMock(withOutputFileHelper: true);
            SetupDpiAwarenessMock(null);
            SetupActionsMock(expectedPath: expectedPath);
            SetupOutputFileHelperMock(filePath: null);
            SetupTargetElementLocatorMock(overrideElements: true, elements: elements);
            _outputFileHelperMock.Setup(x => x.GetNewA11yTestFilePath(It.IsAny<Func<string, string>>())).Returns((Func<string, string> decorator) => decorator == null ? expectedPath : decorator("Test.File"));

            var expectedResults = new WindowScanOutput
            {
                ErrorCount = 75
            };
            InitResultsCallback(expectedResults);

            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .Build();

            var actualResults = SnapshotCommand.Execute(config, _scanToolsMock.Object);
            Assert.AreEqual(1, actualResults.Count);
            Assert.AreEqual(75, actualResults.First().ErrorCount);
            Assert.AreEqual(expectedPath, actualResults.First().OutputFile.A11yTest);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_WithErrors_MultipleFilesNamedCorrectly()
        {
            SetupScanToolsMock(withOutputFileHelper: true);
            SetupTargetElementLocatorMock();
            SetupDpiAwarenessMock(null);
            SetupActionsMock();
            SetupOutputFileHelperMock();

            var expectedResults = new WindowScanOutput
            {
                ErrorCount = 75
            };
            InitResultsCallback(expectedResults);

            _resultsAssemblerMock.Setup(x => x.AssembleWindowScanOutputFromElement(It.IsAny<A11yElement>())).Returns(() => new WindowScanOutput() { ErrorCount = 1 });

            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .WithMultipleScanRootsEnabled()
                .Build();

            var actualResults = SnapshotCommand.Execute(config, _scanToolsMock.Object);
            var expectedPaths = new string[]
            {
                "Test.File_1_of_3",
                "Test.File_2_of_3",
                "Test.File_3_of_3",
            };
            Assert.AreEqual(1, actualResults.First().ErrorCount);
            var fileNames = actualResults.Select(result => result.OutputFile.A11yTest).OrderBy(x => x).ToArray();
            for (int i = 0; i < expectedPaths.Length; i++)
            {
                Assert.AreEqual(expectedPaths[i], fileNames[i]);
            }

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_CustomUIAPropertyConfig_RegisterCustomUIAPropertiesCalled()
        {
            const string configPath = "test.json";
            var elements = CreateMockElementArray().ToArray()[0..1];

            SetupScanToolsMock();
            SetupDpiAwarenessMock(null);
            SetupTargetElementLocatorMock(overrideElements: true, elements: elements);

            var expectedResults = new WindowScanOutput();
            InitResultsCallback(expectedResults);

            _actionsMock.Setup(x => x.RegisterCustomUIAPropertiesFromConfig(configPath));

            var config = Config.Builder
                .ForProcessId(-1)
                .WithCustomUIAConfig(configPath)
                .Build();

            var actualResults = SnapshotCommand.Execute(config, _scanToolsMock.Object);

            VerifyAllMocks();
        }
    } // class
} // namespace
