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
            var mockRepository = new MockRepository(MockBehavior.Strict);

            var actionsMock = mockRepository.Create<IAxeWindowsActions>();
            var nativeMethodsMock = mockRepository.Create<INativeMethods>();
            var outputFileHelperMock = mockRepository.Create<IOutputFileHelper>();
            var resultsAssemblerMock = mockRepository.Create<IScanResultsAssembler>();
            var targetElementLocatorMock = mockRepository.Create<ITargetElementLocator>();

            var factoryMock = mockRepository.Create<IFactory>();
            factoryMock.Setup(x => x.CreateAxeWindowsActions()).Returns(actionsMock.Object);
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
            var scanTools = builder.WithOutputDirectory(expectedPath).Build();

            Assert.IsNotNull(scanTools);
            Assert.IsNotNull(scanTools.Actions);
            Assert.IsNotNull(scanTools.NativeMethods);
            Assert.IsNotNull(scanTools.OutputFileHelper);
            Assert.IsNotNull(scanTools.ResultsAssembler);
            Assert.IsNotNull(scanTools.TargetElementLocator);

            Assert.AreEqual(expectedPath, scanTools.OutputFileHelper.GetNewA11yTestFilePath());

            factoryMock.VerifyAll();
        }
    } // class
} // namespace
