// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Actions.Enums;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Misc;
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
        Mock<IActionContext> mockActionContext;
        DataManager mockDataManager;
        A11yElement mockElement;
        ElementContext mockElementContext;
        int irrelevantMaxElements;
        ElementDataContext mockDataContext;
        TreeViewMode mockTreeViewMode;

        [TestInitialize]
        public void ResetMocks()
        {
            mockDataManager = new DataManager();
            mockElement = new A11yElement
            {
                UniqueId = 0
            };
            mockElementContext = new ElementContext(mockElement);
            irrelevantMaxElements = 10;
            mockDataContext = new ElementDataContext(mockElement, irrelevantMaxElements);
            mockTreeViewMode = (TreeViewMode)(-1);

            mockElementContext.DataContext = mockDataContext;
            mockDataContext.TreeMode = mockTreeViewMode;
            mockDataContext.Mode = DataContextMode.Live;
            mockDataManager.AddElementContext(mockElementContext);

            Registrar registrar = new Registrar();
            TreeWalkerDataContext treeWalkerDataContext = new TreeWalkerDataContext(registrar, CancellationToken.None);

            mockActionContext = new Mock<IActionContext>(MockBehavior.Strict);
            mockActionContext.Setup(m => m.DataManager).Returns(mockDataManager);
            mockActionContext.Setup(m => m.Registrar).Returns(registrar);
            mockActionContext.Setup(m => m.TreeWalkerDataContext).Returns(treeWalkerDataContext);
        }

        [TestMethod]
        [Timeout(1000)]
        public void SetLiveModeDataContext_NoopsIfElementAlreadyHasComparableDataContext()
        {
            mockDataContext.Mode = DataContextMode.Live;

            using (new CaptureActionTestHookOverrides(null, null))
            {
                CaptureAction.SetLiveModeDataContext(mockElementContext.Id, mockTreeViewMode, mockActionContext.Object, force: false);
            }

            Assert.AreSame(mockDataContext, mockElementContext.DataContext);
        }

        [DataTestMethod]
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
            if (startWithNullDataContext) { mockElementContext.DataContext = null; }
            mockDataContext.Mode = originalDataContextMode;
            mockDataContext.TreeMode = originalDataContextTreeViewMode;
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
                CaptureAction.SetLiveModeDataContext(mockElementContext.Id, treeViewMode, mockActionContext.Object, force);

                // Assert
                Assert.IsNotNull(mockElementContext.DataContext);
                Assert.AreNotSame(mockDataContext, mockElementContext.DataContext);
                var result = mockElementContext.DataContext;
                Assert.AreSame(mockElement, result.Element);
                Assert.AreEqual(20000, result.ElementCounter.UpperBound);
                Assert.AreEqual(treeViewMode, result.TreeMode);
                Assert.AreEqual(DataContextMode.Live, result.Mode);
                Assert.AreEqual(mockTopMostElement, result.RootElment);
                mockTreeWalkerForLive.Verify(w => w.GetTreeHierarchy(mockElement, treeViewMode));
                Assert.AreEqual(2, result.Elements.Count);
                Assert.AreSame(mockElementsItem1, result.Elements[mockElementsItem1.UniqueId]);
                Assert.AreSame(mockElementsItem2, result.Elements[mockElementsItem2.UniqueId]);
            }
        }

        [DataTestMethod]
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
            mockDataContext.Mode = originalMode;
            mockDataContext.TreeMode = originalTreeMode;

            using (new CaptureActionTestHookOverrides(null, null))
            {
                bool retVal = CaptureAction.SetTestModeDataContext(mockElementContext.Id, newlySetMode, newlySetTreeMode, mockActionContext.Object, force: false);

                Assert.IsFalse(retVal);
                Assert.AreSame(mockDataContext, mockElementContext.DataContext);
            }
        }

        [DataTestMethod]
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
            if (startWithNullDataContext) { mockElementContext.DataContext = null; }
            mockDataContext.Mode = originalDataContextMode;
            mockDataContext.TreeMode = originalDataContextTreeViewMode;
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
                bool retVal = CaptureAction.SetTestModeDataContext(mockElementContext.Id, DataContextMode.Test, treeViewMode, mockActionContext.Object, force);
                // Assert
                Assert.IsTrue(retVal);
                Assert.IsNotNull(mockElementContext.DataContext);
                Assert.AreNotSame(mockDataContext, mockElementContext.DataContext);
                var result = mockElementContext.DataContext;
                Assert.AreSame(mockElement, result.Element);
                Assert.AreEqual(20000, result.ElementCounter.UpperBound);
                Assert.AreEqual(treeViewMode, result.TreeMode);
                Assert.AreEqual(DataContextMode.Test, result.Mode);
                Assert.AreEqual(mockTopMostElement, result.RootElment);
                mockTreeWalkerForTest.Verify(w => w.RefreshTreeData(treeViewMode, It.IsAny<TreeWalkerDataContext>()));
                Assert.AreEqual(2, result.Elements.Count);
                Assert.AreSame(mockElementsItem1, result.Elements[mockElementsItem1.UniqueId]);
                Assert.AreSame(mockElementsItem2, result.Elements[mockElementsItem2.UniqueId]);
            }
        }

        [DataTestMethod]
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
            if (startWithNullDataContext) { mockElementContext.DataContext = null; }
            mockDataContext.Mode = originalDataContextMode;
            mockDataContext.TreeMode = originalDataContextTreeViewMode;
            TreeViewMode treeViewMode = TreeViewMode.Raw;

            var mockOriginAncestor = mockElement;
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
                bool retVal = CaptureAction.SetTestModeDataContext(mockElementContext.Id, DataContextMode.Load, treeViewMode, mockActionContext.Object, force);

                // Assert
                Assert.IsTrue(retVal);
                Assert.IsNotNull(mockElementContext.DataContext);
                Assert.AreNotSame(mockDataContext, mockElementContext.DataContext);
                var result = mockElementContext.DataContext;
                Assert.AreSame(mockElement, result.Element);
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
