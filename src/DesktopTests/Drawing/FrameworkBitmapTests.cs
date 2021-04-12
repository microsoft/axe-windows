// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.Drawing;
using Axe.Windows.SystemAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Axe.Windows.DesktopTests.Drawing
{
    [TestClass]
    public class FrameworkBitmapTests
    {
        Bitmap GetBitmapForTest()
        {
            const int testWidth = 20;
            const int testHeight = 40;
            const PixelFormat testFormat = PixelFormat.Format8bppIndexed;

            return new Bitmap(testWidth, testHeight, testFormat);
        }

        [TestMethod]
        [Timeout(1000)]
        public void Ctor_BitmapIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(() => new FrameworkBitmap(null));
            Assert.AreEqual("bitmap", e.ParamName);
        }

        [TestMethod]
        [Timeout(1000)]
        public void ToSystemDrawingBitmap_ReturnsOriginalBitmap()
        {
            Bitmap expectedBitmap = GetBitmapForTest();

            using (IBitmap testSubject = new FrameworkBitmap(expectedBitmap))
            {
                Bitmap actualBitmap = testSubject.ToSystemDrawingBitmap();
                Assert.AreEqual(expectedBitmap, actualBitmap);
                Assert.AreEqual(expectedBitmap.Width, actualBitmap.Width);
                Assert.AreEqual(expectedBitmap.Height, actualBitmap.Height);
                Assert.AreEqual(expectedBitmap.PixelFormat, actualBitmap.PixelFormat);
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void SavePngToStream_StreamIsNull_ThrowsArgumentNullException()
        {
            Bitmap expectedBitmap = GetBitmapForTest();

            using (IBitmap testSubject = new FrameworkBitmap(expectedBitmap))
            {
                ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(() => testSubject.SavePngToStream(null));
                Assert.AreEqual("stream", e.ParamName);
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void SavePngToStream_StreamIsValid_StreamContentIsCorrect()
        {
            Bitmap expectedBitmap = GetBitmapForTest();
            byte[] expectedContent;

            using (MemoryStream stream = new MemoryStream())
            {
                expectedBitmap.Save(stream, ImageFormat.Png);
                expectedContent = stream.ToArray();
            }

            using (IBitmap testSubject = new FrameworkBitmap(expectedBitmap))
            using (MemoryStream stream = new MemoryStream())
            {
                testSubject.SavePngToStream(stream);
                stream.Seek(0, SeekOrigin.Begin);
                byte[] actualContent = stream.ToArray();

                Assert.AreEqual(expectedContent.Length, actualContent.Length);
                for (int loop = 0; loop < expectedContent.Length; loop++)
                {
                    Assert.AreEqual(expectedContent[loop], actualContent[loop]);
                }
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void Dispose_DisposesBitmap()
        {
            Bitmap expectedBitmap = GetBitmapForTest();
            using (IBitmap testSubject = new FrameworkBitmap(expectedBitmap))
            {
                Assert.AreNotEqual(IntPtr.Zero, expectedBitmap.GetHbitmap());
            }
            ArgumentException e = Assert.ThrowsException<ArgumentException>(() => expectedBitmap.GetHbitmap());
        }

        [TestMethod]
        [Timeout(1000)]
        public void Dispose_CalledTwice_DoesNotThrow()
        {
            Bitmap expectedBitmap = GetBitmapForTest();
            IBitmap testSubject = new FrameworkBitmap(expectedBitmap);
            testSubject.Dispose();
            testSubject.Dispose();
        }
    }
}
