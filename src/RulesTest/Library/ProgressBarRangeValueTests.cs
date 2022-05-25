// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class ProgressBarRangeValueTests
    {
        private readonly Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.ProgressBarRangeValue();
        private Mock<IA11yElement> elementMock = null;
        private Mock<IA11yPattern> patternMock = null;

        [TestInitialize]
        public void TestInitialize()
        {
            patternMock = new Mock<IA11yPattern>(MockBehavior.Strict);
            elementMock = new Mock<IA11yElement>(MockBehavior.Strict);
            elementMock.Setup(e => e.GetPattern(PatternType.UIA_RangeValuePatternId)).Returns(patternMock.Object);
        }

        private void AddPatternValue<T>(string name, T value)
        {
            patternMock.Setup(p => p.GetValue<T>(name)).Returns(value);
        }

        [TestMethod]
        public void ProgressBarRangeValue_MaxGreaterMin_Pass()
        {
            AddPatternValue("IsReadOnly", true);
            AddPatternValue("Minimum", 0);
            AddPatternValue("Maximum", 1);

            Assert.IsTrue(Rule.PassesTest(elementMock.Object));

            patternMock.Verify();
            elementMock.Verify();
        }

        [TestMethod]
        public void ProgressBarRangeValue_MaxEqualMin_Fail()
        {
            AddPatternValue("IsReadOnly", true);
            AddPatternValue("Minimum", 1);
            AddPatternValue("Maximum", 1);

            Assert.IsFalse(Rule.PassesTest(elementMock.Object));

            patternMock.Verify();
            elementMock.Verify();
        }

        [TestMethod]
        public void ProgressBarRangeValue_MaxLessMin_Fail()
        {
            AddPatternValue("IsReadOnly", true);
            AddPatternValue("Minimum", 1);
            AddPatternValue("Maximum", 0);

            Assert.IsFalse(Rule.PassesTest(elementMock.Object));

            patternMock.Verify();
            elementMock.Verify();
        }

        [TestMethod]
        public void ProgressBarRangeValue_IsReadOnlyFalse_Fail()
        {
            AddPatternValue("IsReadOnly", false);
            AddPatternValue("Minimum", 0);
            AddPatternValue("Maximum", 1);

            Assert.IsFalse(Rule.PassesTest(elementMock.Object));

            patternMock.Verify();
            elementMock.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ProgressBarRangeValue_NullElement_ThrowsException()
        {
            Rule.PassesTest(null);
        }

        [TestMethod]
        public void ProgressBarRangeValue_NullPattern_ThrowsException()
        {
            elementMock.Reset();
            elementMock.Setup(e => e.GetPattern(PatternType.UIA_RangeValuePatternId)).Returns<IA11yPattern>(null);

            var ex = Assert.ThrowsException<ArgumentNullException>(() => Rule.PassesTest(elementMock.Object));
            Assert.AreEqual("pattern", ex.ParamName);

            elementMock.Verify();
        }

        [TestMethod]
        public void ProgressBarRangeValue_Match()
        {
            TestControlTypeMatchesCondition(ControlType.ProgressBar, true);
        }

        [TestMethod]
        public void ProgressBarRangeValue_NotProgressBar_NoMatch()
        {
            var controlTypesToTest = ControlType.All.Difference(ControlType.ProgressBar);
            foreach (var controlType in controlTypesToTest)
                TestControlTypeMatchesCondition(controlType, false);
        }

        private void TestControlTypeMatchesCondition(int controlType, bool expectedResult)
        {
            elementMock.Setup(e => e.ControlTypeId).Returns(controlType);

            Assert.AreEqual(expectedResult, Rule.Condition.Matches(elementMock.Object));

            elementMock.Verify();
            patternMock.Verify();
        }

        [TestMethod]
        public void ProgressBarRangeValue_NoRangeValuePattern_NoMatch()
        {
            elementMock.Reset();
            elementMock.Setup(e => e.ControlTypeId).Returns(ControlType.ProgressBar);
            elementMock.Setup(e => e.GetPattern(PatternType.UIA_RangeValuePatternId)).Returns<IA11yPattern>(null);

            Assert.IsFalse(Rule.Condition.Matches(elementMock.Object));

            elementMock.Verify();
        }
    } // class
} // namespace
