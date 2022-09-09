// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Axe.Windows.AutomationTests
{
    [TestClass]
    public class ScanToolsBuilderTests
    {
        [TestMethod]
        [Timeout(1000)]
        public void Build_WithOutputDirectory_CreatesExpectedObject()
        {
            const string expectedPath = @"c:\_TestPath";
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var actionsMock = mockRepository.Create<IAxeWindowsActions>();
            var dpiAwarenessMock = mockRepository.Create<IDPIAwareness>();
            var outputFileHelperMock = mockRepository.Create<IOutputFileHelper>();
            var resultsAssemblerMock = mockRepository.Create<IScanResultsAssembler>();
            var targetElementLocatorMock = mockRepository.Create<ITargetElementLocator>();

            var factoryMock = mockRepository.Create<IFactory>();
            factoryMock.Setup(x => x.CreateAxeWindowsActions()).Returns(actionsMock.Object);
            factoryMock.Setup(x => x.CreateDPIAwareness()).Returns(dpiAwarenessMock.Object);
            factoryMock.Setup(x => x.CreateResultsAssembler()).Returns(resultsAssemblerMock.Object);
            factoryMock.Setup(x => x.CreateTargetElementLocator()).Returns(targetElementLocatorMock.Object);

            string tempString = null;
            factoryMock.Setup(x => x.CreateOutputFileHelper(expectedPath))
                .Callback<string>(s => tempString = s)
                .Returns(outputFileHelperMock.Object);

            outputFileHelperMock.Setup(x => x.GetNewA11yTestFilePath(It.IsAny<Func<string, string>>()))
                .Returns(() => tempString);

            var builder = new ScanToolsBuilder(factoryMock.Object);

            var scanTools = builder
                .WithOutputDirectory(expectedPath)
                .Build();

            Assert.IsNotNull(scanTools);
            Assert.IsNotNull(scanTools.Actions);
            Assert.IsNotNull(scanTools.DpiAwareness);
            Assert.IsNotNull(scanTools.OutputFileHelper);
            Assert.IsNotNull(scanTools.ResultsAssembler);
            Assert.IsNotNull(scanTools.TargetElementLocator);

            Assert.AreEqual(expectedPath, scanTools.OutputFileHelper.GetNewA11yTestFilePath((value) => value));

            factoryMock.VerifyAll();
        }


        [TestMethod]
        [Timeout(1000)]
        public void Build_WithDPIAwareness_CreatesExpectedObject()
        {
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var actionsMock = mockRepository.Create<IAxeWindowsActions>();
            var dpiAwarenessMock = mockRepository.Create<IDPIAwareness>();
            var outputFileHelperMock = mockRepository.Create<IOutputFileHelper>();
            var resultsAssemblerMock = mockRepository.Create<IScanResultsAssembler>();
            var targetElementLocatorMock = mockRepository.Create<ITargetElementLocator>();

            var factoryMock = mockRepository.Create<IFactory>();
            factoryMock.Setup(x => x.CreateAxeWindowsActions()).Returns(actionsMock.Object);
            factoryMock.Setup(x => x.CreateResultsAssembler()).Returns(resultsAssemblerMock.Object);
            factoryMock.Setup(x => x.CreateTargetElementLocator()).Returns(targetElementLocatorMock.Object);

            factoryMock.Setup(x => x.CreateOutputFileHelper(null))
                .Returns(outputFileHelperMock.Object);

            var builder = new ScanToolsBuilder(factoryMock.Object);

            var scanTools = builder
                .WithDPIAwareness(dpiAwarenessMock.Object)
                .Build();

            Assert.IsNotNull(scanTools);
            Assert.IsNotNull(scanTools.Actions);
            Assert.IsNotNull(scanTools.DpiAwareness);
            Assert.IsNotNull(scanTools.OutputFileHelper);
            Assert.IsNotNull(scanTools.ResultsAssembler);
            Assert.IsNotNull(scanTools.TargetElementLocator);

            Assert.AreSame(dpiAwarenessMock.Object, scanTools.DpiAwareness);

            factoryMock.VerifyAll();
        }
    } // class
} // namespace
