// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Drawing;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class ClickablePointTests
    {
        private Mock<IA11yElement> mockElement = new Mock<IA11yElement>(MockBehavior.Strict);
        private delegate void TryGetDelegate(int propertyId, out Point value);

        [TestCleanup]
        public void TestCleanup()
        {
            mockElement.Reset();
        }

        private void SetupTryGetProperty(Point outVal, bool retVal = true)
        {
            mockElement.Setup(m => m.TryGetPropertyValue<Point>(PropertyType.UIA_ClickablePointPropertyId, out It.Ref<Point>.IsAny))
                .Callback(new TryGetDelegate((int _, out Point p) =>
                {
                    p = outVal;
                }))
                .Returns(retVal);
        }

        [TestMethod]
        public void ClickablePoint_OnScreen_True()
        {
            SetupTryGetProperty(new Point(100, 100));
            Assert.IsTrue(ClickablePoint.OnScreen.Matches(mockElement.Object));
            mockElement.VerifyAll();
        }

        [TestMethod]
        public void ClickablePoint_OnScreen_OffLeft_False()
        {
            SetupTryGetProperty(new Point(-100, 100));
            Assert.IsFalse(ClickablePoint.OnScreen.Matches(mockElement.Object));
            mockElement.VerifyAll();
        }

        [TestMethod]
        public void ClickablePoint_OnScreen_OffRightt_False()
        {
            SetupTryGetProperty(new Point(10000, 100));
            Assert.IsFalse(ClickablePoint.OnScreen.Matches(mockElement.Object));
            mockElement.VerifyAll();
        }

        [TestMethod]
        public void ClickablePoint_OnScreen_OffTop_False()
        {
            SetupTryGetProperty(new Point(100, -100));
            Assert.IsFalse(ClickablePoint.OnScreen.Matches(mockElement.Object));
            mockElement.VerifyAll();
        }

        [TestMethod]
        public void ClickablePoint_OnScreen_OffBottom_False()
        {
            SetupTryGetProperty(new Point(100, 10000));
            Assert.IsFalse(ClickablePoint.OnScreen.Matches(mockElement.Object));
            mockElement.VerifyAll();
        }

        [TestMethod]
        public void ClickablePoint_OnScreen_NoProperty_False()
        {
            SetupTryGetProperty(new Point(100, 100), false);
            Assert.IsFalse(ClickablePoint.OnScreen.Matches(mockElement.Object));
            mockElement.VerifyAll();
        }

        [TestMethod]
        public void ClickablePoint_OnScreen_NullElement_Throws()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ClickablePoint.OnScreen.Matches(null));
        }

        [TestMethod]
        public void ClickablePoint_OffScreen_False()
        {
            SetupTryGetProperty(new Point(100, 100));
            Assert.IsFalse(ClickablePoint.OffScreen.Matches(mockElement.Object));
            mockElement.VerifyAll();
        }

        [TestMethod]
        public void ClickablePoint_OffScreen_OffLeft_True()
        {
            SetupTryGetProperty(new Point(-100, 100));
            Assert.IsTrue(ClickablePoint.OffScreen.Matches(mockElement.Object));
            mockElement.VerifyAll();
        }

        [TestMethod]
        public void ClickablePoint_OffScreen_OffRightt_True()
        {
            SetupTryGetProperty(new Point(10000, 100));
            Assert.IsTrue(ClickablePoint.OffScreen.Matches(mockElement.Object));
            mockElement.VerifyAll();
        }

        [TestMethod]
        public void ClickablePoint_OffScreen_OffTop_True()
        {
            SetupTryGetProperty(new Point(100, -100));
            Assert.IsTrue(ClickablePoint.OffScreen.Matches(mockElement.Object));
            mockElement.VerifyAll();
        }

        [TestMethod]
        public void ClickablePoint_OffScreen_OffBottom_True()
        {
            SetupTryGetProperty(new Point(100, 10000));
            Assert.IsTrue(ClickablePoint.OffScreen.Matches(mockElement.Object));
            mockElement.VerifyAll();
        }

        [TestMethod]
        public void ClickablePoint_OffScreen_NoProperty_False()
        {
            SetupTryGetProperty(new Point(100, 100), false);
            Assert.IsFalse(ClickablePoint.OffScreen.Matches(mockElement.Object));
            mockElement.VerifyAll();
        }

        [TestMethod]
        public void ClickablePoint_OffScreen_NullElement_Throws()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ClickablePoint.OffScreen.Matches(null));
        }
    } // class
} // namespace
