// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Axe.Windows.AutomationTests
{
    [TestClass]
    public class ConfigTests
    {
        private const int TestProcessId = 12345;
        private Config.Builder _builder;

        [TestInitialize]
        public void BeforeEach()
        {
            _builder = Config.Builder.ForProcessId(TestProcessId);
        }

        private void AssertBuiltValues(string outputDirectory = null,
            IDPIAwareness dpiAwareness = null, OutputFileFormat? outputFileFormat = null,
            string customUIAConfig = null, bool multipleScanRoots = false)
        {
            Config config = _builder.Build();

            OutputFileFormat finalFileFormat = outputFileFormat ?? OutputFileFormat.None;
            Assert.AreEqual(TestProcessId, config.ProcessId);
            Assert.AreEqual(outputDirectory, config.OutputDirectory);
            Assert.AreEqual(dpiAwareness, config.DPIAwareness);
            Assert.AreEqual(finalFileFormat, config.OutputFileFormat);
            Assert.AreEqual(customUIAConfig, config.CustomUIAConfigPath);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Builder_JustProcessId_DefaultConfig()
        {
            AssertBuiltValues();
        }

        [TestMethod]
        [Timeout(1000)]
        public void Builder_WithOutputDirectory_SetsOutputDirectory()
        {
            const string outputDirectory = "This is a placeholder for the output directory";
            _builder.WithOutputDirectory(outputDirectory);
            AssertBuiltValues(outputDirectory: outputDirectory);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Builder_WithDPIAwareness_SetsDPIAwareness()
        {
            Mock<IDPIAwareness> dpiAwarenessMock = new Mock<IDPIAwareness>(MockBehavior.Strict);
            _builder.WithDPIAwareness(dpiAwarenessMock.Object);
            AssertBuiltValues(dpiAwareness: dpiAwarenessMock.Object);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Builder_WithOutputFileFormat_SetsOutputFileFormat()
        {
            const OutputFileFormat testFormat = OutputFileFormat.A11yTest;
            _builder.WithOutputFileFormat(testFormat);
            AssertBuiltValues(outputFileFormat: testFormat);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Builder_WithCustomUIAConfig_SetsCustomUIAConfig()
        {
            const string testUIAConfig = "This is a placeholder for the UIA config";
            _builder.WithCustomUIAConfig(testUIAConfig);
            AssertBuiltValues(customUIAConfig: testUIAConfig);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Builder_WithMultipleScanRootsEnabled_SetsMultipleScanRootsEnabled()
        {
            _builder.WithMultipleScanRootsEnabled();
            AssertBuiltValues(multipleScanRoots: true);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Builder_ChainingTest_AllValuesAreSet()
        {
            const string testOutputDirectory = "put tests here";
            const string testUIAConfig = "load uia here";
            const OutputFileFormat testOutputFileFormat = OutputFileFormat.A11yTest;
            Mock<IDPIAwareness> dpiAwarenessMock = new Mock<IDPIAwareness>(MockBehavior.Strict);
            Config.Builder builder = _builder
                .WithCustomUIAConfig(testUIAConfig)
                .WithDPIAwareness(dpiAwarenessMock.Object)
                .WithMultipleScanRootsEnabled()
                .WithOutputDirectory(testOutputDirectory)
                .WithOutputFileFormat(testOutputFileFormat);
            Assert.AreSame(builder, _builder);
            AssertBuiltValues(customUIAConfig: testUIAConfig, dpiAwareness: dpiAwarenessMock.Object,
                multipleScanRoots: true, outputDirectory: testOutputDirectory,
                outputFileFormat: testOutputFileFormat);
        }
    }
}
