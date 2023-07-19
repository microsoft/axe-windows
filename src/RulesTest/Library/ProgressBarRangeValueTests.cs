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
        private static readonly Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.ProgressBarRangeValue(excludedCondition: null);
        private Mock<IA11yElement> _elementMock = null;
        private Mock<IA11yPattern> _patternMock = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _patternMock = new Mock<IA11yPattern>(MockBehavior.Strict);
            _elementMock = new Mock<IA11yElement>(MockBehavior.Strict);
            _elementMock.Setup(e => e.GetPattern(PatternType.UIA_RangeValuePatternId)).Returns(_patternMock.Object);
        }

        private void AddPatternValue<T>(string name, T value)
        {
            _patternMock.Setup(p => p.GetValue<T>(name)).Returns(value);
        }

        [TestMethod]
        public void ProgressBarRangeValue_MaxGreaterMin_Pass()
        {
            AddPatternValue("IsReadOnly", true);
            AddPatternValue("Minimum", 0);
            AddPatternValue("Maximum", 1);

            Assert.IsTrue(Rule.PassesTest(_elementMock.Object));

            _patternMock.Verify();
            _elementMock.Verify();
        }

        [TestMethod]
        public void ProgressBarRangeValue_MaxEqualMin_Fail()
        {
            AddPatternValue("IsReadOnly", true);
            AddPatternValue("Minimum", 1);
            AddPatternValue("Maximum", 1);

            Assert.IsFalse(Rule.PassesTest(_elementMock.Object));

            _patternMock.Verify();
            _elementMock.Verify();
        }

        [TestMethod]
        public void ProgressBarRangeValue_MaxLessMin_Fail()
        {
            AddPatternValue("IsReadOnly", true);
            AddPatternValue("Minimum", 1);
            AddPatternValue("Maximum", 0);

            Assert.IsFalse(Rule.PassesTest(_elementMock.Object));

            _patternMock.Verify();
            _elementMock.Verify();
        }

        [TestMethod]
        public void ProgressBarRangeValue_IsReadOnlyFalse_Fail()
        {
            AddPatternValue("IsReadOnly", false);
            AddPatternValue("Minimum", 0);
            AddPatternValue("Maximum", 1);

            Assert.IsFalse(Rule.PassesTest(_elementMock.Object));

            _patternMock.Verify();
            _elementMock.Verify();
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
            _elementMock.Reset();
            _elementMock.Setup(e => e.GetPattern(PatternType.UIA_RangeValuePatternId)).Returns<IA11yPattern>(null);

            var ex = Assert.ThrowsException<ArgumentNullException>(() => Rule.PassesTest(_elementMock.Object));
            Assert.AreEqual("pattern", ex.ParamName);

            _elementMock.Verify();
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
            _elementMock.Setup(e => e.ControlTypeId).Returns(controlType);

            Assert.AreEqual(expectedResult, Rule.Condition.Matches(_elementMock.Object));

            _elementMock.Verify();
            _patternMock.Verify();
        }

        [TestMethod]
        public void ProgressBarRangeValue_NoRangeValuePattern_NoMatch()
        {
            _elementMock.Reset();
            _elementMock.Setup(e => e.ControlTypeId).Returns(ControlType.ProgressBar);
            _elementMock.Setup(e => e.GetPattern(PatternType.UIA_RangeValuePatternId)).Returns<IA11yPattern>(null);

            Assert.IsFalse(Rule.Condition.Matches(_elementMock.Object));

            _elementMock.Verify();
        }
    } // class
} // namespace
