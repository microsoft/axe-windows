// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.Drawing;
using Axe.Windows.SystemAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Axe.Windows.DesktopTests.Drawing
{
    [TestClass]
    public class FrameworkBitmapCreatorTests
    {
        Bitmap GetBitmapForTest()
        {
            const int testWidth = 20;
            const int testHeight = 40;
            const PixelFormat testFormat = PixelFormat.Format8bppIndexed;

            return new Bitmap(testWidth, testHeight, testFormat);
        }

        private void FailingScreenCapture(Bitmap bitmap, Rectangle rect)
        {
            Assert.Fail("This method should never get called!");
        }

        [TestMethod]
        public void Ctor_DefaultVersion_DoesNotThrow()
        {
            IBitmapCreator testSubject = new FrameworkBitmapCreator();
            Assert.IsInstanceOfType(testSubject, typeof(FrameworkBitmapCreator));
        }

        [TestMethod]
        public void Ctor_ScreenCaptureIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(() => new FrameworkBitmapCreator(null));
            Assert.AreEqual("screenCapture", e.ParamName);
        }

        [TestMethod]
        public void FromSystemDrawingBitmap_ReturnsCorrectType()
        {
            IBitmapCreator testSubject = new FrameworkBitmapCreator(FailingScreenCapture);
            using (IBitmap createdBitmap = testSubject.FromSystemDrawingBitmap(GetBitmapForTest()))
            {
                Assert.IsInstanceOfType(createdBitmap, typeof(FrameworkBitmap));
            }
        }

        [TestMethod]
        public void FromScreenRectangle_CallsScreenCaptureWithExpectedParameters_ReturnsCorrectType()
        {
            Bitmap actualBitmap = null;
            Rectangle actualRectangle = new Rectangle();
            Rectangle expectedRectangle = new Rectangle(20, 30, 100, 300);
            IBitmapCreator testSubject = new FrameworkBitmapCreator((bitmap, rect) =>
            {
                actualBitmap = bitmap;
                actualRectangle = rect;
            });
            Assert.AreNotEqual(expectedRectangle, actualRectangle);

            using (IBitmap createdBitmap = testSubject.FromScreenRectangle(expectedRectangle))
            {
                Assert.IsInstanceOfType(createdBitmap, typeof(FrameworkBitmap));
                Assert.IsNotNull(actualBitmap);
                Assert.AreEqual(expectedRectangle.Width, actualBitmap.Width);
                Assert.AreEqual(expectedRectangle.Height, actualBitmap.Height);
                Assert.AreEqual(expectedRectangle, actualRectangle);
            }
        }
    }
}
