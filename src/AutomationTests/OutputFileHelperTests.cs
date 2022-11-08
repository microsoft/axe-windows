// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using Axe.Windows.Automation.Resources;
using Axe.Windows.SystemAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Globalization;
using Path = System.IO.Path;

namespace Axe.Windows.AutomationTests
{
    [TestClass]
    public class OutputFileHelperTests
    {
        private static readonly ISystemDateTime InertDateTime = new Mock<ISystemDateTime>(MockBehavior.Strict).Object;
        private static readonly ISystemEnvironment InertEnvironment = new Mock<ISystemEnvironment>(MockBehavior.Strict).Object;

        // There's no way this test should take 15 seconds, but it often gets flagged as timing out in the pipeline. This is
        // probably just initialization time, but if increasing the timeout reduces pipeline noise, then it's a good change
        [TestMethod]
        [Timeout(15000)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OutputFileHelperCtor_NullSystem_ThrowsException()
        {
            new OutputFileHelper(outputDirectory: null, system: null);
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelperCtor_NullSystemDateTime_ThrowsException()
        {
            var mockSystem = new Mock<ISystem>(MockBehavior.Strict);
            mockSystem.Setup(x => x.DateTime).Returns<ISystemDateTime>(null);

            Action action = () => new OutputFileHelper(outputDirectory: null, system: mockSystem.Object);
            var ex = Assert.ThrowsException<InvalidOperationException>(action);
            Assert.IsTrue(ex.Message.Contains("_dateTime"));

            mockSystem.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelperCtor_NullSystemEnvironment_ThrowsException()
        {
            var mockSystem = new Mock<ISystem>(MockBehavior.Strict);
            mockSystem.Setup(x => x.DateTime).Returns(InertDateTime);
            mockSystem.Setup(x => x.Environment).Returns<ISystemEnvironment>(null);

            Action action = () => new OutputFileHelper(outputDirectory: null, system: mockSystem.Object);
            var ex = Assert.ThrowsException<InvalidOperationException>(action);
            Assert.IsTrue(ex.Message.Contains("environment"));

            mockSystem.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelperCtor_NullSystemDirectory_ThrowsException()
        {
            var mockSystem = new Mock<ISystem>(MockBehavior.Strict);
            var mockIO = new Mock<ISystemIO>(MockBehavior.Strict);
            mockSystem.Setup(x => x.DateTime).Returns(InertDateTime);
            mockSystem.Setup(x => x.Environment).Returns(InertEnvironment);
            mockSystem.Setup(x => x.IO).Returns(mockIO.Object);
            mockIO.Setup(x => x.Directory).Returns<ISystemIODirectory>(null);

            Action action = () => new OutputFileHelper(outputDirectory: null, system: mockSystem.Object);
            var ex = Assert.ThrowsException<InvalidOperationException>(action);
            Assert.IsTrue(ex.Message.Contains("directory"));

            mockSystem.VerifyAll();
            mockIO.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelperCtor_InvalidOutputDirectory_ThrowsException()
        {
            var mockSystem = new Mock<ISystem>(MockBehavior.Strict);
            var mockIO = new Mock<ISystemIO>(MockBehavior.Strict);
            var mockDirectory = new Mock<ISystemIODirectory>(MockBehavior.Strict);
            mockSystem.Setup(x => x.DateTime).Returns(InertDateTime);
            mockSystem.Setup(x => x.Environment).Returns(InertEnvironment);
            mockSystem.Setup(x => x.IO).Returns(mockIO.Object);
            mockIO.Setup(x => x.Directory).Returns(mockDirectory.Object);

            var phonyDirectory = "flub";
            Action action = () => new OutputFileHelper(phonyDirectory, mockSystem.Object);
            var ex = Assert.ThrowsException<AxeWindowsAutomationException>(action);
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, ErrorMessages.ErrorDirectoryInvalid, phonyDirectory), ex.Message);

            mockSystem.VerifyAll();
            mockIO.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelperCtor_NullOutputDirectory_UsesEnvironment()
        {
            var mockSystem = new Mock<ISystem>(MockBehavior.Strict);
            var mockDateTime = new Mock<ISystemDateTime>(MockBehavior.Strict);
            var mockEnvironment = new Mock<ISystemEnvironment>(MockBehavior.Strict);
            var mockIO = new Mock<ISystemIO>(MockBehavior.Strict);
            var mockDirectory = new Mock<ISystemIODirectory>(MockBehavior.Strict);
            mockSystem.Setup(x => x.DateTime).Returns(mockDateTime.Object);
            mockSystem.Setup(x => x.Environment).Returns(mockEnvironment.Object);
            mockSystem.Setup(x => x.IO).Returns(mockIO.Object);
            mockIO.Setup(x => x.Directory).Returns(mockDirectory.Object);

            mockDateTime.Setup(x => x.Now).Returns(DateTime.Now);

            string testParam = @"c:\TestDir";
            mockEnvironment.Setup(x => x.CurrentDirectory).Returns(testParam);

            string expectedDirectory = Path.Combine(testParam, OutputFileHelper.DefaultOutputDirectoryName);

            var outputFileHelper = new OutputFileHelper(outputDirectory: null, system: mockSystem.Object);
            string result = outputFileHelper.GetNewA11yTestFilePath((value) => value);

            Assert.AreEqual(expectedDirectory, Path.GetDirectoryName(result));

            mockSystem.VerifyAll();
            mockDateTime.VerifyAll();
            mockEnvironment.VerifyAll();
            mockIO.VerifyAll();
            mockDirectory.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelperCtor_WithOutputDirectory_UsesProvidedDirectory()
        {
            var mockSystem = new Mock<ISystem>(MockBehavior.Strict);
            var mockDateTime = new Mock<ISystemDateTime>(MockBehavior.Strict);
            var mockIO = new Mock<ISystemIO>(MockBehavior.Strict);
            var mockDirectory = new Mock<ISystemIODirectory>(MockBehavior.Strict);
            mockSystem.Setup(x => x.DateTime).Returns(mockDateTime.Object);
            mockSystem.Setup(x => x.Environment).Returns(InertEnvironment);
            mockSystem.Setup(x => x.IO).Returns(mockIO.Object);
            mockIO.Setup(x => x.Directory).Returns(mockDirectory.Object);

            mockDateTime.Setup(x => x.Now).Returns(DateTime.Now);

            string expectedDirectory = @"c:\TestDir";

            var outputFileHelper = new OutputFileHelper(expectedDirectory, mockSystem.Object);
            string result = outputFileHelper.GetNewA11yTestFilePath((value) => value);

            Assert.AreEqual(expectedDirectory, Path.GetDirectoryName(result));

            mockSystem.VerifyAll();
            mockDateTime.VerifyAll();
            mockIO.VerifyAll();
            mockDirectory.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelper_EnsureOutputDirectoryExists_CreatesNonexistentDirectory()
        {
            var mockSystem = new Mock<ISystem>(MockBehavior.Strict);
            var mockIO = new Mock<ISystemIO>(MockBehavior.Strict);
            var mockDirectory = new Mock<ISystemIODirectory>(MockBehavior.Strict);
            mockSystem.Setup(x => x.DateTime).Returns(InertDateTime);
            mockSystem.Setup(x => x.Environment).Returns(InertEnvironment);
            mockSystem.Setup(x => x.IO).Returns(mockIO.Object);
            mockIO.Setup(x => x.Directory).Returns(mockDirectory.Object);

            string directory = @"c:\NonexistentDirectory";
            mockDirectory.Setup(x => x.Exists(directory)).Returns(false);
            mockDirectory.Setup(x => x.CreateDirectory(directory)).Returns<System.IO.DirectoryInfo>(null);

            var outputFileHelper = new OutputFileHelper(directory, mockSystem.Object);
            outputFileHelper.EnsureOutputDirectoryExists();

            // the following verifies that Exists was called

            mockSystem.VerifyAll();
            mockIO.VerifyAll();
            mockDirectory.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelper_EnsureOutputDirectoryExists_DoesNotCreateExistingDirectory()
        {
            var mockSystem = new Mock<ISystem>(MockBehavior.Strict);
            var mockIO = new Mock<ISystemIO>(MockBehavior.Strict);
            var mockDirectory = new Mock<ISystemIODirectory>(MockBehavior.Strict);
            mockSystem.Setup(x => x.DateTime).Returns(InertDateTime);
            mockSystem.Setup(x => x.Environment).Returns(InertEnvironment);
            mockSystem.Setup(x => x.IO).Returns(mockIO.Object);
            mockIO.Setup(x => x.Directory).Returns(mockDirectory.Object);

            string directory = @"c:\NonexistentDirectory";
            mockDirectory.Setup(x => x.Exists(directory)).Returns(true);

            var outputFileHelper = new OutputFileHelper(directory, mockSystem.Object);
            outputFileHelper.EnsureOutputDirectoryExists();

            // the following verifies that CreateDirectory was not called

            mockSystem.VerifyAll();
            mockIO.VerifyAll();
            mockDirectory.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelper_EnsureOutputDirectoryExists_DoesNotCatchExceptions()
        {
            var mockSystem = new Mock<ISystem>(MockBehavior.Strict);
            var mockIO = new Mock<ISystemIO>(MockBehavior.Strict);
            var mockDirectory = new Mock<ISystemIODirectory>(MockBehavior.Strict);
            mockSystem.Setup(x => x.DateTime).Returns(InertDateTime);
            mockSystem.Setup(x => x.Environment).Returns(InertEnvironment);
            mockSystem.Setup(x => x.IO).Returns(mockIO.Object);
            mockIO.Setup(x => x.Directory).Returns(mockDirectory.Object);

            string directory = @"c:\NonexistentDirectory";
            mockDirectory.Setup(x => x.Exists(directory)).Returns(false);
            mockDirectory.Setup(x => x.CreateDirectory(directory)).Throws<Exception>();

            var outputFileHelper = new OutputFileHelper(directory, mockSystem.Object);
            Assert.ThrowsException<Exception>(() => outputFileHelper.EnsureOutputDirectoryExists());

            mockSystem.VerifyAll();
            mockIO.VerifyAll();
            mockDirectory.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelper_GetNewA11yTestFilePath_GeneratedScanId_CreatesExpectedFileName()
        {
            var mockSystem = new Mock<ISystem>(MockBehavior.Strict);
            var mockDateTime = new Mock<ISystemDateTime>(MockBehavior.Strict);
            var mockIO = new Mock<ISystemIO>(MockBehavior.Strict);
            var mockDirectory = new Mock<ISystemIODirectory>(MockBehavior.Strict);
            mockSystem.Setup(x => x.DateTime).Returns(mockDateTime.Object);
            mockSystem.Setup(x => x.Environment).Returns(InertEnvironment);
            mockSystem.Setup(x => x.IO).Returns(mockIO.Object);

            mockIO.Setup(x => x.Directory).Returns(mockDirectory.Object);

            string directory = @"c:\TestDir";

            var dateTime = new DateTime(
                year: 2019,
                month: 4,
                day: 1,
                hour: 20,
                minute: 8,
                second: 8
                ).AddTicks(12345);

            mockDateTime.Setup(x => x.Now).Returns(dateTime);

            var outputFileHelper = new OutputFileHelper(directory, mockSystem.Object);
            var result = outputFileHelper.GetNewA11yTestFilePath((value) => value);

            var expectedFileName = $"{OutputFileHelper.DefaultFileNameBase}_19-04-01_20-08-08.0012345.a11ytest";
            var actualFileName = Path.GetFileName(result);
            Assert.AreEqual(expectedFileName, actualFileName);
            Assert.AreEqual(directory, Path.GetDirectoryName(result));

            mockSystem.VerifyAll();
            mockDateTime.VerifyAll();
            mockIO.VerifyAll();
            mockDirectory.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelper_GetNewA11yTestFilePath_SpecificScanId_CreatesExpectedFileName()
        {
            var mockSystem = new Mock<ISystem>(MockBehavior.Strict);
            var mockIO = new Mock<ISystemIO>(MockBehavior.Strict);
            var mockDirectory = new Mock<ISystemIODirectory>(MockBehavior.Strict);
            mockSystem.Setup(x => x.DateTime).Returns(InertDateTime);
            mockSystem.Setup(x => x.Environment).Returns(InertEnvironment);
            mockSystem.Setup(x => x.IO).Returns(mockIO.Object);

            mockIO.Setup(x => x.Directory).Returns(mockDirectory.Object);

            string directory = @"c:\TestDir2";

            var outputFileHelper = new OutputFileHelper(directory, mockSystem.Object);
            outputFileHelper.SetScanId("myScanId");
            var result = outputFileHelper.GetNewA11yTestFilePath((value) => value);

            var expectedFileName = "myScanId.a11ytest";
            var actualFileName = Path.GetFileName(result);
            Assert.AreEqual(expectedFileName, actualFileName);
            Assert.AreEqual(directory, Path.GetDirectoryName(result));

            mockSystem.VerifyAll();
            mockIO.VerifyAll();
            mockDirectory.VerifyAll();
        }
    } // class
} // namespace
