// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions;
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
    using ScanResults = ScanResults;

    [TestClass]
    public class SnapshotCommandTests
    {
        private readonly MockRepository _mockRepo;
        private Mock<IScanTools> _scanToolsMock;
        private Mock<ITargetElementLocator> _targetElementLocatorMock;
        private Mock<IAxeWindowsActions> _actionsMock;
        private Mock<INativeMethods> _nativeMethodsMock;
        private Mock<IOutputFileHelper> _outputFileHelperMock;
        private Mock<IScanResultsAssembler> _resultsAssemblerMock;
        private readonly Config _minimalConfig;

        public SnapshotCommandTests()
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
            _actionsMock.Setup(x => x.Scan(It.IsAny<A11yElement>(), It.IsAny<ScanActionCallback<ScanResults>>(), It.IsAny<IActionContext>()))
                .Callback<A11yElement, ScanActionCallback<ScanResults>, IActionContext>((e, cb, _) => tempResults = cb(e, Guid.Empty))
                .Returns(() => tempResults);
        }

        private IEnumerable<A11yElement> CreateMockElementArray()
        {
            var elements = new List<A11yElement>();

            for (int i = 0; i < 3; ++i)
                elements.Add(new A11yElement());

            return elements;
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

            _targetElementLocatorMock.Setup(x => x.LocateRootElements(It.IsAny<int>(), It.IsAny<IActionContext>())).Returns<IEnumerable<A11yElement>>(null);

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

            _targetElementLocatorMock.Setup(x => x.LocateRootElements(expectedProcessId, It.IsAny<IActionContext>())).Returns(CreateMockElementArray());
            _actionsMock.Setup(x => x.Scan(It.IsNotNull<A11yElement>(), It.IsNotNull<ScanActionCallback<ScanResults>>(), It.IsAny<IActionContext>())).Returns<ScanResults>(null);

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

            var elements = CreateMockElementArray();
            _targetElementLocatorMock.Setup(x => x.LocateRootElements(It.IsAny<int>(), It.IsAny<IActionContext>())).Returns(elements);
            _actionsMock.Setup(x => x.Scan(
                elements.First(), It.IsAny<ScanActionCallback<ScanResults>>(), It.IsAny<IActionContext>())).Returns<ScanResults>(null);

            SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object);

            _scanToolsMock.VerifyAll();
            _nativeMethodsMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
            _actionsMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_ReturnsExpectedResults_SingleRoot()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns(_nativeMethodsMock.Object);
            _scanToolsMock.Setup(x => x.ResultsAssembler).Returns(_resultsAssemblerMock.Object);

            _targetElementLocatorMock.Setup(x => x.LocateRootElements(It.IsAny<int>(), It.IsAny<IActionContext>())).Returns(CreateMockElementArray().ToArray()[0..1]);

            var expectedResults = new ScanResults();
            InitResultsCallback(expectedResults);

            var actualResults = SnapshotCommand.Execute(_minimalConfig, _scanToolsMock.Object);
            foreach (var actualResult in actualResults)
            {
                Assert.AreEqual(expectedResults, actualResult);
            }

            _scanToolsMock.VerifyAll();
            _nativeMethodsMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
            _actionsMock.VerifyAll();
            _resultsAssemblerMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_ReturnsExpectedResults_MultiRoot()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns(_nativeMethodsMock.Object);
            _scanToolsMock.Setup(x => x.ResultsAssembler).Returns(_resultsAssemblerMock.Object);
            _scanToolsMock.Setup(x => x.OutputFileHelper).Returns(_outputFileHelperMock.Object);

            _actionsMock.Setup(x => x.CaptureScreenshot(It.IsAny<Guid>(), It.IsAny<IActionContext>()));
            _actionsMock.Setup(x => x.SaveA11yTestFile(It.IsAny<string>(), It.IsAny<A11yElement>(), It.IsAny<Guid>(), It.IsAny<IActionContext>()));
            _targetElementLocatorMock.Setup(x => x.LocateRootElements(It.IsAny<int>(), It.IsAny<IActionContext>())).Returns(CreateMockElementArray());
            _outputFileHelperMock.Setup(m => m.EnsureOutputDirectoryExists());
            _outputFileHelperMock.Setup(x => x.GetNewA11yTestFilePath(It.IsAny<Func<string, string>>())).Returns((Func<string, string> decorator) => decorator("Test.File"));


            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .WithMultipleScanRootsEnabled()
                .Build();

            var expectedResults = new ScanResults();
            InitResultsCallback(expectedResults);

            var actualResults = SnapshotCommand.Execute(config, _scanToolsMock.Object);
            foreach (var actualResult in actualResults)
            {
                Assert.AreEqual(expectedResults, actualResult);
            }

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

            _targetElementLocatorMock.Setup(x => x.LocateRootElements(It.IsAny<int>(), It.IsAny<IActionContext>())).Returns(CreateMockElementArray());

            ScanResults tempResults = null;
            _actionsMock.Setup(x => x.Scan(It.IsAny<A11yElement>(), It.IsAny<ScanActionCallback<ScanResults>>(), It.IsAny<IActionContext>()))
                .Callback<A11yElement, ScanActionCallback<ScanResults>, IActionContext>((e, cb, _) => tempResults = cb(e, Guid.Empty))
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
        public void Execute_NoErrors_SingleRootNoOutputFiles()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns(_nativeMethodsMock.Object);
            _scanToolsMock.Setup(x => x.ResultsAssembler).Returns(_resultsAssemblerMock.Object);

            _targetElementLocatorMock.Setup(x => x.LocateRootElements(It.IsAny<int>(), It.IsAny<IActionContext>())).Returns(CreateMockElementArray().ToArray()[0..1]);

            var expectedResults = new ScanResults
            {
                ErrorCount = 0
            };
            InitResultsCallback(expectedResults);

            // In addition to throwing an ArgumentNullException
            // The following call would cause mock exceptions for IAxeWindowsActions.CaptureScreenshot and IAxeWindowsActions.SaveA11yTestFile.
            //
            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .Build();

            var actualResults = SnapshotCommand.Execute(config, _scanToolsMock.Object);
            Assert.IsNull(actualResults.First().OutputFile.A11yTest);
            Assert.AreEqual(1, actualResults.Count);

            _scanToolsMock.VerifyAll();
            _nativeMethodsMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
            _actionsMock.VerifyAll();
            _resultsAssemblerMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_NoErrors_MultiRootHasOutputFiles()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns(_nativeMethodsMock.Object);
            _scanToolsMock.Setup(x => x.ResultsAssembler).Returns(_resultsAssemblerMock.Object);
            _scanToolsMock.Setup(x => x.OutputFileHelper).Returns(_outputFileHelperMock.Object);

            _targetElementLocatorMock.Setup(x => x.LocateRootElements(It.IsAny<int>(), It.IsAny<IActionContext>())).Returns(CreateMockElementArray());

            var expectedResults = new ScanResults
            {
                ErrorCount = 0
            };
            InitResultsCallback(expectedResults);


            _actionsMock.Setup(x => x.CaptureScreenshot(It.IsAny<Guid>(), It.IsAny<IActionContext>()));
            _actionsMock.Setup(x => x.SaveA11yTestFile(It.IsAny<string>(), It.IsAny<A11yElement>(), It.IsAny<Guid>(), It.IsAny<IActionContext>()));
            _resultsAssemblerMock.Setup(x => x.AssembleScanResultsFromElement(It.IsAny<A11yElement>())).Returns(() => new ScanResults() { ErrorCount = 1 });
            _outputFileHelperMock.Setup(m => m.EnsureOutputDirectoryExists());
            _outputFileHelperMock.Setup(x => x.GetNewA11yTestFilePath(It.IsAny<Func<string, string>>())).Returns((Func<string, string> decorator) => decorator("Test.File"));

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

            _scanToolsMock.VerifyAll();
            _nativeMethodsMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
            _actionsMock.VerifyAll();
            _resultsAssemblerMock.VerifyAll();
            _outputFileHelperMock.VerifyAll();
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
            _targetElementLocatorMock.Setup(x => x.LocateRootElements(It.IsAny<int>(), It.IsAny<IActionContext>())).Returns(CreateMockElementArray());
            var expectedResults = new ScanResults
            {
                ErrorCount = 1
            };
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

            _targetElementLocatorMock.Setup(x => x.LocateRootElements(It.IsAny<int>(), It.IsAny<IActionContext>())).Returns(CreateMockElementArray());

            var expectedResults = new ScanResults
            {
                ErrorCount = 1
            };
            InitResultsCallback(expectedResults);

            _actionsMock.Setup(x => x.CaptureScreenshot(It.IsAny<Guid>(), It.IsAny<IActionContext>()));

            _outputFileHelperMock.Setup(m => m.EnsureOutputDirectoryExists());
            _outputFileHelperMock.Setup(x => x.GetNewA11yTestFilePath(It.IsAny<Func<string, string>>())).Returns<string>(null);

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
        public void Execute_WithErrors_SingleFileNamedCorrectly()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns(_nativeMethodsMock.Object);
            _scanToolsMock.Setup(x => x.ResultsAssembler).Returns(_resultsAssemblerMock.Object);
            _scanToolsMock.Setup(x => x.OutputFileHelper).Returns(_outputFileHelperMock.Object);

            _targetElementLocatorMock.Setup(x => x.LocateRootElements(It.IsAny<int>(), It.IsAny<IActionContext>())).Returns(CreateMockElementArray().ToArray()[0..1]);

            var expectedResults = new ScanResults
            {
                ErrorCount = 75
            };
            InitResultsCallback(expectedResults);

            var expectedPath = "Test.File";

            _actionsMock.Setup(x => x.CaptureScreenshot(It.IsAny<Guid>(), It.IsAny<IActionContext>()));
            _actionsMock.Setup(x => x.SaveA11yTestFile(expectedPath, It.IsAny<A11yElement>(), It.IsAny<Guid>(), It.IsAny<IActionContext>()));

            _outputFileHelperMock.Setup(m => m.EnsureOutputDirectoryExists());
            _outputFileHelperMock.Setup(x => x.GetNewA11yTestFilePath(It.IsAny<Func<string, string>>())).Returns((Func<string, string> decorator) => decorator == null ? expectedPath : decorator("Test.File"));

            var config = Config.Builder
                .ForProcessId(-1)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .Build();

            var actualResults = SnapshotCommand.Execute(config, _scanToolsMock.Object);
            Assert.AreEqual(1, actualResults.Count);
            Assert.AreEqual(75, actualResults.First().ErrorCount);
            Assert.AreEqual(expectedPath, actualResults.First().OutputFile.A11yTest);

            _scanToolsMock.VerifyAll();
            _nativeMethodsMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
            _actionsMock.VerifyAll();
            _resultsAssemblerMock.VerifyAll();
            _outputFileHelperMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_WithErrors_MultipleFilesNamedCorrectly()
        {
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns(_nativeMethodsMock.Object);
            _scanToolsMock.Setup(x => x.ResultsAssembler).Returns(_resultsAssemblerMock.Object);
            _scanToolsMock.Setup(x => x.OutputFileHelper).Returns(_outputFileHelperMock.Object);

            _targetElementLocatorMock.Setup(x => x.LocateRootElements(It.IsAny<int>(), It.IsAny<IActionContext>())).Returns(CreateMockElementArray());

            var expectedResults = new ScanResults
            {
                ErrorCount = 75
            };
            InitResultsCallback(expectedResults);


            _actionsMock.Setup(x => x.CaptureScreenshot(It.IsAny<Guid>(), It.IsAny<IActionContext>()));
            _actionsMock.Setup(x => x.SaveA11yTestFile(It.IsAny<string>(), It.IsAny<A11yElement>(), It.IsAny<Guid>(), It.IsAny<IActionContext>()));
            _resultsAssemblerMock.Setup(x => x.AssembleScanResultsFromElement(It.IsAny<A11yElement>())).Returns(() => new ScanResults() { ErrorCount = 1 });
            _outputFileHelperMock.Setup(m => m.EnsureOutputDirectoryExists());
            _outputFileHelperMock.Setup(x => x.GetNewA11yTestFilePath(It.IsAny<Func<string, string>>())).Returns((Func<string, string> decorator) => decorator("Test.File"));

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

            _scanToolsMock.VerifyAll();
            _nativeMethodsMock.VerifyAll();
            _targetElementLocatorMock.VerifyAll();
            _actionsMock.VerifyAll();
            _resultsAssemblerMock.VerifyAll();
            _outputFileHelperMock.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Execute_CustomUIAPropertyConfig_RegisterCustomUIAPropertiesCalled()
        {
            string configPath = "test.json";
            _scanToolsMock.Setup(x => x.TargetElementLocator).Returns(_targetElementLocatorMock.Object);
            _scanToolsMock.Setup(x => x.Actions).Returns(_actionsMock.Object);
            _scanToolsMock.Setup(x => x.NativeMethods).Returns(_nativeMethodsMock.Object);
            _scanToolsMock.Setup(x => x.ResultsAssembler).Returns(_resultsAssemblerMock.Object);

            _targetElementLocatorMock.Setup(x => x.LocateRootElements(It.IsAny<int>(), It.IsAny<IActionContext>())).Returns(CreateMockElementArray().ToArray()[0..1]);
            var expectedResults = new ScanResults();
            InitResultsCallback(expectedResults);
            _outputFileHelperMock.Setup(m => m.EnsureOutputDirectoryExists());

            _actionsMock.Setup(x => x.RegisterCustomUIAPropertiesFromConfig(configPath));

            var config = Config.Builder
                .ForProcessId(-1)
                .WithCustomUIAConfig(configPath)
                .Build();

            var actualResults = SnapshotCommand.Execute(config, _scanToolsMock.Object);

            _actionsMock.VerifyAll();
        }
    } // class
} // namespace
