// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class BoundingRectangleOnUWPMenuItemTest
    {
            private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.BoundingRectangleOnUWPMenuItem();
[TestMethod]
        public void TestBoundingRectangleOnUWPMenuItemPass()
        {
            using (var e = new MockA11yElement())
            {
                e.BoundingRectangle = new Rectangle(0, 0, 2, 2);

                Assert.IsTrue(Rule.PassesTest(e));
            } // using
        }

        [TestMethod]
        public void TestBoundingRectangleOnUWPMenuItemNotPass()
        {
            using (var e = new MockA11yElement())
            {
                Assert.IsFalse(Rule.PassesTest(e));
            } // using
        }
    } // class
} // namespace
