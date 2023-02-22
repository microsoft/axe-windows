// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using Axe.Windows.Automation.Data;
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
using System.Threading.Tasks;

namespace Axe.Windows.AutomationTests
{
    [TestClass, TestCategory(TestCategory.Integration)]
    public class AutomationIntegrationTests
    {
        // These values should change only in response to intentional rule modifications
        const int WildlifeManagerKnownErrorCount = 15;
        const int Win32ControlSamplerKnownErrorCount = 0;
        const int WindowsFormsControlSamplerKnownErrorCount = 6;
        const int WpfControlSamplerKnownErrorCount = 7;
        const int WindowsFormsMultiWindowSamplerAppAllErrorCount = 12;
        // Note: This will be reduced to 1 when we add logic to ignore all but the top level chrome element
        const int WebViewSampleKnownErrorCount = 21;

        readonly string _wildlifeManagerAppPath = Path.GetFullPath("../../../../../tools/WildlifeManager/WildlifeManager.exe");
        readonly string _win32ControlSamplerAppPath = Path.GetFullPath("../../../../../tools/Win32ControlSampler/Win32ControlSampler.exe");
        readonly string _windowsFormsControlSamplerAppPath = Path.GetFullPath("../../../../../tools/WindowsFormsControlSampler/WindowsFormsControlSampler.exe");
        readonly string _windowsFormsMultiWindowSamplerAppPath = Path.GetFullPath("../../../../../tools/WindowsFormsMultiWindowSample/WindowsFormsMultiWindowSample.exe");
        readonly string _wpfControlSamplerAppPath = Path.GetFullPath("../../../../../tools/WpfControlSampler/WpfControlSampler.exe");
        readonly string _webViewSampleAppPath = Path.GetFullPath("../../../../../tools/WebViewSample/WebViewSample.exe");

        readonly string _outputDir = Path.GetFullPath("./TestOutput");
        readonly string _validationAppFolder;
        readonly string _validationApp;

        private readonly TimeSpan _testAppDelay;
        private readonly bool _allowInconclusive;

        public AutomationIntegrationTests()
        {
            _validationAppFolder = Path.GetFullPath(
                Path.Combine(Directory.GetCurrentDirectory(), @"../../../../CurrentFileVersionCompatibilityTests/bin",
#if DEBUG
                    "debug"
#else
                    "release"
#endif
                ));
            _validationApp = Path.Combine(_validationAppFolder, @"CurrentFileVersionCompatibilityTests.exe");

            // Build agents are less predictable than dev machines. Set the flags based
            // on the BUILD_BUILDID environment variable (only set on build agents)
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BUILD_BUILDID")))
            {
                // Dev machine: Require tests with minimal timeout
                _testAppDelay = TimeSpan.FromSeconds(2);
                _allowInconclusive = false;
            }
            else
            {
                // Pipeline machine: Allow inconclusive tests, longer timeout
                _testAppDelay = TimeSpan.FromSeconds(10);
                _allowInconclusive = true;
            }
        }

        readonly List<Process> _testProcesses = new List<Process>();

        [TestCleanup]
        public void Cleanup()
        {
            StopTestApp();

            CleanupTestOutput();
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        [Timeout(30000)]
        public void Scan_Integration_WildlifeManager(bool sync)
        {
            var processId = LaunchTestApp(_wildlifeManagerAppPath);
            WindowScanOutput results = ScanIntegrationCore(sync, _wildlifeManagerAppPath, WildlifeManagerKnownErrorCount, processId: processId);
            EnsureGeneratedFileIsReadableByOldVersionsOfAxeWindows(results, processId);
        }

        // [DataTestMethod]
        // [DataRow(true)]
        // [DataRow(false)]
        [Timeout(30000)]
        public void Scan_Integration_Win32ControlSampler(bool sync)
        {
            ScanIntegrationCore(sync, _win32ControlSamplerAppPath, Win32ControlSamplerKnownErrorCount);
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        [Timeout(30000)]
        public void Scan_Integration_WindowsFormsControlSampler(bool sync)
        {
            ScanIntegrationCore(sync, _windowsFormsControlSamplerAppPath, WindowsFormsControlSamplerKnownErrorCount);
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        [Timeout(30000)]
        public void Scan_Integration_WindowsFormsMultiWindowSample(bool sync)
        {
            ScanIntegrationCore(sync, _windowsFormsMultiWindowSamplerAppPath, WindowsFormsMultiWindowSamplerAppAllErrorCount, 2);
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        [Timeout(30000)]
        public void Scan_Integration_WpfControlSampler(bool sync)
        {
            ScanIntegrationCore(sync, _wpfControlSamplerAppPath, WpfControlSamplerKnownErrorCount);
        }


        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        [Timeout(60000)]
        public void Scan_Integration_WebViewSample(bool sync)
        {
            ScanIntegrationCore(sync, _webViewSampleAppPath, WebViewSampleKnownErrorCount);
        }

        [TestMethod]
        [Timeout(30000)]
        public void ScanAsync_WindowsFormsSampler_TaskIsCancelled_ThrowsCancellationException()
        {
            var processId = LaunchTestApp(_windowsFormsControlSamplerAppPath);
            var builder = Config.Builder.ForProcessId(processId)
                .WithOutputDirectory(_outputDir)
                .WithOutputFileFormat(OutputFileFormat.A11yTest);

            var config = builder.Build();

            var scanner = ScannerFactory.CreateScanner(config);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            var task = scanner.ScanAsync(null, cancellationTokenSource.Token);

            ValidateTaskCancelled(task);
        }

        [TestMethod]
        [Timeout(45000)]
        public async Task ScanAsync_WildlifeManager_MultipleProcesses_RunToCompletion()
        {
            var instanceCount = 3;
            var cancellationTokens = Enumerable.Range(0, instanceCount).Select(_ => new CancellationTokenSource().Token).ToList();
            var tasks = GetAsyncScanTasks(_wildlifeManagerAppPath, cancellationTokens);
            var results = await Task.WhenAll(tasks);

            foreach (var result in results)
            {
                ValidateOutput(result.WindowScanOutputs, WildlifeManagerKnownErrorCount);
            }
        }

        [TestMethod]
        [Timeout(60000)]
        public async Task ScanAsync_WildlifeManager_MultipleProcessesCancelled_ThrowsCancellationException()
        {
            var instanceCount = 5;
            var cancellationTokenSources = Enumerable.Range(0, instanceCount).Select(_ => new CancellationTokenSource()).ToList();
            var tasks = GetAsyncScanTasks(_wildlifeManagerAppPath, cancellationTokenSources.Select(tokenSource => tokenSource.Token));

            var cancelledCount = 2;
            // The first 2 tasks will be cancelled in ~50ms and ~550ms
            var cancelledTasks = tasks.Take(cancelledCount);
            // The final 3 tasks will be cancelled after more than 5 seconds, so we expect them to finish before the cancellation is recognized
            var finishedTasks = tasks.Skip(cancelledCount);

            var timeout = 50;
            for (int i = 0; i < cancelledCount; i++)
            {
                cancellationTokenSources[i].Cancel();
                if (timeout < 10000) // Don't sleep for more than 10 seconds, by then all the scans should be done anyways 
                {
                    Thread.Sleep(timeout);
                    timeout *= 10;
                }
            }

            // Validate cancelled tasks
            foreach (var task in cancelledTasks)
            {
                ValidateTaskCancelled(task);
            }

            // Validate successful tasks
            var results = await Task.WhenAll(finishedTasks);
            foreach (var result in results)
            {
                ValidateOutput(result.WindowScanOutputs, WildlifeManagerKnownErrorCount);
            }
        }

        private WindowScanOutput ScanIntegrationCore(bool sync, string testAppPath, int expectedErrorCount, int expectedWindowCount = 1, int? processId = null)
        {
            if (processId == null)
            {
                processId = LaunchTestApp(testAppPath);
            }
            var builder = Config.Builder.ForProcessId((int)processId)
                .WithOutputDirectory(_outputDir)
                .WithOutputFileFormat(OutputFileFormat.A11yTest);

            var config = builder.Build();

            var scanner = ScannerFactory.CreateScanner(config);

            IReadOnlyCollection<WindowScanOutput> output;
            if (sync)
            {
                output = ScanSyncWithProvisionForBuildAgents(scanner);
            }
            else
            {
                output = ScanAsyncWithProvisionForBuildAgents(scanner);
            }

            return ValidateOutput(output, expectedErrorCount, expectedWindowCount);
        }

        private IEnumerable<Task<ScanOutput>> GetAsyncScanTasks(string testAppPath, IEnumerable<CancellationToken> cancellationTokens)
        {
            var taskFuncs = new List<Func<Task<ScanOutput>>>();

            // Prepare scan tasks
            foreach (var token in cancellationTokens)
            {
                var processId = LaunchTestApp(testAppPath);
                var builder = Config.Builder.ForProcessId(processId)
                    .WithOutputDirectory(_outputDir)
                    .WithOutputFileFormat(OutputFileFormat.A11yTest);

                var config = builder.Build();

                var scanner = ScannerFactory.CreateScanner(config);

                taskFuncs.Add(() => scanner.ScanAsync(null, token));
            }

            // Kick scan tasks off
            var tasks = new List<Task<ScanOutput>>();
            foreach (var func in taskFuncs)
            {
                tasks.Add(func());
            }

            return tasks;
        }

        private WindowScanOutput ValidateOutput(IReadOnlyCollection<WindowScanOutput> output, int expectedErrorCount, int expectedWindowCount = 1)
        {
            Assert.AreEqual(expectedWindowCount, output.Count);
            Assert.AreEqual(expectedErrorCount, output.Sum(x => x.ErrorCount));
            Assert.AreEqual(expectedErrorCount, output.Sum(x => x.Errors.Count()));

            if (expectedErrorCount > 0)
            {
                var regexForExpectedFile = $"{_outputDir.Replace("\\", "\\\\")}.*\\.a11ytest";

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

        private void ValidateTaskCancelled(Task<ScanOutput> task)
        {
            var exception = Assert.ThrowsException<AggregateException>(() => task.Result);
            Assert.AreEqual(typeof(TaskCanceledException), exception.InnerException.GetType());
            Assert.IsTrue(task.IsCanceled);
        }

        private IReadOnlyCollection<WindowScanOutput> ScanSyncWithProvisionForBuildAgents(IScanner scanner)
        {
            try
            {
                return scanner.Scan(null).WindowScanOutputs;
            }
            catch (Exception)
            {
                if (_allowInconclusive)
                {
                    Assert.Inconclusive("Unable to complete Integration tests");
                }
                throw;
            }
        }

        private IReadOnlyCollection<WindowScanOutput> ScanAsyncWithProvisionForBuildAgents(IScanner scanner)
        {
            try
            {
                return scanner.ScanAsync(null, CancellationToken.None).Result.WindowScanOutputs;
            }
            catch (Exception)
            {
                if (_allowInconclusive)
                {
                    Assert.Inconclusive("Unable to complete Integration tests");
                }
                throw;
            }
        }

        private void EnsureGeneratedFileIsReadableByOldVersionsOfAxeWindows(WindowScanOutput scanResults, int processId)
        {
            Assert.IsTrue(File.Exists(_validationApp), _validationApp + " was not found");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = _validationApp,
                Arguments = string.Format(CultureInfo.InvariantCulture, @"""{0}"" {1} {2}",
                    scanResults.OutputFile.A11yTest, scanResults.ErrorCount, processId),
                WorkingDirectory = _validationAppFolder
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

        private int LaunchTestApp(string testAppPath)
        {
            var process = Process.Start(testAppPath);
            process.WaitForInputIdle();
            _testProcesses.Add(process);

            Thread.Sleep(_testAppDelay);

            return process.Id;
        }

        private void StopTestApp()
        {
            foreach (var process in _testProcesses)
            {
                process.Kill();
            }
            _testProcesses.Clear();
        }

        private void CleanupTestOutput()
        {
            if (Directory.Exists(_outputDir))
                Directory.Delete(_outputDir, true);
        }
    }
}
