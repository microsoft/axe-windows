// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class BoundingRectangleNotValidButOffScreenTests
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.BoundingRectangleNotValidButOffScreen();

        [TestMethod]
        public void TestBoundingRectangleNotValidButOffScreenInformation()
        {
            Assert.IsFalse(Rule.PassesTest(null));
        }
    }
}
