// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Axe.Windows.SystemAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Axe.Windows.ActionsTests.Actions
{
    [TestClass]
    public class ScreenShotActionUnitTests
    {
        // This sets the Bounding Rectangle. It breaks encapsulation in
        // many reproachable ways
        private void SetBoundingRectangle(A11yElement element, Rectangle b)
        {
            int typeId = PropertyType.UIA_BoundingRectanglePropertyId;
            double[] data = { b.Left, b.Top, b.Right - b.Left, b.Bottom - b.Top };
            if (element.Properties == null)
                element.Properties = new Dictionary<int, A11yProperty>();
            element.Properties.Add(typeId, new A11yProperty(typeId, data));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ScreenShotAction.GetDataManager = () => DataManager.GetDefaultInstance();
        }

        [TestMethod]
        [Timeout(1000)]
        public void CaptureScreenShot_BitmapCreatorIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(() => ScreenShotAction.CaptureScreenShot(Guid.Empty, null));
            Assert.AreEqual("bitmapCreator", e.ParamName);
        }

        [TestMethod]
        [Timeout(2000)]
        public void CaptureScreenShot_ElementWithoutBoundingRectangle_NoScreenShot()
        {
            Mock<IBitmapCreator> bitmapCreatorMock = new Mock<IBitmapCreator>(MockBehavior.Strict);

            using (var dm = new DataManager())
            {
                // no bounding rectangle.
                A11yElement element = new A11yElement
                {
                    UniqueId = 42,
                };

                var dc = new ElementDataContext(element, 1);

                var elementContext = new ElementContext(element)
                {
                    DataContext = dc,
                };

                dm.AddElementContext(elementContext);
                ScreenShotAction.GetDataManager = () => dm;

                ScreenShotAction.CaptureScreenShot(elementContext.Id, bitmapCreatorMock.Object);

                Assert.IsNull(dc.Screenshot);
                Assert.AreEqual(default(int), dc.ScreenshotElementId);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void CaptureScreenShot_ElementWithBoundingRectangle_ScreenShotCreated()
        {
            Mock<IBitmap> bitmapMock = new Mock<IBitmap>(MockBehavior.Strict);
            Mock<IBitmapCreator> bitmapCreatorMock = new Mock<IBitmapCreator>(MockBehavior.Strict);

            using (var dm = new DataManager())
            {
                A11yElement element = new A11yElement
                {
                    UniqueId = 42,
                };

                Rectangle expectedRectangle = new Rectangle(10, 20, 100, 200);
                SetBoundingRectangle(element, expectedRectangle);

                var dc = new ElementDataContext(element, 1);

                var elementContext = new ElementContext(element)
                {
                    DataContext = dc,
                };

                dm.AddElementContext(elementContext);

                ScreenShotAction.GetDataManager = () => dm;
                bitmapCreatorMock.Setup(m => m.FromScreenRectangle(expectedRectangle)).Returns(bitmapMock.Object);

                ScreenShotAction.CaptureScreenShot(elementContext.Id, bitmapCreatorMock.Object);

                Assert.IsNotNull(dc.Screenshot);
                Assert.AreEqual(element.UniqueId, dc.ScreenshotElementId);
                bitmapCreatorMock.VerifyAll();
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void CaptureScreenShotOnWCOS_ElementWithBoundingRectangle_NoScreenShot()
        {
            Mock<IBitmapCreator> bitmapCreatorMock = new Mock<IBitmapCreator>(MockBehavior.Strict);

            using (var dm = new DataManager())
            {
                A11yElement element = new A11yElement
                {
                    UniqueId = 42,
                };

                Rectangle expectedRectangle = new Rectangle(10, 20, 100, 200);
                SetBoundingRectangle(element, expectedRectangle);

                var dc = new ElementDataContext(element, 1);

                var elementContext = new ElementContext(element)
                {
                    DataContext = dc,
                };

                dm.AddElementContext(elementContext);

                ScreenShotAction.GetDataManager = () => dm;
                bitmapCreatorMock.Setup(m => m.FromScreenRectangle(expectedRectangle)).Throws(new TypeInitializationException("Bitmap", null));

                ScreenShotAction.CaptureScreenShot(elementContext.Id, bitmapCreatorMock.Object);

                Assert.IsNull(dc.Screenshot);
                Assert.AreEqual(default(int), dc.ScreenshotElementId);
                bitmapCreatorMock.VerifyAll();
            }
        }
    }
}
