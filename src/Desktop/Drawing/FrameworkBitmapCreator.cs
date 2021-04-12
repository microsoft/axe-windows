// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.SystemAbstractions;
using System;
using System.Drawing;

namespace Axe.Windows.Desktop.Drawing
{
    /// <summary>
    /// Implementation of <see cref="IBitmapCreator"/> using System.Drawing.Common
    /// </summary>
    public class FrameworkBitmapCreator : IBitmapCreator
    {
        internal delegate void ScreenCapture(Bitmap bitmap, Rectangle rect);

        private readonly ScreenCapture _screenCapture;

        /// <summary>
        /// Production ctor
        /// </summary>
        public FrameworkBitmapCreator()
            : this(CaptureScreenRect)
        {
        }

        /// <summary>
        /// Testable ctor
        /// </summary>
        /// <param name="screenCapture">The method that gets called to capture the screenshot region</param>
        internal FrameworkBitmapCreator(ScreenCapture screenCapture)
        {
            _screenCapture = screenCapture ?? throw new ArgumentNullException(nameof(screenCapture));
        }

        private static void CaptureScreenRect(Bitmap bitmap, Rectangle rect)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size);
            }
        }

        /// <summary>
        /// Implementation of <see cref="IBitmapCreator.FromScreenRectangle(Rectangle)"/>
        /// </summary>
        public IBitmap FromScreenRectangle(Rectangle rect)
        {
            Bitmap bmp = new Bitmap(rect.Width, rect.Height);
            _screenCapture(bmp, rect);
            return new FrameworkBitmap(bmp);
        }

        /// <summary>
        /// Implementation of <see cref="IBitmapCreator.FromSystemDrawingBitmap(Bitmap)"/>
        /// </summary>
        public IBitmap FromSystemDrawingBitmap(Bitmap bitmap)
        {
            return new FrameworkBitmap(bitmap);
        }
    }
}
