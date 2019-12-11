// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation;
using Axe.Windows.UnitTestSharedLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
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
        const int WildlifeManagerKnownErrorCount = 13;
        const int Win32ControlSamplerKnownErrorCount = 0;
        const int WindowsFormsControlSamplerKnownErrorCount = 6;
        const int WpfControlSamplerKnownErrorCount = 5;

        readonly string WildlifeManagerAppPath = Path.GetFullPath("../../../../tools/WildlifeManager/WildlifeManager.exe");
        readonly string Win32ControlSamplerAppPath = Path.GetFullPath("../../../../tools/Win32ControlSampler/Win32ControlSampler.exe");
        readonly string WindowsFormsControlSamplerAppPath = Path.GetFullPath("../../../../tools/WindowsFormsControlSampler/WindowsFormsControlSampler.exe");
        readonly string WpfControlSamplerAppPath = Path.GetFullPath("../../../../tools/WpfControlSampler/WpfControlSampler.exe");

        readonly string OutputDir = Path.GetFullPath("./TestOutput");
        readonly string ValidationAppFolder;
        readonly string ValidationApp;

        // Build agents need more than than local dev machines to have the test app
        // up and running. Pipelines set BUILD_BUILDID, dev machines don't
        private TimeSpan testAppDelay = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BUILD_BUILDID")) ?
            TimeSpan.FromSeconds(2) : TimeSpan.FromSeconds(10);

        public AutomationIntegrationTests()
        {
            ValidationAppFolder = Path.GetFullPath(
                Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\CurrentFileVersionCompatibilityTests\bin",
#if DEBUG
                    "debug"
#else
                    "release"
#endif
                ));
            ValidationApp = Path.Combine(ValidationAppFolder, @"CurrentFileVersionCompatibilityTests.exe");
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

        [TestMethod]
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
        public void Scan_Integration_WpfControlSampler()
        {
            Scan_Integration_Core(WpfControlSamplerAppPath, WpfControlSamplerKnownErrorCount);
        }

        private ScanResults Scan_Integration_Core(string testAppPath, int expectedErrorCount)
        {
            LaunchTestApp(testAppPath);
            var config = Config.Builder.ForProcessId(TestProcess.Id)
                .WithOutputDirectory(OutputDir)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .Build();

            var scanner = ScannerFactory.CreateScanner(config);

            var output = scanner.Scan();

            // Validate for consistency
            Assert.AreEqual(expectedErrorCount, output.ErrorCount);
            Assert.AreEqual(expectedErrorCount, output.Errors.Count());

            if (expectedErrorCount > 0)
            {
                var regexForExpectedFile = $"{OutputDir.Replace("\\", "\\\\")}.*\\.a11ytest";

                // Validate the output file exists where it is expected
                Assert.IsTrue(Regex.IsMatch(output.OutputFile.A11yTest, regexForExpectedFile));
                Assert.IsTrue(File.Exists(output.OutputFile.A11yTest));

                // Validate that we got some properties and patterns
                Assert.IsTrue(output.Errors.All(error => error.Element.Properties != null));
                Assert.IsTrue(output.Errors.All(error => error.Element.Patterns != null));
            }
            else
            {
                Assert.IsNull(output.OutputFile.A11yTest);
            }

            return output;
        }

        private void EnsureGeneratedFileIsReadableByOldVersionsOfAxeWindows(ScanResults scanResults, int processId)
        {
            Assert.IsTrue(File.Exists(ValidationApp), ValidationApp + " was not found");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = ValidationApp,
                Arguments = string.Format(@"""{0}"" {1} {2}",
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

        private void CleanupTestOutput() => Directory.Delete(OutputDir, true);
    }
}
