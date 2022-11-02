// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class ControlShouldSupportExpandCollapsePatternTests
    {
        private Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.ControlShouldSupportExpandCollapsePattern();

        /// <summary>
        /// Condition should not match since TreeItem doesn't have any childTreeItem
        /// </summary>
        [TestMethod]
        public void TestTreeItemWithoutChildTreeItem()
        {
            var e = new MockA11yElement();
            var ec = new MockA11yElement();

            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_TreeItemControlTypeId;
            ec.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_TitleBarControlTypeId;

            e.Children.Add(ec);

            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        /// <summary>
        /// Condition should match since TreeItem has a treeitem child
        /// but ExpandCollapse is not supported. so scan fails.
        /// </summary>
        [TestMethod]
        public void TestTreeItemWithChildTreeItemWithoutExpandCollapsePattern()
        {
            var e = new MockA11yElement();
            var ec = new MockA11yElement();

            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_TreeItemControlTypeId;
            ec.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_TreeItemControlTypeId;

            e.Children.Add(ec);

            Assert.IsTrue(Rule.Condition.Matches(e));
            Assert.IsFalse(Rule.PassesTest(e));
        }

        /// <summary>
        /// Condition should match since TreeItem has a treeitem child
        /// and ExpandCollapse is supported. so scan succeeds
        /// </summary>
        [TestMethod]
        public void TestTreeItemWithChildTreeItemWithExpandCollapsePattern()
        {
            var e = new MockA11yElement();
            var ec = new MockA11yElement();

            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_TreeItemControlTypeId;
            ec.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_TreeItemControlTypeId;

            e.Children.Add(ec);
            e.Patterns.Add(new Core.Bases.A11yPattern(e, PatternType.UIA_ExpandCollapsePatternId));

            Assert.IsTrue(Rule.Condition.Matches(e));
            Assert.IsTrue(Rule.PassesTest(e));
        }

        /// <summary>
        /// Condition should match since SplitButton has a child text item
        /// and ExpandCollapse is supported. so scan succeeds
        /// </summary>
        [TestMethod]
        public void TestSplitButtonWithChildItem()
        {
            var e = new MockA11yElement();
            var ec = new MockA11yElement();

            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_SplitButtonControlTypeId;
            ec.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_TextControlTypeId;

            e.Children.Add(ec);
            e.Patterns.Add(new Core.Bases.A11yPattern(e, PatternType.UIA_ExpandCollapsePatternId));
            ec.Parent = e;

            Assert.IsTrue(Rule.Condition.Matches(e));
            Assert.IsTrue(Rule.PassesTest(e));
        }

        /// <summary>
        /// Condition should not match since SplitButton has a child SplitButton
        /// and ExpandCollapse is supported. so scan succeeds
        /// </summary>
        [TestMethod]
        public void TestSplitButtonWithChildSplitButton()
        {
            var e = new MockA11yElement();
            var ec = new MockA11yElement();

            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_SplitButtonControlTypeId;
            ec.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_SplitButtonControlTypeId;

            e.Children.Add(ec);
            e.Patterns.Add(new Core.Bases.A11yPattern(e, PatternType.UIA_ExpandCollapsePatternId));
            ec.Parent = e;

            Assert.IsTrue(Rule.Condition.Matches(e));
            Assert.IsFalse(Rule.Condition.Matches(ec));
            Assert.IsTrue(Rule.PassesTest(e));
        }


        /// <summary>
        /// Condition should match since SplitButton is child of a Pane
        /// and ExpandCollapse is supported. so scan succeeds
        /// </summary>
        [TestMethod]
        public void TestPaneWithSplitButtonChild()
        {
            var e = new MockA11yElement();
            var ec = new MockA11yElement();

            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_PaneControlTypeId;
            ec.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_SplitButtonControlTypeId;

            e.Children.Add(ec);
            ec.Patterns.Add(new Core.Bases.A11yPattern(e, PatternType.UIA_ExpandCollapsePatternId));
            ec.Parent = e;

            Assert.IsFalse(Rule.Condition.Matches(e));
            Assert.IsTrue(Rule.Condition.Matches(ec));
            Assert.IsTrue(Rule.PassesTest(ec));
        }
    }
}
