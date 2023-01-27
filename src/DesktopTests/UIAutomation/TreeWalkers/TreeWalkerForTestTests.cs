// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Misc;
using Axe.Windows.Desktop.UIAutomation;
using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using Axe.Windows.Desktop.UIAutomation.TreeWalkers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading;

namespace Axe.Windows.DesktopTests.UIAutomation.TreeWalkers
{
    [TestClass]
    public class TreeWalkerForTestTests
    {
        private TreeWalkerForTest _treeWalker;

        private Mock<A11yElement> _elementMock;
        private Mock<BoundedCounter> _boundedCounterMock;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _elementMock = new Mock<A11yElement>(MockBehavior.Strict);
            _boundedCounterMock = new Mock<BoundedCounter>(MockBehavior.Strict, 1);
            _treeWalker = new TreeWalkerForTest(_elementMock.Object, _boundedCounterMock.Object);
        }

        [TestMethod]
        public void RefreshTreeData_ThrowsWhenCancelled()
        {
            var cancellationToken = new CancellationTokenSource();
            cancellationToken.Cancel();
            var dataContext = new DesktopDataContext(Registrar.GetDefaultInstance(), A11yAutomation.GetDefaultInstance(), cancellationToken.Token);
            Assert.ThrowsException<OperationCanceledException>(() => _treeWalker.RefreshTreeData(TreeViewMode.Raw, dataContext));
        }

        [TestMethod]
        public void RefreshTreeData_RunsAllRules()
        {
            _elementMock.Setup(e => e.Framework).Returns(FrameworkId.WPF);
            var dataContext = new DesktopDataContext(Registrar.GetDefaultInstance(), A11yAutomation.GetDefaultInstance(), CancellationToken.None);
            _treeWalker.RefreshTreeData(TreeViewMode.Raw, dataContext);
            var scanResults = _elementMock.Object.ScanResults;
            Assert.AreEqual(2, scanResults.Items.Count);
            var exclusionRule = scanResults.Items.Where(i => i.Items.FirstOrDefault().Rule.Equals(RuleId.ChromiumComponentsShouldUseWebScanner));
            Assert.AreEqual(1, exclusionRule.Count());
            Assert.AreEqual(Core.Results.ScanStatus.Pass, exclusionRule.FirstOrDefault().Status);
        }

        [TestMethod]
        public void RefreshTreeData_ExcludesElementsWhenTheyViolateExclusionRules()
        {
            _elementMock.Setup(e => e.Framework).Returns(FrameworkId.Chrome);
            var dataContext = new DesktopDataContext(Registrar.GetDefaultInstance(), A11yAutomation.GetDefaultInstance(), CancellationToken.None);
            _treeWalker.RefreshTreeData(TreeViewMode.Raw, dataContext);
            var scanResults = _elementMock.Object.ScanResults;
            Assert.AreEqual(1, scanResults.Items.Count);
            var exclusionRule = scanResults.Items.Where(i => i.Items.FirstOrDefault().Rule.Equals(RuleId.ChromiumComponentsShouldUseWebScanner));
            Assert.AreEqual(1, exclusionRule.Count());
            Assert.AreEqual(Core.Results.ScanStatus.Fail, exclusionRule.FirstOrDefault().Status);
        }
    }
}
