// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Misc;
using Axe.Windows.Desktop.UIAutomation.TreeWalkers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
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
        [Timeout(1000)]
        public void RefreshTreeData_ThrowsWhenCancelled()
        {
            var cancellationToken = new CancellationTokenSource();
            cancellationToken.Cancel();
            var dataContext = new TreeWalkerDataContext(cancellationToken.Token);
            Assert.ThrowsException<OperationCanceledException>(() => _treeWalker.RefreshTreeData(TreeViewMode.Raw, dataContext));
        }
    }
}
