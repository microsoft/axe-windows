// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class SiblingUniqueAndFocusableTests
    {
        private static readonly Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.SiblingUniqueAndFocusable();

        private static MockA11yElement CreateParentWithMatchingChildren()
        {
            var parent = new MockA11yElement();
            var child1 = new MockA11yElement();
            var child2 = new MockA11yElement();
            child1.BoundingRectangle = new Rectangle(0, 0, 25, 25);
            child2.BoundingRectangle = new Rectangle(0, 0, 25, 25);
            child1.IsContentElement = true;
            child2.IsContentElement = true;
            child1.LocalizedControlType = "MyType1";
            child2.LocalizedControlType = "MyType1";
            child1.Name = "Alice";
            child2.Name = "Alice";
            child1.IsKeyboardFocusable = true;
            child2.IsKeyboardFocusable = true;
            child1.Parent = parent;
            child2.Parent = parent;
            parent.Children.Add(child1);
            parent.Children.Add(child2);

            return parent;
        }

        [TestMethod]
        public void TestTypeMismatchPass()
        {
            var parent = CreateParentWithMatchingChildren();
            var child2 = parent.Children[1] as MockA11yElement;
            child2.LocalizedControlType = "MyType2";

            Assert.IsTrue(Rule.PassesTest(child2));
        }

        [TestMethod]
        public void TestNameMismatchPass()
        {
            var parent = CreateParentWithMatchingChildren();
            var child2 = parent.Children[1] as MockA11yElement;
            child2.Name = "Bob";

            Assert.IsTrue(Rule.PassesTest(child2));
        }

        [TestMethod]
        public void TestFocusableMismatchPass()
        {
            var parent = CreateParentWithMatchingChildren();
            var child2 = parent.Children[1] as MockA11yElement;
            child2.IsKeyboardFocusable = false;

            Assert.IsTrue(Rule.PassesTest(child2));
        }

        [TestMethod]
        public void TestGridItemPatternPass()
        {
            var parent = CreateParentWithMatchingChildren();
            var child2 = parent.Children[1] as MockA11yElement;
            child2.Patterns.Add(new A11yPattern(child2, PatternIDs.GridItem));

            Assert.IsTrue(Rule.PassesTest(child2));
        }

        [TestMethod]
        public void TestMatchError()
        {
            var parent = CreateParentWithMatchingChildren();
            var child2 = parent.Children[1] as MockA11yElement;

            Assert.IsFalse(Rule.PassesTest(child2));
        }
    } // class
} // namespace
