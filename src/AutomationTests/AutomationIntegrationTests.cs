// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation;
using Axe.Windows.UnitTestSharedLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        readonly string TestAppPath = Path.GetFullPath("../../../../tools/WildlifeManager/WildlifeManager.exe");
        readonly string OutputDir = Path.GetFullPath("./TestOutput");

        Process TestProcess;

        [TestInitialize]
        public void Startup()
        {
            LaunchTestApp();
        }

        [TestCleanup]
        public void Cleanup()
        {
            StopTestApp();

            CleanupTestOutput();
        }

        [TestMethod]
        [Timeout(17000)]
        public void Scan_Integration()
        {
            var config = Config.Builder.ForProcessId(TestProcess.Id)
                .WithOutputDirectory(OutputDir)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .Build();

            var scanner = ScannerFactory.CreateScanner(config);

            var output = scanner.Scan();

            var regexForExpectedFile = $"{OutputDir.Replace("\\", "\\\\")}.*\\.a11ytest";

            // Validate that we got some errors
            Assert.IsTrue(output.ErrorCount > 0);
            Assert.AreEqual(output.ErrorCount, output.Errors.Count());

            // Validate that we got some properties and patterns
            Assert.IsTrue(output.Errors.All(error => error.Element.Properties != null));
            Assert.IsTrue(output.Errors.All(error => error.Element.Patterns != null));

            // Validate the output file exists where it is expected
            Assert.IsTrue(Regex.IsMatch(output.OutputFile.A11yTest, regexForExpectedFile));
            Assert.IsTrue(File.Exists(output.OutputFile.A11yTest));
        }

        private void LaunchTestApp()
        {
            TestProcess?.Kill();
            TestProcess = Process.Start(TestAppPath);
            TestProcess.WaitForInputIdle();

            // this is painfully long, but the build agents will fail the test without it
            Thread.Sleep(10000);
        }

        private void StopTestApp()
        {
            TestProcess.Kill();
            TestProcess = null;
        }

        private void CleanupTestOutput() => Directory.Delete(OutputDir, true);
    }
}
