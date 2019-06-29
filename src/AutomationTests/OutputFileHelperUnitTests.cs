// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Abstractions;
using Axe.Windows.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Path = System.IO.Path;

namespace Axe.Windows.AutomationTests
{
    [TestClass]
    public class OutputFileHelperUnitTests
    {
        private static readonly ISystemDateTime InertDateTime = new Mock<ISystemDateTime>(MockBehavior.Strict).Object;
        private static readonly ISystemEnvironment InertEnvironment = new Mock<ISystemEnvironment>(MockBehavior.Strict).Object;

        [TestMethod]
        [Timeout(1000)]
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
            Assert.AreEqual(String.Format(DisplayStrings.ErrorDirectoryInvalid, phonyDirectory), ex.Message);

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
            mockDirectory.Setup(x => x.Exists(expectedDirectory)).Returns(true);

            var outputFileHelper = new OutputFileHelper(outputDirectory: null, system: mockSystem.Object);
            string result = outputFileHelper.GetNewA11yTestFilePath();

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
            mockDirectory.Setup(x => x.Exists(expectedDirectory)).Returns(true);

            var outputFileHelper = new OutputFileHelper(expectedDirectory, mockSystem.Object);
            string result = outputFileHelper.GetNewA11yTestFilePath();

            Assert.AreEqual(expectedDirectory, Path.GetDirectoryName(result));

            mockSystem.VerifyAll();
            mockDateTime.VerifyAll();
            mockIO.VerifyAll();
            mockDirectory.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelperCtor_CreatesNonexistentDirectory()
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

            // the folowing verifies that Exists was called

            mockSystem.VerifyAll();
            mockIO.VerifyAll();
            mockDirectory.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelper_GetNewA11yTestFilePath_CreatesExpectedFileName()
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
            mockDirectory.Setup(x => x.Exists(directory)).Returns(true);

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
            var result = outputFileHelper.GetNewA11yTestFilePath();

            var expectedFileName = $"{OutputFileHelper.DefaultFileNameBase}_19-04-01_20-08-08.0012345.a11ytest";
            var actualFileName = Path.GetFileName(result);
            Assert.AreEqual(expectedFileName, actualFileName);

            mockSystem.VerifyAll();
            mockDateTime.VerifyAll();
            mockIO.VerifyAll();
            mockDirectory.VerifyAll();
        }
    } // class
} // namespace
