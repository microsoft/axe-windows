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
        [TestMethod]
        [Timeout(1000)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OutputFileHelperCtor_NullSystemFactory_ThrowsException()
        {
            new OutputFileHelper(outputDirectory: null, systemFactory: null);
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelperCtor_NullSystemDateTime_ThrowsException()
        {
            var mockSystemFactory = new Mock<ISystemFactory>(MockBehavior.Strict);
            mockSystemFactory.Setup(x => x.CreateSystemDateTime()).Returns<ISystemDateTime>(null);

            Action action = () => new OutputFileHelper(outputDirectory: null, systemFactory: mockSystemFactory.Object);
            var ex = Assert.ThrowsException<InvalidOperationException>(action);
            Assert.IsTrue(ex.Message.Contains("_dateTime"));

            mockSystemFactory.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelperCtor_NullSystemEnvironment_ThrowsException()
        {
            var mockSystemFactory = new Mock<ISystemFactory>(MockBehavior.Strict);
            var mockDateTime = new Mock<ISystemDateTime>(MockBehavior.Strict);
            mockSystemFactory.Setup(x => x.CreateSystemDateTime()).Returns(mockDateTime.Object);
            mockSystemFactory.Setup(x => x.CreateSystemEnvironment()).Returns<ISystemEnvironment>(null);

            Action action = () => new OutputFileHelper(outputDirectory: null, systemFactory: mockSystemFactory.Object);
            var ex = Assert.ThrowsException<InvalidOperationException>(action);
            Assert.IsTrue(ex.Message.Contains("environment"));

            mockSystemFactory.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelperCtor_NullSystemDirectory_ThrowsException()
        {
            var mockSystemFactory = new Mock<ISystemFactory>(MockBehavior.Strict);
            var mockDateTime = new Mock<ISystemDateTime>(MockBehavior.Strict);
            var mockEnvironment = new Mock<ISystemEnvironment>(MockBehavior.Strict);
            mockSystemFactory.Setup(x => x.CreateSystemDateTime()).Returns(mockDateTime.Object);
            mockSystemFactory.Setup(x => x.CreateSystemEnvironment()).Returns(mockEnvironment.Object);
            mockSystemFactory.Setup(x => x.CreateSystemIODirectory()).Returns<ISystemIODirectory>(null);

            Action action = () => new OutputFileHelper(outputDirectory: null, systemFactory: mockSystemFactory.Object);
            var ex = Assert.ThrowsException<InvalidOperationException>(action);
            Assert.IsTrue(ex.Message.Contains("directory"));

            mockSystemFactory.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelperCtor_InvalidOutputDirectory_ThrowsException()
        {
            var mockSystemFactory = new Mock<ISystemFactory>(MockBehavior.Strict);
            var mockDateTime = new Mock<ISystemDateTime>(MockBehavior.Strict);
            var mockEnvironment = new Mock<ISystemEnvironment>(MockBehavior.Strict);
            var mockDirectory = new Mock<ISystemIODirectory>(MockBehavior.Strict);
            mockSystemFactory.Setup(x => x.CreateSystemDateTime()).Returns(mockDateTime.Object);
            mockSystemFactory.Setup(x => x.CreateSystemEnvironment()).Returns(mockEnvironment.Object);
            mockSystemFactory.Setup(x => x.CreateSystemIODirectory()).Returns(mockDirectory.Object);

            var phonyDirectory = "flub";
            Action action = () => new OutputFileHelper(phonyDirectory, mockSystemFactory.Object);
            var ex = Assert.ThrowsException<AxeWindowsAutomationException>(action);
            Assert.AreEqual(String.Format(DisplayStrings.ErrorDirectoryInvalid, phonyDirectory), ex.Message);

            mockSystemFactory.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelperCtor_NullOutputDirectory_UsesEnvironment()
        {
            var mockSystemFactory = new Mock<ISystemFactory>(MockBehavior.Strict);
            var mockDateTime = new Mock<ISystemDateTime>(MockBehavior.Strict);
            var mockEnvironment = new Mock<ISystemEnvironment>(MockBehavior.Strict);
            var mockDirectory = new Mock<ISystemIODirectory>(MockBehavior.Strict);
            mockSystemFactory.Setup(x => x.CreateSystemDateTime()).Returns(mockDateTime.Object);
            mockSystemFactory.Setup(x => x.CreateSystemEnvironment()).Returns(mockEnvironment.Object);
            mockSystemFactory.Setup(x => x.CreateSystemIODirectory()).Returns(mockDirectory.Object);

            mockDateTime.Setup(x => x.Now).Returns(DateTime.Now);

            string testParam = @"c:\TestDir";
            mockEnvironment.Setup(x => x.CurrentDirectory).Returns(testParam);

            string expectedDirectory = Path.Combine(testParam, OutputFileHelper.DefaultOutputDirectoryName);
            mockDirectory.Setup(x => x.Exists(expectedDirectory)).Returns(true);

            var outputFileHelper = new OutputFileHelper(outputDirectory: null, systemFactory: mockSystemFactory.Object);
            string result = outputFileHelper.GetNewA11yTestFilePath();

            Assert.AreEqual(expectedDirectory, Path.GetDirectoryName(result));

            mockSystemFactory.VerifyAll();
            mockDateTime.VerifyAll();
            mockEnvironment.VerifyAll();
            mockDirectory.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelperCtor_WithOutputDirectory_UsesProvidedDirectory()
        {
            var mockSystemFactory = new Mock<ISystemFactory>(MockBehavior.Strict);
            var mockDateTime = new Mock<ISystemDateTime>(MockBehavior.Strict);
            var mockEnvironment = new Mock<ISystemEnvironment>(MockBehavior.Strict);
            var mockDirectory = new Mock<ISystemIODirectory>(MockBehavior.Strict);
            mockSystemFactory.Setup(x => x.CreateSystemDateTime()).Returns(mockDateTime.Object);
            mockSystemFactory.Setup(x => x.CreateSystemEnvironment()).Returns(mockEnvironment.Object);
            mockSystemFactory.Setup(x => x.CreateSystemIODirectory()).Returns(mockDirectory.Object);

            mockDateTime.Setup(x => x.Now).Returns(DateTime.Now);

            string expectedDirectory = @"c:\TestDir";
            mockDirectory.Setup(x => x.Exists(expectedDirectory)).Returns(true);

            var outputFileHelper = new OutputFileHelper(expectedDirectory, mockSystemFactory.Object);
            string result = outputFileHelper.GetNewA11yTestFilePath();

            Assert.AreEqual(expectedDirectory, Path.GetDirectoryName(result));

            mockSystemFactory.VerifyAll();
            mockDateTime.VerifyAll();
            mockEnvironment.VerifyAll();
            mockDirectory.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelperCtor_CreatesNonexistentDirectory()
        {
            var mockSystemFactory = new Mock<ISystemFactory>(MockBehavior.Strict);
            var mockDateTime = new Mock<ISystemDateTime>(MockBehavior.Strict);
            var mockEnvironment = new Mock<ISystemEnvironment>(MockBehavior.Strict);
            var mockDirectory = new Mock<ISystemIODirectory>(MockBehavior.Strict);
            mockSystemFactory.Setup(x => x.CreateSystemDateTime()).Returns(mockDateTime.Object);
            mockSystemFactory.Setup(x => x.CreateSystemEnvironment()).Returns(mockEnvironment.Object);
            mockSystemFactory.Setup(x => x.CreateSystemIODirectory()).Returns(mockDirectory.Object);

            string directory = @"c:\NonexistentDirectory";
            mockDirectory.Setup(x => x.Exists(directory)).Returns(false);
            mockDirectory.Setup(x => x.CreateDirectory(directory)).Returns<System.IO.DirectoryInfo>(null);

            var outputFileHelper = new OutputFileHelper(directory, mockSystemFactory.Object);

            // the folowing verifies that Exists was called

            mockSystemFactory.VerifyAll();
            mockDateTime.VerifyAll();
            mockEnvironment.VerifyAll();
            mockDirectory.VerifyAll();
        }

        [TestMethod]
        [Timeout(1000)]
        public void OutputFileHelper_GetNewA11yTestFilePath_CreatesExpectedFileName()
        {
            var mockSystemFactory = new Mock<ISystemFactory>(MockBehavior.Strict);
            var mockDateTime = new Mock<ISystemDateTime>(MockBehavior.Strict);
            var mockEnvironment = new Mock<ISystemEnvironment>(MockBehavior.Strict);
            var mockDirectory = new Mock<ISystemIODirectory>(MockBehavior.Strict);
            mockSystemFactory.Setup(x => x.CreateSystemDateTime()).Returns(mockDateTime.Object);
            mockSystemFactory.Setup(x => x.CreateSystemEnvironment()).Returns(mockEnvironment.Object);
            mockSystemFactory.Setup(x => x.CreateSystemIODirectory()).Returns(mockDirectory.Object);

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

            var outputFileHelper = new OutputFileHelper(directory, mockSystemFactory.Object);
            var result = outputFileHelper.GetNewA11yTestFilePath();

            var expectedFileName = $"{OutputFileHelper.DefaultFileNameBase}_19-04-01_20-08-08.0012345.a11ytest";
            var actualFileName = Path.GetFileName(result);
            Assert.AreEqual(expectedFileName, actualFileName);

            mockSystemFactory.VerifyAll();
            mockDateTime.VerifyAll();
            mockEnvironment.VerifyAll();
            mockDirectory.VerifyAll();
        }
    } // class
} // namespace
