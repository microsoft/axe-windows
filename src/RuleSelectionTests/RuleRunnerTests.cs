// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.RuleSelection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading;

namespace Axe.Windows.RuleSelectionTests
{
    [TestClass]
    public class RuleRunnerTests
    {
        [TestMethod]
        public void ExcludeFromRun_WhenExcludeRulesPass_ReturnsFalse()
        {
            var mockElement = new Mock<A11yElement>(MockBehavior.Strict);
            mockElement.Setup(e => e.Framework).Returns(FrameworkId.WPF);
            var exclude = RuleRunner.ExcludeFromRun(mockElement.Object, CancellationToken.None);
            Assert.IsFalse(exclude);
            var scanResults = mockElement.Object.ScanResults;
            Assert.AreEqual(1, scanResults.Items.Count);
            var chromeRuleResult = scanResults.Items.Where(i => i.Items.FirstOrDefault().Rule.Equals(RuleId.ChromiumComponentsShouldUseWebScanner));
            Assert.AreEqual(1, chromeRuleResult.Count());
            Assert.AreEqual(Core.Results.ScanStatus.Pass, chromeRuleResult.FirstOrDefault().Status);
        }

        [TestMethod]
        public void ExcludeFromRun_WhenExcludeRulesFail_ReturnsTrue()
        {
            var mockElement = new Mock<A11yElement>(MockBehavior.Strict);
            mockElement.Setup(e => e.Framework).Returns(FrameworkId.Chrome);
            var exclude = RuleRunner.ExcludeFromRun(mockElement.Object, CancellationToken.None);
            Assert.IsTrue(exclude);
            var scanResults = mockElement.Object.ScanResults;
            Assert.AreEqual(1, scanResults.Items.Count);
            var chromeRuleResult = scanResults.Items.Where(i => i.Items.FirstOrDefault().Rule.Equals(RuleId.ChromiumComponentsShouldUseWebScanner));
            Assert.AreEqual(1, chromeRuleResult.Count());
            Assert.AreEqual(Core.Results.ScanStatus.Fail, chromeRuleResult.FirstOrDefault().Status);
        }
    } // class
} // namespace
