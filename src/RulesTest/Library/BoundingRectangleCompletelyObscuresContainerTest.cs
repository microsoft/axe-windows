// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class BoundingRectangleCompletelyObscuresContainerTest
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.BoundingRectangleCompletelyObscuresContainer();
        private static Rectangle TestRect = new Rectangle(300, 300, 400, 400);
        private static Rectangle ValidRect = new Rectangle(TestRect.Left - BoundingRectangle.OverlapMargin,
            TestRect.Top - BoundingRectangle.OverlapMargin,
            TestRect.Size.Width + (BoundingRectangle.OverlapMargin * 2),
            TestRect.Size.Height + (BoundingRectangle.OverlapMargin * 2));
        private static Rectangle ErrorRect = new Rectangle(ValidRect.Left - 1,
            ValidRect.Top - 1,
            ValidRect.Size.Width + 2,
            ValidRect.Size.Height + 2);

        [TestMethod]
        public void BoundingRectangleCompletelyObscuresContainer_LeftPass()
        {
            var e = new MockA11yElement();
            var parent = new MockA11yElement();

            e.BoundingRectangle = new Rectangle(ErrorRect.Left + 2,
                ErrorRect.Top,
                ErrorRect.Size.Width,
                ErrorRect.Size.Height);

            parent.BoundingRectangle = TestRect;
            e.Parent = parent;

            Assert.IsTrue(Rule.Condition.Matches(e));
            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void BoundingRectangleCompletelyObscuresContainer_TopPass()
        {
            var e = new MockA11yElement();
            var parent = new MockA11yElement();

            e.BoundingRectangle = new Rectangle(ErrorRect.Left,
                ErrorRect.Top + 2,
                ErrorRect.Size.Width,
                ErrorRect.Size.Height);

            parent.BoundingRectangle = TestRect;
            e.Parent = parent;

            Assert.IsTrue(Rule.Condition.Matches(e));
            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void BoundingRectangleCompletelyObscuresContainer_RightPass()
        {
            var e = new MockA11yElement();
            var parent = new MockA11yElement();

            e.BoundingRectangle = new Rectangle(ErrorRect.Left,
                ErrorRect.Top,
                ErrorRect.Size.Width - 2,
                ErrorRect.Size.Height);

            parent.BoundingRectangle = TestRect;
            e.Parent = parent;

            Assert.IsTrue(Rule.Condition.Matches(e));
            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void BoundingRectangleCompletelyObscuresContainer_BottomPass()
        {
            var e = new MockA11yElement();
            var parent = new MockA11yElement();

            e.BoundingRectangle = new Rectangle(ErrorRect.Left,
                ErrorRect.Top,
                ErrorRect.Size.Width,
                ErrorRect.Size.Height - 2);

            parent.BoundingRectangle = TestRect;
            e.Parent = parent;

            Assert.IsTrue(Rule.Condition.Matches(e));
            Assert.IsTrue(Rule.PassesTest(e));
        }

        [TestMethod]
        public void BoundingRectangleCompletelyObscuresContainer_LeftFail()
        {
            var e = new MockA11yElement();
            var parent = new MockA11yElement();

            e.BoundingRectangle = new Rectangle(ValidRect.Left - 1,
                ValidRect.Top,
                ValidRect.Size.Width + 1,
                ValidRect.Size.Height);

            parent.BoundingRectangle = TestRect;
            e.Parent = parent;

            Assert.IsTrue(Rule.Condition.Matches(e));
            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void BoundingRectangleCompletelyObscuresContainer_TopFail()
        {
            var e = new MockA11yElement();
            var parent = new MockA11yElement();

            e.BoundingRectangle = new Rectangle(ValidRect.Left,
                ValidRect.Top - 1,
                ValidRect.Size.Width,
                ValidRect.Size.Height + 1);

            parent.BoundingRectangle = TestRect;
            e.Parent = parent;

            Assert.IsTrue(Rule.Condition.Matches(e));
            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void BoundingRectangleCompletelyObscuresContainer_RightFail()
        {
            var e = new MockA11yElement();
            var parent = new MockA11yElement();

            e.BoundingRectangle = new Rectangle(ValidRect.Left,
                ValidRect.Top,
                ValidRect.Size.Width + 1,
                ValidRect.Size.Height);

            parent.BoundingRectangle = TestRect;
            e.Parent = parent;

            Assert.IsTrue(Rule.Condition.Matches(e));
            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void BoundingRectangleCompletelyObscuresContainer_BottomFail()
        {
            var e = new MockA11yElement();
            var parent = new MockA11yElement();

            e.BoundingRectangle = new Rectangle(ValidRect.Left,
                ValidRect.Top,
                ValidRect.Size.Width,
                ValidRect.Size.Height + 1);

            parent.BoundingRectangle = TestRect;
            e.Parent = parent;

            Assert.IsTrue(Rule.Condition.Matches(e));
            Assert.IsFalse(Rule.PassesTest(e));
        }

        [TestMethod]
        public void BoundingRectangleCompletelyObscuresContainer_IsDialog_NotApplicable()
        {
            var e = new MockA11yElement();
            var parent = new MockA11yElement();

            e.BoundingRectangle = TestRect;
            parent.BoundingRectangle = TestRect;
            e.Parent = parent;

            Assert.IsTrue(Rule.Condition.Matches(e));

            var a11yProperty = new A11yProperty(PropertyType.UIA_IsDialogPropertyId, true);
            e.Properties.Add(PropertyType.UIA_IsDialogPropertyId, a11yProperty);

            Assert.IsTrue(Rule.Condition.Matches(e));

            e.ControlTypeId = ControlType.Pane;

            Assert.IsFalse(Rule.Condition.Matches(e));

            e.Properties.Remove(PropertyType.UIA_IsDialogPropertyId);

            Assert.IsTrue(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void BoundingRectangleCompletelyObscuresContainer_Window_NotApplicable()
        {
            var e = new MockA11yElement();
            var parent = new MockA11yElement();

            e.BoundingRectangle = TestRect;
            parent.BoundingRectangle = TestRect;
            e.Parent = parent;

            Assert.IsTrue(Rule.Condition.Matches(e));

            e.ControlTypeId = ControlType.Window;

            Assert.IsFalse(Rule.Condition.Matches(e));
        }
    } // class
} // namespace
