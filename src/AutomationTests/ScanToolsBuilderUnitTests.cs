// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Path = System.IO.Path;

namespace Axe.Windows.AutomationTests
{
    [TestClass]
    public class ScanToolsBuilderUnitTests
    {
        [TestMethod]
        [Timeout(1000)]
        public void ScanToolsBuilder_CreatesIOutputFileHelperWithExpectedPath()
        {
            const string testPath = @"c:\_TestPath";

            // #ToDo update this test to use Moq
            var builder = Factory.CreateScanToolsBuilder();
            Assert.IsNotNull(builder);

            var scanTools = builder.WithOutputDirectory(testPath).Build();
            Assert.IsNotNull(scanTools);
            Assert.IsNotNull(scanTools.OutputFileHelper);

            var a11ytestFilePath = scanTools.OutputFileHelper.GetNewA11yTestFilePath();
            Assert.AreEqual(testPath, Path.GetDirectoryName(a11ytestFilePath));
        }

        [TestMethod]
        [Timeout(1000)]
        public void ScanToolsBuilder_CreatesAllObjects()
        {
            // #ToDo use mock with the ScanResultsBuilder class to ensure 
            // all IFactory methods are called
        }
    } // class
} // namespace
