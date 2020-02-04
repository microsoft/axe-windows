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

        readonly string WildlifeManagerAppPath = Path.GetFullPath("../../../../../tools/WildlifeManager/WildlifeManager.exe");
        readonly string Win32ControlSamplerAppPath = Path.GetFullPath("../../../../../tools/Win32ControlSampler/Win32ControlSampler.exe");
        readonly string WindowsFormsControlSamplerAppPath = Path.GetFullPath("../../../../../tools/WindowsFormsControlSampler/WindowsFormsControlSampler.exe");
        readonly string WpfControlSamplerAppPath = Path.GetFullPath("../../../../../tools/WpfControlSampler/WpfControlSampler.exe");

        readonly string OutputDir = Path.GetFullPath("./TestOutput");
        readonly string ValidationAppFolder;
        readonly string ValidationApp;

        const string OldFileVersionCompatibilityAppName = "OldFileVersionCompatibilityTests";
        readonly string OldFileVersionCompatibilityAppFolder;
        readonly string OldFileVersionCompatibilityAppPath;

        private readonly TimeSpan testAppDelay;
        private readonly bool allowInconclusive;

        public AutomationIntegrationTests()
        {
            ValidationAppFolder = GetProjectOutputFolder("CurrentFileVersionCompatibilityTests");
            ValidationApp = Path.Combine(ValidationAppFolder, @"CurrentFileVersionCompatibilityTests.exe");

            OldFileVersionCompatibilityAppFolder = Path.Combine(GetProjectOutputFolder(OldFileVersionCompatibilityAppName), "netcoreapp3.0");
            OldFileVersionCompatibilityAppPath = Path.Combine(OldFileVersionCompatibilityAppFolder, OldFileVersionCompatibilityAppName + ".exe");

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

        private static string GetProjectOutputFolder(string projectName)
        {
            return Path.GetFullPath(
                Path.Combine(Directory.GetCurrentDirectory(), $@"../../../../{projectName}/bin",
#if DEBUG
                    "debug"
#else
                    "release"
#endif
                ));
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

            var output = ScanWithProvisionForBuildAgents(scanner);

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

        private ScanResults ScanWithProvisionForBuildAgents(IScanner scanner)
        {
            try
            {
                return scanner.Scan();
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
            TestProcess?.Kill();
            TestProcess = null;
        }

        private void CleanupTestOutput()
        {
            if (Directory.Exists(OutputDir))
                Directory.Delete(OutputDir, true);
        }

        [TestMethod]
        public void EnsureA11yTestFileV1_0IsStillReadable()
        {
            EnsureOldFileIsReadableByCurrentVersionOfAxeWindows("ValidateFileVersion_0_1_0");
        }

        [TestMethod]
        public void EnsureA11yTestFileV2_0IsStillReadable()
        {
            EnsureOldFileIsReadableByCurrentVersionOfAxeWindows("ValidateFileVersion_0_2_0");
        }

        [TestMethod]
        public void EnsureA11yTestFileV3_1IsStillReadable()
        {
            EnsureOldFileIsReadableByCurrentVersionOfAxeWindows("ValidateFileVersion_0_3_1");
        }

        /// <summary>
        /// Ensure old a11ytest file versions are readable by the current version of Axe.Windows
        /// </summary>
        /// <remarks>
        /// This test method and the associated console app are needed because the unit test methods
        /// in OldFileVersionCompatibilityTests cannot be run using a .Net Core unit test app.
        /// 
        /// There is a failure to load Microsoft.IntelliTrace.Core.dll in that case.
        /// The problem appears to be unsolvable in our code at this time.
        /// </remarks>
        /// <param name="testRunnerMethodName"></param>
        private void EnsureOldFileIsReadableByCurrentVersionOfAxeWindows(string testRunnerMethodName)
        {
            Assert.IsTrue(File.Exists(OldFileVersionCompatibilityAppPath), OldFileVersionCompatibilityAppPath + " was not found");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = OldFileVersionCompatibilityAppPath,
                Arguments = testRunnerMethodName,
                WorkingDirectory = OldFileVersionCompatibilityAppFolder
            };

            Process app = Process.Start(startInfo);
            if (app.WaitForExit(10000))
            {
                Assert.AreEqual(0, app.ExitCode);
                return;
            }

            app.Kill();

            Assert.Fail(OldFileVersionCompatibilityAppName + " was still running after 10 seconds");
        }
    }
}
