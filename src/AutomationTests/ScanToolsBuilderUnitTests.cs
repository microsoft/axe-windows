// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Axe.Windows.AutomationTests
{
    [TestClass]
    public class ScanToolsBuilderUnitTests
    {
        [TestMethod]
        [Timeout(1000)]
        public void Build_CreatesExpectedObject()
        {
            const string bitmapCreatorAssembly = @".\WidgetAssembly.dll";

            var mockRepository = new MockRepository(MockBehavior.Strict);

            var actionsMock = mockRepository.Create<IAxeWindowsActions>(MockBehavior.Strict);
            var nativeMethodsMock = mockRepository.Create<INativeMethods>(MockBehavior.Strict);
            var outputFileHelperMock = mockRepository.Create<IOutputFileHelper>(MockBehavior.Strict);
            var resultsAssemblerMock = mockRepository.Create<IScanResultsAssembler>(MockBehavior.Strict);
            var targetElementLocatorMock = mockRepository.Create<ITargetElementLocator>(MockBehavior.Strict);

            var factoryMock = mockRepository.Create<IFactory>(MockBehavior.Strict);
            factoryMock.Setup(x => x.CreateAxeWindowsActions(bitmapCreatorAssembly)).Returns(actionsMock.Object);
            factoryMock.Setup(x => x.CreateNativeMethods()).Returns(nativeMethodsMock.Object);
            factoryMock.Setup(x => x.CreateResultsAssembler()).Returns(resultsAssemblerMock.Object);
            factoryMock.Setup(x => x.CreateTargetElementLocator()).Returns(targetElementLocatorMock.Object);

            string tempString = null;
            factoryMock.Setup(x => x.CreateOutputFileHelper(It.IsAny<string>()))
                .Callback<string>(s => tempString = s)
                .Returns(outputFileHelperMock.Object);

            outputFileHelperMock.Setup(x => x.GetNewA11yTestFilePath())
                .Returns(() => tempString);

            var builder = new ScanToolsBuilder(factoryMock.Object);

            const string expectedPath = @"c:\_TestPath";
            var scanTools = builder
                .WithOutputDirectory(expectedPath)
                .WithBitmapCreatorFrom(bitmapCreatorAssembly)
                .Build();

            Assert.IsNotNull(scanTools);
            Assert.IsNotNull(scanTools.Actions);
            Assert.IsNotNull(scanTools.NativeMethods);
            Assert.IsNotNull(scanTools.OutputFileHelper);
            Assert.IsNotNull(scanTools.ResultsAssembler);
            Assert.IsNotNull(scanTools.TargetElementLocator);

            Assert.AreEqual(expectedPath, scanTools.OutputFileHelper.GetNewA11yTestFilePath());

            factoryMock.VerifyAll();
            outputFileHelperMock.VerifyAll();
        }
    } // class
} // namespace
