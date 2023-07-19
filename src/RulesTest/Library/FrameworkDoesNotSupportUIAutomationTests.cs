﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class FrameworkDoesNotSupportUIAutomationTests
    {
        private static readonly Rules.IRule Rule = new Rules.Library.FrameworkDoesNotSupportUIAutomation(excludedCondition: null);
        private Mock<IA11yElement> _elementMock;

        [TestInitialize]
        public void BeforeEach()
        {
            _elementMock = new Mock<IA11yElement>(MockBehavior.Strict);
        }

        [TestMethod]
        public void FrameworkIssueLink_IsNotNull()
        {
            Assert.IsNotNull(Rule.Info.FrameworkIssueLink);
        }

        [TestMethod]
        public void Condition_FrameworkIsNotWin32_ReturnsFalse()
        {
            IEnumerable<string> nonWin32Values = Extensions.GetFrameworkIds().Append("NotWin32").Except(new string[] { FrameworkId.Win32 });

            foreach (string nonWin32Value in nonWin32Values)
            {
                _elementMock.Setup(m => m.Framework)
                    .Returns(nonWin32Value)
                    .Verifiable();

                Assert.IsFalse(Rule.Condition.Matches(_elementMock.Object));
            }

            _elementMock.Verify(m => m.Framework, Times.Exactly(nonWin32Values.Count()));
        }

        [TestMethod]
        public void Condition_FrameworkIsWin32_ControlTypeIsNotWindow_ReturnsFalse()
        {
            _elementMock.Setup(m => m.Framework).Returns(FrameworkId.Win32).Verifiable();
            _elementMock.Setup(m => m.ControlTypeId).Returns(Core.Types.ControlType.UIA_ButtonControlTypeId);

            Assert.IsFalse(Rule.Condition.Matches(_elementMock.Object));

            _elementMock.Verify(m => m.Framework, Times.Once());
            _elementMock.Verify(m => m.ControlTypeId, Times.Once());
        }

        [TestMethod]
        public void Condition_FrameworkIsWin32_ControlTypeIsWindow_ReturnsTrue()
        {
            _elementMock.Setup(m => m.Framework).Returns(FrameworkId.Win32).Verifiable();
            _elementMock.Setup(m => m.ControlTypeId).Returns(Core.Types.ControlType.UIA_WindowControlTypeId);

            Assert.IsTrue(Rule.Condition.Matches(_elementMock.Object));

            _elementMock.Verify(m => m.Framework, Times.Once());
        }

        [TestMethod]
        public void PassesTest_ClassNameIsNotKnownProblematic_ReturnsFalse()
        {
            string[] testClasses = { "Edit", "SunAwT" };

            foreach (string testClass in testClasses)
            {
                _elementMock.Setup(m => m.ClassName).Returns(testClass).Verifiable(); ;

                Assert.IsTrue(Rule.PassesTest(_elementMock.Object));
            }

            _elementMock.VerifyAll();
        }

        [TestMethod]
        public void PassesTest_ClassNameIsKnownProblematic_ReturnsTrue()
        {
            string[] testClasses = { "SunAwt", "SunAwtXyz" };

            foreach (string testClass in testClasses)
            {
                _elementMock.Setup(m => m.ClassName).Returns(testClass).Verifiable(); ;

                Assert.IsFalse(Rule.PassesTest(_elementMock.Object));
            }

            _elementMock.VerifyAll();
        }
    }
}
