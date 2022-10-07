// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Axe.Windows.RulesTests
{
    [TestClass]
    public class RuleRunnerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsForNullProvider()
        {
            new RuleRunner(null);
        }

        [TestMethod]
        public void RunRuleByID_ReturnsExecutionErrorForUnknownRuleID()
        {
            var providerMock = new Mock<IRuleProvider>(MockBehavior.Strict);
            providerMock.Setup(m => m.GetRule(It.IsAny<RuleId>())).Returns(() => null).Verifiable();

            var e = new MockA11yElement();

            var runner = new RuleRunner(providerMock.Object);
            var result = runner.RunRuleByID(default(RuleId), e);

            Assert.AreEqual(EvaluationCode.RuleExecutionError, result.EvaluationCode);
            Assert.AreEqual(e, result.element);
            Assert.IsNull(result.RuleInfo);

            providerMock.VerifyAll();
        }

        [TestMethod]
        public void RunRuleByID_RuleNotApplicableForNonMatchingCondition()
        {
            // This test has the side effect of ensuring IRule.Evaluate is not called
            // when the condition is not matched.

            var e = new MockA11yElement();

            var conditionMock = new Mock<Condition>(MockBehavior.Strict);
            conditionMock.Setup(m => m.Matches(e)).Returns(false).Verifiable();

            var infoStub = new RuleInfo();

            var ruleMock = new Mock<IRule>(MockBehavior.Strict);
            ruleMock.Setup(m => m.Condition).Returns(conditionMock.Object).Verifiable();
            ruleMock.Setup(m => m.Info).Returns(() => infoStub).Verifiable();

            var providerMock = new Mock<IRuleProvider>(MockBehavior.Strict);
            providerMock.Setup(m => m.GetRule(It.IsAny<RuleId>())).Returns(() => ruleMock.Object).Verifiable();

            var runner = new RuleRunner(providerMock.Object);
            var result = runner.RunRuleByID(default(RuleId), e);

            Assert.AreEqual(EvaluationCode.NotApplicable, result.EvaluationCode);
            Assert.AreEqual(e, result.element);
            Assert.AreEqual(infoStub, result.RuleInfo);

            conditionMock.VerifyAll();
            ruleMock.VerifyAll();
            providerMock.VerifyAll();
        }

        [TestMethod]
        public void RunRuleByID_ReturnsExpectedEvaluationCodeWhenSuccessful()
        {
            var e = new MockA11yElement();

            var conditionMock = new Mock<Condition>(MockBehavior.Strict);
            conditionMock.Setup(m => m.Matches(e)).Returns(true).Verifiable();

            var ruleMock = CreateRuleMock(conditionMock.Object, EvaluationCode.NeedsReview, e);

            var providerMock = new Mock<IRuleProvider>(MockBehavior.Strict);
            providerMock.Setup(m => m.GetRule(It.IsAny<RuleId>())).Returns(() => ruleMock.Object).Verifiable();

            var runner = new RuleRunner(providerMock.Object);
            var result = runner.RunRuleByID(default(RuleId), e);

            Assert.AreEqual(EvaluationCode.NeedsReview, result.EvaluationCode);
            Assert.AreEqual(e, result.element);
            Assert.AreEqual(ruleMock.Object.Info, result.RuleInfo);

            conditionMock.VerifyAll();
            ruleMock.VerifyAll();
            providerMock.VerifyAll();
        }

        [TestMethod]
        public void RunRuleByID_ReturnsExpectedEvaluationCodeWhenPassesTestTrue()
        {
            var e = new MockA11yElement();

            var conditionMock = new Mock<Condition>(MockBehavior.Strict);
            conditionMock.Setup(m => m.Matches(e)).Returns(true).Verifiable();

            var infoStub = new RuleInfo
            {
                ErrorCode = EvaluationCode.NeedsReview,
            };

            var ruleMock = new Mock<IRule>(MockBehavior.Strict);
            ruleMock.Setup(m => m.Condition).Returns(conditionMock.Object).Verifiable();
            ruleMock.Setup(m => m.PassesTest(e)).Returns(true).Verifiable();
            ruleMock.Setup(m => m.Info).Returns(() => infoStub).Verifiable();

            var providerMock = new Mock<IRuleProvider>(MockBehavior.Strict);
            providerMock.Setup(m => m.GetRule(It.IsAny<RuleId>())).Returns(() => ruleMock.Object).Verifiable();

            var runner = new RuleRunner(providerMock.Object);
            var result = runner.RunRuleByID(default(RuleId), e);

            Assert.AreEqual(EvaluationCode.Pass, result.EvaluationCode);
            Assert.AreEqual(e, result.element);
            Assert.AreEqual(ruleMock.Object.Info, result.RuleInfo);

            conditionMock.VerifyAll();
            ruleMock.VerifyAll();
            providerMock.VerifyAll();
        }

        [TestMethod]
        public void RunRuleByID_ReturnsExpectedEvaluationCodeWhenPassesTestFalse()
        {
            var e = new MockA11yElement();

            var conditionMock = new Mock<Condition>(MockBehavior.Strict);
            conditionMock.Setup(m => m.Matches(e)).Returns(true).Verifiable();

            var infoStub = new RuleInfo
            {
                ErrorCode = EvaluationCode.NeedsReview,
            };

            var ruleMock = new Mock<IRule>(MockBehavior.Strict);
            ruleMock.Setup(m => m.Condition).Returns(conditionMock.Object).Verifiable();
            ruleMock.Setup(m => m.PassesTest(e)).Returns(false).Verifiable();
            ruleMock.Setup(m => m.Info).Returns(() => infoStub).Verifiable();

            var providerMock = new Mock<IRuleProvider>(MockBehavior.Strict);
            providerMock.Setup(m => m.GetRule(It.IsAny<RuleId>())).Returns(() => ruleMock.Object).Verifiable();

            var runner = new RuleRunner(providerMock.Object);
            var result = runner.RunRuleByID(default(RuleId), e);

            Assert.AreEqual(EvaluationCode.NeedsReview, result.EvaluationCode);
            Assert.AreEqual(e, result.element);
            Assert.AreEqual(ruleMock.Object.Info, result.RuleInfo);

            conditionMock.VerifyAll();
            ruleMock.VerifyAll();
            providerMock.VerifyAll();
        }

        [TestMethod]
        public void RunRuleByID_FrameworkIssueLinkIsNull_PropagatesToResult()
        {
            var e = new MockA11yElement();

            var conditionMock = new Mock<Condition>(MockBehavior.Strict);
            conditionMock.Setup(m => m.Matches(e)).Returns(true).Verifiable();

            var infoStub = new RuleInfo
            {
                ErrorCode = EvaluationCode.NeedsReview,
            };

            var ruleMock = new Mock<IRule>(MockBehavior.Strict);
            ruleMock.Setup(m => m.Condition).Returns(conditionMock.Object).Verifiable();
            ruleMock.Setup(m => m.PassesTest(e)).Returns(true).Verifiable();
            ruleMock.Setup(m => m.Info).Returns(() => infoStub).Verifiable();

            var providerMock = new Mock<IRuleProvider>(MockBehavior.Strict);
            providerMock.Setup(m => m.GetRule(It.IsAny<RuleId>())).Returns(() => ruleMock.Object).Verifiable();

            var runner = new RuleRunner(providerMock.Object);
            var result = runner.RunRuleByID(default(RuleId), e);

            Assert.IsNull(result.RuleInfo.FrameworkIssueLink);

            conditionMock.VerifyAll();
            ruleMock.VerifyAll();
            providerMock.VerifyAll();
        }

        [TestMethod]
        public void RunRuleByID_FrameworkIssueLinkIsNotNull_PropagatesToResult()
        {
            const string frameworkIssueLink = "https://docs.microsoft.com/known-framework-link";
            var e = new MockA11yElement();

            var conditionMock = new Mock<Condition>(MockBehavior.Strict);
            conditionMock.Setup(m => m.Matches(e)).Returns(true).Verifiable();

            var infoStub = new RuleInfo
            {
                ErrorCode = EvaluationCode.NeedsReview,
                FrameworkIssueLink = frameworkIssueLink,
            };

            var ruleMock = new Mock<IRule>(MockBehavior.Strict);
            ruleMock.Setup(m => m.Condition).Returns(conditionMock.Object).Verifiable();
            ruleMock.Setup(m => m.PassesTest(e)).Returns(true).Verifiable();
            ruleMock.Setup(m => m.Info).Returns(() => infoStub).Verifiable();

            var providerMock = new Mock<IRuleProvider>(MockBehavior.Strict);
            providerMock.Setup(m => m.GetRule(It.IsAny<RuleId>())).Returns(() => ruleMock.Object).Verifiable();

            var runner = new RuleRunner(providerMock.Object);
            var result = runner.RunRuleByID(default(RuleId), e);

            Assert.AreEqual(frameworkIssueLink, result.RuleInfo.FrameworkIssueLink);

            conditionMock.VerifyAll();
            ruleMock.VerifyAll();
            providerMock.VerifyAll();
        }

        [TestMethod]
        public void RunRuleByID_ReturnsExecutionErrorWhenPassesTestThrowsException()
        {
            var e = new MockA11yElement();

            var conditionMock = new Mock<Condition>(MockBehavior.Strict);
            conditionMock.Setup(m => m.Matches(e)).Returns(true).Verifiable();

            var infoStub = new RuleInfo();

            var ruleMock = new Mock<IRule>(MockBehavior.Strict);
            ruleMock.Setup(m => m.Condition).Returns(conditionMock.Object).Verifiable();
            ruleMock.Setup(m => m.PassesTest(e)).Throws<Exception>().Verifiable();
            ruleMock.Setup(m => m.Info).Returns(() => infoStub).Verifiable();

            var providerMock = new Mock<IRuleProvider>(MockBehavior.Strict);
            providerMock.Setup(m => m.GetRule(It.IsAny<RuleId>())).Returns(() => ruleMock.Object).Verifiable();

            var runner = new RuleRunner(providerMock.Object);
            var result = runner.RunRuleByID(default(RuleId), e);

            Assert.AreEqual(EvaluationCode.RuleExecutionError, result.EvaluationCode);
            Assert.AreEqual(e, result.element);
            Assert.AreEqual(infoStub, result.RuleInfo);

            conditionMock.VerifyAll();
            ruleMock.VerifyAll();
            providerMock.VerifyAll();
        }

        [TestMethod]
        public void RunRuleByID_ReturnsExecutionErrorWhenConditionMatchesThrowsException()
        {
            var e = new MockA11yElement();

            var conditionMock = new Mock<Condition>(MockBehavior.Strict);
            conditionMock.Setup(m => m.Matches(e)).Throws<Exception>().Verifiable();

            var infoStub = new RuleInfo();

            var ruleMock = new Mock<IRule>(MockBehavior.Strict);
            ruleMock.Setup(m => m.Condition).Returns(conditionMock.Object).Verifiable();
            ruleMock.Setup(m => m.Info).Returns(() => infoStub).Verifiable();

            var providerMock = new Mock<IRuleProvider>(MockBehavior.Strict);
            providerMock.Setup(m => m.GetRule(It.IsAny<RuleId>())).Returns(() => ruleMock.Object).Verifiable();

            var runner = new RuleRunner(providerMock.Object);
            var result = runner.RunRuleByID(default(RuleId), e);

            Assert.AreEqual(EvaluationCode.RuleExecutionError, result.EvaluationCode);
            Assert.AreEqual(e, result.element);
            Assert.AreEqual(infoStub, result.RuleInfo);

            conditionMock.VerifyAll();
            ruleMock.VerifyAll();
            providerMock.VerifyAll();
        }

        [TestMethod]
        public void RunRuleByID_CallsPassesTestWhenConditionIsNull()
        {
            var e = new MockA11yElement();

            var ruleMock = CreateRuleMock(null, EvaluationCode.Pass, e);

            var providerMock = new Mock<IRuleProvider>(MockBehavior.Strict);
            providerMock.Setup(m => m.GetRule(It.IsAny<RuleId>())).Returns(() => ruleMock.Object).Verifiable();

            var runner = new RuleRunner(providerMock.Object);
            var result = runner.RunRuleByID(default(RuleId), e);

            Assert.AreEqual(EvaluationCode.Pass, result.EvaluationCode);
            Assert.AreEqual(e, result.element);
            Assert.AreEqual(ruleMock.Object.Info, result.RuleInfo);

            ruleMock.VerifyAll();
            providerMock.VerifyAll();
        }

        [TestMethod]
        public void RunAll_ReturnsExpectedResults()
        {
            var e = new MockA11yElement();

            var ruleMocks = new List<Mock<IRule>>();
            var codes = Enum.GetValues(typeof(EvaluationCode)) as IEnumerable<EvaluationCode>;

            foreach (var code in codes)
                ruleMocks.Add(CreateRuleMock(Condition.True, code, e));

            var rules = from m in ruleMocks select (m.Object);

            var providerMock = new Mock<IRuleProvider>(MockBehavior.Strict);
            providerMock.Setup(m => m.All).Returns(() => rules).Verifiable();

            var runner = new RuleRunner(providerMock.Object);
            var results = runner.RunAll(e, CancellationToken.None);

            Assert.AreEqual(codes.Count(), results.Count());

            for (var i = 0; i < results.Count(); ++i)
            {
                var result = results.ElementAt(i);
                var expectedCode = codes.ElementAt(i);
                var ruleMock = ruleMocks[i];

                Assert.AreEqual(expectedCode, result.EvaluationCode);
                Assert.AreEqual(e, result.element);
                Assert.AreEqual(ruleMock.Object.Info, result.RuleInfo);

                ruleMock.VerifyAll();
            }

            providerMock.VerifyAll();
        }

        [TestMethod]
        public void RunAll_ThrowsOnCancellation()
        {
            var e = new MockA11yElement();
            var ruleMock = new Mock<IRule>(MockBehavior.Strict);
            var providerMock = new Mock<IRuleProvider>(MockBehavior.Strict);
            providerMock.Setup(m => m.All).Returns(() => new IRule[] { ruleMock.Object }).Verifiable();

            var runner = new RuleRunner(providerMock.Object);
            var cancellationToken = new CancellationTokenSource();
            cancellationToken.Cancel();
            Assert.ThrowsException<OperationCanceledException>(() => runner.RunAll(e, cancellationToken.Token));
        }

        private static Mock<IRule> CreateRuleMock(Condition condition, EvaluationCode code, A11yElement e)
        {
            var infoStub = new RuleInfo
            {
                ErrorCode = code
            };

            var ruleMock = new Mock<IRule>(MockBehavior.Strict);
            ruleMock.Setup(m => m.Condition).Returns(() => condition).Verifiable();
            ruleMock.Setup(m => m.PassesTest(e)).Returns(() => false).Verifiable();
            ruleMock.Setup(m => m.Info).Returns(() => infoStub).Verifiable();

            return ruleMock;
        }
    } // class
} // namespace
