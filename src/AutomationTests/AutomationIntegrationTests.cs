// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation;
using Axe.Windows.UnitTestSharedLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Axe.Windows.AutomationTests
{
    [TestClass, TestCategory(TestCategory.Integration)]
    public class AutomationIntegrationTests
    {
        // These values should change only in response to intentional rule modifications
        const int WildlifeManagerKnownErrorCount = 12;
        const int Win32ControlSamplerKnownErrorCount = 0;
        const int WindowsFormsControlSamplerKnownErrorCount = 4;
        const int WpfControlSamplerKnownErrorCount = 5;
        const int WindowsFormsMultiWindowSamplerAppAllErrorCount = 8;
        const int WindowsFormsMultiWindowSamplerSingleWindowAllErrorCount = 4;

        readonly string WildlifeManagerAppPath = Path.GetFullPath("../../../../../tools/WildlifeManager/WildlifeManager.exe");
        readonly string Win32ControlSamplerAppPath = Path.GetFullPath("../../../../../tools/Win32ControlSampler/Win32ControlSampler.exe");
        readonly string WindowsFormsControlSamplerAppPath = Path.GetFullPath("../../../../../tools/WindowsFormsControlSampler/WindowsFormsControlSampler.exe");
        readonly string WindowsFormsMultiWindowSamplerAppPath = Path.GetFullPath("../../../../../tools/WindowsFormsMultiWindowSample/WindowsFormsMultiWindowSample.exe");
        readonly string WpfControlSamplerAppPath = Path.GetFullPath("../../../../../tools/WpfControlSampler/WpfControlSampler.exe");

        readonly string OutputDir = Path.GetFullPath("./TestOutput");
        readonly string ValidationAppFolder;
        readonly string ValidationApp;

        private readonly TimeSpan testAppDelay;
        private readonly bool allowInconclusive;

        public AutomationIntegrationTests()
        {
            ValidationAppFolder = Path.GetFullPath(
                Path.Combine(Directory.GetCurrentDirectory(), @"../../../../CurrentFileVersionCompatibilityTests/bin",
#if DEBUG
                    "debug"
#else
                    "release"
#endif
                ));
            ValidationApp = Path.Combine(ValidationAppFolder, @"CurrentFileVersionCompatibilityTests.exe");

            // Build agents are less predictable than dev machines. Set the flags based
            // on the BUILD_BUILDID environment variable (only set on build agents)
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BUILD_BUILDID")))
            {
                // Dev machine: Require tests with minimal timeout
                testAppDelay = TimeSpan.FromSeconds(2);
                allowInconclusive = false;
            }
            else
            {
                // Pipeline machine: Allow inconclusive tests, longer timeout
                testAppDelay = TimeSpan.FromSeconds(10);
                allowInconclusive = true;
            }
        }

        Process TestProcess;

        [TestCleanup]
        public void Cleanup()
        {
            StopTestApp();

            CleanupTestOutput();
        }

        [TestMethod]
        [Timeout(30000)]
        public void Scan_Integration_WildlifeManager()
        {
            ScanResults results = Scan_Integration_Core(WildlifeManagerAppPath, WildlifeManagerKnownErrorCount);
            EnsureGeneratedFileIsReadableByOldVersionsOfAxeWindows(results, TestProcess.Id);
        }

        // [TestMethod]
        [Timeout(30000)]
        public void Scan_Integration_Win32ControlSampler()
        {
            Scan_Integration_Core(Win32ControlSamplerAppPath, Win32ControlSamplerKnownErrorCount);
        }

        [TestMethod]
        [Timeout(30000)]
        public void Scan_Integration_WindowsFormsControlSampler()
        {
            Scan_Integration_Core(WindowsFormsControlSamplerAppPath, WindowsFormsControlSamplerKnownErrorCount);
        }

        [TestMethod]
        [Timeout(30000)]
        public void Scan_Integration_WindowsFormsMultiWindowSample()
        {
            Scan_Integration_Core(WindowsFormsMultiWindowSamplerAppPath, WindowsFormsMultiWindowSamplerAppAllErrorCount, expectedWindowCount : 2);
        }

        [TestMethod]
        [Timeout(30000)]
        public void Scan_Integration_WindowsFormsMultiWindowSample_SingleWindow()
        {
            Scan_Integration_Core(WindowsFormsMultiWindowSamplerAppPath, WindowsFormsMultiWindowSamplerSingleWindowAllErrorCount, false);
        }

        [TestMethod]
        [Timeout(30000)]
        public void Scan_Integration_WpfControlSampler()
        {
            Scan_Integration_Core(WpfControlSamplerAppPath, WpfControlSamplerKnownErrorCount);
        }

        [TestMethod]
        [Timeout(30000)]
        public void SingleWindowScan_MultipleRootsEnabledThrows()
        {
            LaunchTestApp(WindowsFormsMultiWindowSamplerAppPath);
            var config = Config.Builder.ForProcessId(TestProcess.Id)
                .WithOutputDirectory(OutputDir)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .WithMultipleScanRootsEnabled()
                .Build();
            var scanner = ScannerFactory.CreateScanner(config);

            var action = new Action(() => scanner.Scan());
            Assert.ThrowsException<InvalidOperationException>(action);
        }

        [TestMethod]
        [Timeout(30000)]
        public void SingleWindowScanWithID_MultipleRootsEnabledThrows()
        {
            LaunchTestApp(WindowsFormsMultiWindowSamplerAppPath);
            var config = Config.Builder.ForProcessId(TestProcess.Id)
                .WithOutputDirectory(OutputDir)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .WithMultipleScanRootsEnabled()
                .Build();
            var scanner = ScannerFactory.CreateScanner(config);

            var action = new Action(() => scanner.Scan("TestIDForThrow"));
            Assert.ThrowsException<InvalidOperationException>(action);
        }

        private ScanResults Scan_Integration_Core(string testAppPath, int expectedErrorCount, bool enableMultipleScanRoots = true, int expectedWindowCount = 1)
        {
            LaunchTestApp(testAppPath);
            var builder = Config.Builder.ForProcessId(TestProcess.Id)
                .WithOutputDirectory(OutputDir)
                .WithOutputFileFormat(OutputFileFormat.A11yTest);

            if (enableMultipleScanRoots)
            {
                builder = builder.WithMultipleScanRootsEnabled();
            }

            var config = builder.Build();

            var scanner = ScannerFactory.CreateScanner(config);

            var output = ScanWithProvisionForBuildAgents(scanner);

            // Validate for consistency
            Assert.AreEqual(expectedWindowCount, output.Count);
            Assert.AreEqual(expectedErrorCount, output.Sum(x => x.ErrorCount));
            Assert.AreEqual(expectedErrorCount, output.Sum(x => x.Errors.Count()));

            if (expectedErrorCount > 0)
            {
                var regexForExpectedFile = $"{OutputDir.Replace("\\", "\\\\")}.*\\.a11ytest";

                // Validate the output file exists where it is expected
                Assert.IsTrue(Regex.IsMatch(output.First().OutputFile.A11yTest, regexForExpectedFile));
                Assert.IsTrue(File.Exists(output.First().OutputFile.A11yTest));

                // Validate that we got some properties and patterns
                Assert.IsTrue(output.First().Errors.All(error => error.Element.Properties != null));
                Assert.IsTrue(output.First().Errors.All(error => error.Element.Patterns != null));
            }
            else
            {
                Assert.IsNull(output.First().OutputFile.A11yTest);
            }

            return output.First();
        }

        private IReadOnlyCollection<ScanResults> ScanWithProvisionForBuildAgents(IScanner scanner)
        {
            try
            {
                return scanner.ScanAll();
            }
            catch (AxeWindowsAutomationException e)
            {
                if (allowInconclusive && e.Message.Contains("Automation017"))
                {
                    Assert.Inconclusive("Unable to complete Integration tests");
                }
                throw;
            }
        }

        private void EnsureGeneratedFileIsReadableByOldVersionsOfAxeWindows(ScanResults scanResults, int processId)
        {
            Assert.IsTrue(File.Exists(ValidationApp), ValidationApp + " was not found");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = ValidationApp,
                Arguments = string.Format(CultureInfo.InvariantCulture, @"""{0}"" {1} {2}",
                    scanResults.OutputFile.A11yTest, scanResults.ErrorCount, processId),
                WorkingDirectory = ValidationAppFolder
            };

            Process testApp = Process.Start(startInfo);
            if (testApp.WaitForExit(10000))
            {
                Assert.AreEqual(0, testApp.ExitCode);
            }
            else
            {
                testApp.Kill();
                Assert.Fail("Test app was still running after 10 seconds");
            }
        }

        private void LaunchTestApp(string testAppPath)
        {
            TestProcess?.Kill();
            TestProcess = Process.Start(testAppPath);
            TestProcess.WaitForInputIdle();

            Thread.Sleep(testAppDelay);
        }

        private void StopTestApp()
        {
            TestProcess.Kill();
            TestProcess = null;
        }

        private void CleanupTestOutput() {
            if (Directory.Exists(OutputDir))
                Directory.Delete(OutputDir, true);
        }
    }
}
