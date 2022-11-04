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
using System.Threading;
using System.Threading.Tasks;

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

        private void InitOutputCallback(WindowScanOutput output)
        {
            _resultsAssemblerMock.Setup(x => x.AssembleWindowScanOutputFromElement(It.IsAny<A11yElement>())).Returns(output);

            WindowScanOutput tempOutput = null;
            _actionsMock.Setup(x => x.Scan(It.IsAny<A11yElement>(), It.IsAny<ScanActionCallback<WindowScanOutput>>(), It.IsAny<IActionContext>()))
                .Callback<A11yElement, ScanActionCallback<WindowScanOutput>, IActionContext>((e, cb, _) => tempOutput = cb(e, Guid.Empty))
                .Returns(() => tempOutput);
        }

        private IReadOnlyList<A11yElement> CreateMockElementArray()
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
        public async Task ExecuteAsync_NullConfig_ThrowsException()
        {
            var action = new Func<Task>(() => SnapshotCommand.ExecuteAsync(config: null, scanTools: _scanToolsMock.Object, cancellationToken: CancellationToken.None));
            var ex = await Assert.ThrowsExceptionAsync<ArgumentNullException>(action);
            Assert.IsTrue(ex.Message.Contains("config"));

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_NullScanTools_ThrowsException()
        {
            var action = new Func<Task>(() => SnapshotCommand.ExecuteAsync(config: _minimalConfig, scanTools: null, cancellationToken: CancellationToken.None));
            var ex = await Assert.ThrowsExceptionAsync<ArgumentNullException>(action);
            Assert.IsTrue(ex.Message.Contains("scanTools"));

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_NullTargetElementLocator_ThrowsException()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns<ITargetElementLocator>(null);
            var action = new Func<Task>(() => SnapshotCommand.ExecuteAsync(_minimalConfig, _scanToolsMock.Object, CancellationToken.None));
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(action);
            Assert.IsTrue(ex.Message.Contains("TargetElementLocator"));

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_NullAxeWindowsActions_ThrowsException()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns<IAxeWindowsActions>(null);

            var action = new Func<Task>(() => SnapshotCommand.ExecuteAsync(_minimalConfig, _scanToolsMock.Object, CancellationToken.None));
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(action);
            Assert.IsTrue(ex.Message.Contains("Actions"));

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_NullDpiAwareness_ThrowsException()
        {
            SetupScanToolsMock(withResultsAssembler: false);
            _scanToolsMock.Setup(x => x.DpiAwareness).Returns<IDPIAwareness>(null);

            var action = new Func<Task>(() => SnapshotCommand.ExecuteAsync(_minimalConfig, _scanToolsMock.Object, CancellationToken.None));
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(action);
            Assert.IsTrue(ex.Message.Contains("DpiAwareness"));

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_TargetElementLocatorReturnsNull_PassesNullToScan()
        {
            SetupScanToolsMock(withResultsAssembler: false);
            _scanToolsMock.Setup(x => x.DpiAwareness).Returns(_dpiAwarenessMock.Object);
            SetupTargetElementLocatorMock(overrideElements: true, elements: null);

            await SnapshotCommand.ExecuteAsync(_minimalConfig, _scanToolsMock.Object, CancellationToken.None);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_TargetElementLocatorReceivesConfigProcessId()
        {
            const int expectedProcessId = 42;

            SetupScanToolsMock(withResultsAssembler: false);
            SetupDpiAwarenessMock(null);
            SetupTargetElementLocatorMock(processId: expectedProcessId);

            _actionsMock.Setup(x => x.Scan(It.IsNotNull<A11yElement>(), It.IsNotNull<ScanActionCallback<WindowScanOutput>>(), It.IsAny<IActionContext>())).Returns<WindowScanOutput>(null);

            var config = Config.Builder.ForProcessId(expectedProcessId).Build();

            await SnapshotCommand.ExecuteAsync(config, _scanToolsMock.Object, CancellationToken.None);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_ActionsScanWithDpiAwarenessObject_IsCalledWithExpectedElements()
        {
            int index = 0;
            var elements = CreateMockElementArray();
            SetupScanToolsMock(withResultsAssembler: false);
            SetupDpiAwarenessMock("This is an arbitrary object");
            SetupTargetElementLocatorMock(overrideElements: true, elements: elements);

            _actionsMock.Setup(x => x.Scan(
                It.IsAny<A11yElement>(), It.IsAny<ScanActionCallback<WindowScanOutput>>(), It.IsAny<IActionContext>())).Returns<WindowScanOutput>(null)
                .Callback<A11yElement, ScanActionCallback<WindowScanOutput>, IActionContext>((element, _, __) =>
                {
                    Assert.AreSame(element, elements[index++]);
                });

            await SnapshotCommand.ExecuteAsync(_minimalConfig, _scanToolsMock.Object, CancellationToken.None);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_ActionsScan_IsCalledWithExpectedElements()
        {
            int index = 0;
            var elements = CreateMockElementArray();
            SetupScanToolsMock(withResultsAssembler: false);
            SetupDpiAwarenessMock(null);
            SetupTargetElementLocatorMock(overrideElements: true, elements: elements);
            _actionsMock.Setup(x => x.Scan(
                It.IsAny<A11yElement>(), It.IsAny<ScanActionCallback<WindowScanOutput>>(), It.IsAny<IActionContext>())).Returns<WindowScanOutput>(null)
                .Callback<A11yElement, ScanActionCallback<WindowScanOutput>, IActionContext>((element, _, __) =>
                {
                    Assert.AreSame(element, elements[index++]);
                });

            await SnapshotCommand.ExecuteAsync(_minimalConfig, _scanToolsMock.Object, CancellationToken.None);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_ReturnsExpectedResults_SingleRoot()
        {
            var elements = CreateMockElementArray().ToArray()[0..1];
            SetupScanToolsMock();
            SetupDpiAwarenessMock(null);
            SetupTargetElementLocatorMock(overrideElements: true, elements: elements);

            var expectedWindowOutput = new WindowScanOutput();
            InitOutputCallback(expectedWindowOutput);

            var actualOutput = await SnapshotCommand.ExecuteAsync(_minimalConfig, _scanToolsMock.Object, CancellationToken.None);
            foreach (var actualWindowOutput in actualOutput.WindowScanOutputs)
            {
                Assert.AreEqual(expectedWindowOutput, actualWindowOutput);
            }

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_ReturnsExpectedResults_MultiRoot()
        {
            SetupScanToolsMock(withOutputFileHelper: true);
            SetupTargetElementLocatorMock();
            SetupDpiAwarenessMock(null);
            SetupActionsMock();
            SetupOutputFileHelperMock();

            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .Build();

            var expectedWindowOutput = new WindowScanOutput();
            InitOutputCallback(expectedWindowOutput);

            var actualOutput = await SnapshotCommand.ExecuteAsync(config, _scanToolsMock.Object, CancellationToken.None);
            foreach (var actualWindowOutput in actualOutput.WindowScanOutputs)
            {
                Assert.AreEqual(expectedWindowOutput, actualWindowOutput);
            }

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_NullResultsAssembler_ThrowsException()
        {
            SetupScanToolsMock(withResultsAssembler: false);
            SetupTargetElementLocatorMock();
            _scanToolsMock.Setup(x => x.ResultsAssembler).Returns<IScanResultsAssembler>(null);
            SetupDpiAwarenessMock(null);

            WindowScanOutput tempOutput = null;
            _actionsMock.Setup(x => x.Scan(It.IsAny<A11yElement>(), It.IsAny<ScanActionCallback<WindowScanOutput>>(), It.IsAny<IActionContext>()))
                .Callback<A11yElement, ScanActionCallback<WindowScanOutput>, IActionContext>((e, cb, _) => tempOutput = cb(e, Guid.Empty))
                .Returns(() => tempOutput);

            var action = new Func<Task>(() => SnapshotCommand.ExecuteAsync(_minimalConfig, _scanToolsMock.Object, CancellationToken.None));
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(action);
            Assert.IsTrue(ex.Message.Contains("ResultsAssembler"));

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_NoErrors_SingleRootNoOutputFiles()
        {
            var elements = CreateMockElementArray().ToArray()[0..1];
            SetupScanToolsMock();
            SetupDpiAwarenessMock(null);
            SetupTargetElementLocatorMock(overrideElements: true, elements: elements);

            var expectedWindowOutput = new WindowScanOutput
            {
                ErrorCount = 0
            };
            InitOutputCallback(expectedWindowOutput);

            // In addition to throwing an ArgumentNullException
            // The following call would cause mock exceptions for IAxeWindowsActions.CaptureScreenshot and IAxeWindowsActions.SaveA11yTestFile.
            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .Build();

            var actualOutput = await SnapshotCommand.ExecuteAsync(config, _scanToolsMock.Object, CancellationToken.None);
            Assert.IsNull(actualOutput.WindowScanOutputs.First().OutputFile.A11yTest);
            Assert.AreEqual(1, actualOutput.WindowScanOutputs.Count);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_NoErrors_MultiRootHasOutputFiles()
        {
            SetupScanToolsMock(withOutputFileHelper: true);
            SetupTargetElementLocatorMock();
            SetupDpiAwarenessMock(null);
            SetupActionsMock();
            SetupOutputFileHelperMock();

            var expectedOutput = new WindowScanOutput
            {
                ErrorCount = 0
            };
            InitOutputCallback(expectedOutput);

            _resultsAssemblerMock.Setup(x => x.AssembleWindowScanOutputFromElement(It.IsAny<A11yElement>())).Returns(() => new WindowScanOutput() { ErrorCount = 1 });

            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .Build();

            var actualOutput = await SnapshotCommand.ExecuteAsync(config, _scanToolsMock.Object, CancellationToken.None);
            var expectedPaths = new string[]
            {
                "Test.File_1_of_3",
                "Test.File_2_of_3",
                "Test.File_3_of_3",
            };
            Assert.AreEqual(1, actualOutput.WindowScanOutputs.First().ErrorCount);
            var fileNames = actualOutput.WindowScanOutputs.Select(result => result.OutputFile.A11yTest).OrderBy(x => x).ToArray();
            for (int i = 0; i < expectedPaths.Length; i++)
            {
                Assert.AreEqual(expectedPaths[i], fileNames[i]);
            }

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_NullOutputFileHelper_ThrowsException()
        {
            SetupScanToolsMock();
            _scanToolsMock.Setup(x => x.OutputFileHelper).Returns<IOutputFileHelper>(null);
            SetupTargetElementLocatorMock();
            SetupDpiAwarenessMock(null);
            var expectedOutput = new WindowScanOutput
            {
                ErrorCount = 1
            };
            InitOutputCallback(expectedOutput);

            var action = new Func<Task>(() => SnapshotCommand.ExecuteAsync(_minimalConfig, _scanToolsMock.Object, CancellationToken.None));

            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(action);
            Assert.IsTrue(ex.Message.Contains("OutputFileHelper"));

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_NullOutputFilePath_ThrowsException()
        {
            SetupScanToolsMock(withOutputFileHelper: true);
            SetupTargetElementLocatorMock();
            SetupDpiAwarenessMock(null);
            SetupActionsMock(expectedPath: null);
            SetupOutputFileHelperMock(filePath: "null");

            var expectedOutput = new WindowScanOutput
            {
                ErrorCount = 1
            };
            InitOutputCallback(expectedOutput);

            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .Build();

            var action = new Func<Task>(() => SnapshotCommand.ExecuteAsync(config, _scanToolsMock.Object, CancellationToken.None));

            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(action);
            Assert.IsTrue(ex.Message.Contains("a11yTestOutputFile"));

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_WithErrors_SingleFileNamedCorrectly()
        {
            const string expectedPath = "Test.File";
            var elements = CreateMockElementArray().ToArray()[0..1];

            SetupScanToolsMock(withOutputFileHelper: true);
            SetupDpiAwarenessMock(null);
            SetupActionsMock(expectedPath: expectedPath);
            SetupOutputFileHelperMock(filePath: null);
            SetupTargetElementLocatorMock(overrideElements: true, elements: elements);
            _outputFileHelperMock.Setup(x => x.GetNewA11yTestFilePath(It.IsAny<Func<string, string>>())).Returns((Func<string, string> decorator) => decorator == null ? expectedPath : decorator("Test.File"));

            var expectedOutput = new WindowScanOutput
            {
                ErrorCount = 75
            };
            InitOutputCallback(expectedOutput);

            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .Build();

            var actualOutput = await SnapshotCommand.ExecuteAsync(config, _scanToolsMock.Object, CancellationToken.None);
            Assert.AreEqual(1, actualOutput.WindowScanOutputs.Count);
            Assert.AreEqual(75, actualOutput.WindowScanOutputs.First().ErrorCount);
            Assert.AreEqual(expectedPath, actualOutput.WindowScanOutputs.First().OutputFile.A11yTest);

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_WithErrors_MultipleFilesNamedCorrectly()
        {
            SetupScanToolsMock(withOutputFileHelper: true);
            SetupTargetElementLocatorMock();
            SetupDpiAwarenessMock(null);
            SetupActionsMock();
            SetupOutputFileHelperMock();

            var expectedOutput = new WindowScanOutput
            {
                ErrorCount = 75
            };
            InitOutputCallback(expectedOutput);

            _resultsAssemblerMock.Setup(x => x.AssembleWindowScanOutputFromElement(It.IsAny<A11yElement>())).Returns(() => new WindowScanOutput() { ErrorCount = 1 });

            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .Build();

            var actualOutput = await SnapshotCommand.ExecuteAsync(config, _scanToolsMock.Object, CancellationToken.None);
            var expectedPaths = new string[]
            {
                "Test.File_1_of_3",
                "Test.File_2_of_3",
                "Test.File_3_of_3",
            };
            Assert.AreEqual(1, actualOutput.WindowScanOutputs.First().ErrorCount);
            var fileNames = actualOutput.WindowScanOutputs.Select(result => result.OutputFile.A11yTest).OrderBy(x => x).ToArray();
            for (int i = 0; i < expectedPaths.Length; i++)
            {
                Assert.AreEqual(expectedPaths[i], fileNames[i]);
            }

            VerifyAllMocks();
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task ExecuteAsync_CustomUIAPropertyConfig_RegisterCustomUIAPropertiesCalled()
        {
            const string configPath = "test.json";
            var elements = CreateMockElementArray().ToArray()[0..1];

            SetupScanToolsMock();
            SetupDpiAwarenessMock(null);
            SetupTargetElementLocatorMock(overrideElements: true, elements: elements);

            var expectedOutput = new WindowScanOutput();
            InitOutputCallback(expectedOutput);

            _actionsMock.Setup(x => x.RegisterCustomUIAPropertiesFromConfig(configPath));

            var config = Config.Builder
                .ForProcessId(-1)
                .WithCustomUIAConfig(configPath)
                .Build();

            var actualOutput = await SnapshotCommand.ExecuteAsync(config, _scanToolsMock.Object, CancellationToken.None);

            VerifyAllMocks();
        }
    } // class
} // namespace
