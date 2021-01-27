// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class BoundingRectangleSizeReasonableTest
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.BoundingRectangleSizeReasonable();

        [TestMethod]
        public void TestBoundingRectangleSizeReasonablePass()
        {
            Rectangle rect = new Rectangle(0, 0, 13, 2);

            using (var e = new MockA11yElement())
            {
                e.BoundingRectangle = rect;
                Assert.IsTrue(Rule.PassesTest(e));
            } // using
        }

        [TestMethod]
        public void TestBoundingRectangleSizeReasonableEmptyFail()
        {
            using (var e = new MockA11yElement())
            {
                e.BoundingRectangle = Rectangle.Empty;
                Assert.IsFalse(Rule.PassesTest(e));
            } // using
        }

        [TestMethod]
        public void TestBoundingRectangleSizeReasonableNotMinAreaFail()
        {
            using (var e = new MockA11yElement())
            {
                e.BoundingRectangle = new Rectangle(0, 0, 12, 2);
                Assert.IsFalse(Rule.PassesTest(e));
            } // using
        }

        [TestMethod]
        public void TestBoundingRectangleSizeReasonableArgumentException()
        {
            Action action = () => Rule.PassesTest(null);
            Assert.ThrowsException<ArgumentNullException>(action);
        }

        [TestMethod]
        public void BoundingRectangleSizeReasonable_EmptyTextNonApplicable()
        {
            var e = new MockA11yElement();

            // valid rectangle, no name, IsKeyboardFocusable false, no children

            e.BoundingRectangle = new Rectangle(0, 0, 2, 2);
            e.ControlTypeId = ControlType.Text;

            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void BoundingRectangleSizeReasonable_SeparatorNonApplicable()
        {
            var e = new MockA11yElement();

            // valid rectangle, no name, IsKeyboardFocusable false, no children

            e.BoundingRectangle = new Rectangle(0, 0, 2, 2);
            e.ControlTypeId = ControlType.Separator;

            Assert.IsFalse(Rule.Condition.Matches(e));
        }
    }// class
} // namespace
