// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.SystemAbstractions;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Axe.Windows.Desktop.Drawing
{
    /// <summary>
    /// Implementation of <see cref="IBitmap"/> using System.Drawing.Common
    /// </summary>
    internal class FrameworkBitmap : IBitmap
    {
        private Bitmap _bitmap;

        #region IDisposable Support
        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _bitmap.Dispose();
                }

                disposedValue = true;
            }
        }

        ~FrameworkBitmap()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// Testable ctor
        /// </summary>
        internal FrameworkBitmap(Bitmap bitmap)
        {
            _bitmap = bitmap ?? throw new ArgumentNullException(nameof(bitmap));
        }

        /// <summary>
        /// Implementation of <see cref="IBitmap.SavePngToStream(Stream)"/>
        /// </summary>
        public void SavePngToStream(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            _bitmap.Save(stream, ImageFormat.Png);
        }

        /// <summary>
        /// Implementation of <see cref="IBitmap.ToSystemDrawingBitmap()"/>
        /// </summary>
        public Bitmap ToSystemDrawingBitmap()
        {
            return _bitmap;
        }
    }
}
