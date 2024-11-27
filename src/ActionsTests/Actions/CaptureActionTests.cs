// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Actions.Enums;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Misc;
using Axe.Windows.Desktop.UIAutomation;
using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using Axe.Windows.Desktop.UIAutomation.TreeWalkers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Axe.Windows.ActionsTests.Actions
{
    [TestClass]
    public class CaptureActionTests
    {
        Mock<IActionContext> _mockActionContext;
        DataManager _mockDataManager;
        A11yElement _mockElement;
        ElementContext _mockElementContext;
        int _irrelevantMaxElements;
        ElementDataContext _mockDataContext;
        TreeViewMode _mockTreeViewMode;
        DesktopDataContext _mockDesktopDataContext;

        [TestInitialize]
        public void ResetMocks()
        {
            _mockDataManager = new DataManager();
            _mockElement = new A11yElement
            {
                UniqueId = 0
            };
            _mockElementContext = new ElementContext(_mockElement);
            _irrelevantMaxElements = 10;
            _mockDataContext = new ElementDataContext(_mockElement, _irrelevantMaxElements);
            _mockTreeViewMode = (TreeViewMode)(-1);

            _mockElementContext.DataContext = _mockDataContext;
            _mockDataContext.TreeMode = _mockTreeViewMode;
            _mockDataContext.Mode = DataContextMode.Live;
            _mockDataManager.AddElementContext(_mockElementContext);

            Registrar registrar = new Registrar();
            _mockDesktopDataContext = new DesktopDataContext(registrar, A11yAutomation.CreateInstance(), CancellationToken.None);

            _mockActionContext = new Mock<IActionContext>(MockBehavior.Strict);
            _mockActionContext.Setup(m => m.DataManager).Returns(_mockDataManager);
            _mockActionContext.Setup(m => m.Registrar).Returns(registrar);
            _mockActionContext.Setup(m => m.DesktopDataContext).Returns(_mockDesktopDataContext);
        }

        [TestMethod]
        [Timeout(1000)]
        public void SetLiveModeDataContext_NoopsIfElementAlreadyHasComparableDataContext()
        {
            _mockDataContext.Mode = DataContextMode.Live;

            using (new CaptureActionTestHookOverrides(null, null))
            {
                CaptureAction.SetLiveModeDataContext(_mockElementContext.Id, _mockTreeViewMode, _mockActionContext.Object, force: false);
            }

            Assert.AreSame(_mockDataContext, _mockElementContext.DataContext);
        }

        [TestMethod]
        // force == true should force a TreeWalker evaluation, even with an existing similar DataContext
        [DataRow(true, false, DataContextMode.Live, TreeViewMode.Raw)]
        // An element with a null DataContext should force a TreeWalker evaluation
        [DataRow(false, true, DataContextMode.Live, TreeViewMode.Raw)]
        // An element with Mode != the newly requested mode should force a TreeWalker evaluation
        [DataRow(false, false, DataContextMode.Test, TreeViewMode.Raw)]
        // An element with TreeMode != the newly requested mode should force a TreeWalker evaluation
        [DataRow(false, false, DataContextMode.Live, TreeViewMode.Control)]
        [Timeout(1000)]
        public void SetLiveModeDataContext_UsesTreeWalkerForLiveToCreateNewDataContext(
            bool force,
            bool startWithNullDataContext,
            DataContextMode originalDataContextMode,
            TreeViewMode originalDataContextTreeViewMode)
        {
            // Arrange
            if (startWithNullDataContext) { _mockElementContext.DataContext = null; }
            _mockDataContext.Mode = originalDataContextMode;
            _mockDataContext.TreeMode = originalDataContextTreeViewMode;
            TreeViewMode treeViewMode = TreeViewMode.Raw;

            var mockTreeWalkerForLive = new Mock<ITreeWalkerForLive>();
            var mockTopMostElement = new A11yElement();
            var mockElementsItem1 = new A11yElement
            {
                UniqueId = 101
            };
            var mockElementsItem2 = new A11yElement
            {
                UniqueId = 102
            };
            mockTreeWalkerForLive.Setup(w => w.Elements).Returns(new List<A11yElement> { mockElementsItem1, mockElementsItem2 });
            mockTreeWalkerForLive.Setup(w => w.RootElement).Returns(mockTopMostElement);

            using (new CaptureActionTestHookOverrides(() => mockTreeWalkerForLive.Object, null))
            {
                // Act
                CaptureAction.SetLiveModeDataContext(_mockElementContext.Id, treeViewMode, _mockActionContext.Object, force);

                // Assert
                Assert.IsNotNull(_mockElementContext.DataContext);
                Assert.AreNotSame(_mockDataContext, _mockElementContext.DataContext);
                var result = _mockElementContext.DataContext;
                Assert.AreSame(_mockElement, result.Element);
                Assert.AreEqual(20000, result.ElementCounter.UpperBound);
                Assert.AreEqual(treeViewMode, result.TreeMode);
                Assert.AreEqual(DataContextMode.Live, result.Mode);
                Assert.AreEqual(mockTopMostElement, result.RootElment);
                mockTreeWalkerForLive.Verify(w => w.GetTreeHierarchy(_mockElement, treeViewMode, _mockDesktopDataContext));
                Assert.AreEqual(2, result.Elements.Count);
                Assert.AreSame(mockElementsItem1, result.Elements[mockElementsItem1.UniqueId]);
                Assert.AreSame(mockElementsItem2, result.Elements[mockElementsItem2.UniqueId]);
            }
        }

        [TestMethod]
        // Matching combinations of mode/treemode should result in reloading being skipped
        [DataRow(DataContextMode.Test, TreeViewMode.Raw, DataContextMode.Test, TreeViewMode.Raw)]
        [DataRow(DataContextMode.Test, TreeViewMode.Control, DataContextMode.Test, TreeViewMode.Control)]
        [DataRow(DataContextMode.Load, TreeViewMode.Raw, DataContextMode.Load, TreeViewMode.Raw)]
        [DataRow(DataContextMode.Load, TreeViewMode.Control, DataContextMode.Load, TreeViewMode.Control)]
        // It should assume that a "Load" DataContextMode never be reload, regardless of TreeViewMode, since it wouldn't be able to change anyway
        [DataRow(DataContextMode.Load, TreeViewMode.Control, DataContextMode.Load, TreeViewMode.Raw)]
        [Timeout(1000)]
        public void SetTestModeDataContext_NoopsIfElementDoesNotNeedTestContextUpdate(DataContextMode originalMode, TreeViewMode originalTreeMode, DataContextMode newlySetMode, TreeViewMode newlySetTreeMode)
        {
            _mockDataContext.Mode = originalMode;
            _mockDataContext.TreeMode = originalTreeMode;

            using (new CaptureActionTestHookOverrides(null, null))
            {
                bool retVal = CaptureAction.SetTestModeDataContext(_mockElementContext.Id, newlySetMode, newlySetTreeMode, _mockActionContext.Object, force: false);

                Assert.IsFalse(retVal);
                Assert.AreSame(_mockDataContext, _mockElementContext.DataContext);
            }
        }

        [TestMethod]
        // force == true should force a TreeWalker evaluation, even with an existing similar DataContext
        [DataRow(true, false, DataContextMode.Test, TreeViewMode.Raw)]
        // An element with a null DataContext should force a TreeWalker evaluation
        [DataRow(false, true, DataContextMode.Test, TreeViewMode.Raw)]
        // An element with Mode != the newly requested mode should force a TreeWalker evaluation
        [DataRow(false, false, DataContextMode.Live, TreeViewMode.Raw)]
        // An element with TreeMode != the newly requested mode should force a TreeWalker evaluation
        [DataRow(false, false, DataContextMode.Test, TreeViewMode.Control)]
        [Timeout(1000)]
        public void SetTestModeDataContext_ForTestDataContextMode_UsesTreeWalkerForTestToCreateNewDataContext(
            bool force,
            bool startWithNullDataContext,
            DataContextMode originalDataContextMode,
            TreeViewMode originalDataContextTreeViewMode)
        {
            // Arrange
            if (startWithNullDataContext) { _mockElementContext.DataContext = null; }
            _mockDataContext.Mode = originalDataContextMode;
            _mockDataContext.TreeMode = originalDataContextTreeViewMode;
            TreeViewMode treeViewMode = TreeViewMode.Raw;

            var mockTreeWalkerForTest = new Mock<ITreeWalkerForTest>();
            var mockTopMostElement = new A11yElement();
            var mockElementsItem1 = new A11yElement
            {
                UniqueId = 101
            };
            var mockElementsItem2 = new A11yElement
            {
                UniqueId = 102
            };
            mockTreeWalkerForTest.Setup(w => w.Elements).Returns(new List<A11yElement> { mockElementsItem1, mockElementsItem2 });
            mockTreeWalkerForTest.Setup(w => w.TopMostElement).Returns(mockTopMostElement);

            using (new CaptureActionTestHookOverrides(null, (_1, _2) => mockTreeWalkerForTest.Object))
            {
                // Act
                bool retVal = CaptureAction.SetTestModeDataContext(_mockElementContext.Id, DataContextMode.Test, treeViewMode, _mockActionContext.Object, force);
                // Assert
                Assert.IsTrue(retVal);
                Assert.IsNotNull(_mockElementContext.DataContext);
                Assert.AreNotSame(_mockDataContext, _mockElementContext.DataContext);
                var result = _mockElementContext.DataContext;
                Assert.AreSame(_mockElement, result.Element);
                Assert.AreEqual(20000, result.ElementCounter.UpperBound);
                Assert.AreEqual(treeViewMode, result.TreeMode);
                Assert.AreEqual(DataContextMode.Test, result.Mode);
                Assert.AreEqual(mockTopMostElement, result.RootElment);
                mockTreeWalkerForTest.Verify(w => w.RefreshTreeData(treeViewMode, It.IsAny<DesktopDataContext>()));
                Assert.AreEqual(2, result.Elements.Count);
                Assert.AreSame(mockElementsItem1, result.Elements[mockElementsItem1.UniqueId]);
                Assert.AreSame(mockElementsItem2, result.Elements[mockElementsItem2.UniqueId]);
            }
        }

        [TestMethod]
        // force == true should force a TreeWalker evaluation, even with an existing similar DataContext
        [DataRow(true, false, DataContextMode.Test, TreeViewMode.Raw)]
        // An element with a null DataContext should force a TreeWalker evaluation
        [DataRow(false, true, DataContextMode.Test, TreeViewMode.Raw)]
        // An element with Mode != the newly requested mode should force a TreeWalker evaluation
        [DataRow(false, false, DataContextMode.Live, TreeViewMode.Raw)]
        [Timeout(1000)]
        public void SetTestModeDataContext_ForLoadDataContextMode_WalksTheOriginAncestorsChildrenDirectly(
            bool force,
            bool startWithNullDataContext,
            DataContextMode originalDataContextMode,
            TreeViewMode originalDataContextTreeViewMode)
        {
            // Arrange
            if (startWithNullDataContext) { _mockElementContext.DataContext = null; }
            _mockDataContext.Mode = originalDataContextMode;
            _mockDataContext.TreeMode = originalDataContextTreeViewMode;
            TreeViewMode treeViewMode = TreeViewMode.Raw;

            var mockOriginAncestor = _mockElement;
            mockOriginAncestor.UniqueId = 1;
            var mockChild1 = new A11yElement
            {
                UniqueId = 2
            };
            var mockChild2 = new A11yElement
            {
                UniqueId = 3
            };
            var mockGrandChild1 = new A11yElement
            {
                UniqueId = 4
            };
            mockOriginAncestor.Children = new List<A11yElement>() { mockChild1, mockChild2 };
            mockChild2.Children = new List<A11yElement>() { mockGrandChild1 };

            using (new CaptureActionTestHookOverrides(null, null))
            {
                // Act
                bool retVal = CaptureAction.SetTestModeDataContext(_mockElementContext.Id, DataContextMode.Load, treeViewMode, _mockActionContext.Object, force);

                // Assert
                Assert.IsTrue(retVal);
                Assert.IsNotNull(_mockElementContext.DataContext);
                Assert.AreNotSame(_mockDataContext, _mockElementContext.DataContext);
                var result = _mockElementContext.DataContext;
                Assert.AreSame(_mockElement, result.Element);
                Assert.AreEqual(20000, result.ElementCounter.UpperBound);
                Assert.AreEqual(treeViewMode, result.TreeMode);
                Assert.AreEqual(DataContextMode.Load, result.Mode);
                Assert.AreEqual(mockOriginAncestor, result.RootElment);

                Assert.AreEqual(4, result.Elements.Count);
                Assert.AreSame(mockOriginAncestor, result.Elements[mockOriginAncestor.UniqueId]);
                Assert.AreSame(mockChild1, result.Elements[mockChild1.UniqueId]);
                Assert.AreSame(mockChild2, result.Elements[mockChild2.UniqueId]);
                Assert.AreSame(mockGrandChild1, result.Elements[mockGrandChild1.UniqueId]);
            }
        }

        private class CaptureActionTestHookOverrides : IDisposable
        {
            internal static Func<ITreeWalkerForLive> originalNewTreeWalkerForLive;
            internal static Func<A11yElement, BoundedCounter, ITreeWalkerForTest> originalNewTreeWalkerForTest;

            public CaptureActionTestHookOverrides(Func<ITreeWalkerForLive> newTreeWalkerForLive, Func<A11yElement, BoundedCounter, ITreeWalkerForTest> newTreeWalkerForTest)
            {
                originalNewTreeWalkerForLive = CaptureAction.NewTreeWalkerForLive;
                originalNewTreeWalkerForTest = CaptureAction.NewTreeWalkerForTest;

                CaptureAction.NewTreeWalkerForLive = newTreeWalkerForLive;
                CaptureAction.NewTreeWalkerForTest = newTreeWalkerForTest;
            }

            public void Dispose()
            {
                CaptureAction.NewTreeWalkerForLive = originalNewTreeWalkerForLive;
                CaptureAction.NewTreeWalkerForTest = originalNewTreeWalkerForTest;
            }
        }
    }
}
