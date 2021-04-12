// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Drawing;
using System.IO;

namespace Axe.Windows.SystemAbstractions
{
    /// <summary>
    /// Interface to encapsulate Bitmap-related operations
    /// </summary>
    public interface IBitmap : IDisposable
    {
        /// <summary>
        /// Save the current contents to the provided stream, in PNG format
        /// </summary>
        /// <param name="stream">The stream to receive the PNG data</param>
        void SavePngToStream(Stream stream);

        /// <summary>
        /// Convert the current contents to a System.Drawing.Bitmap object.
        /// This is *not* used during capture, so capture-only implementations
        /// should simply throw a NotImplementedException.
        /// </summary>
        /// <returns></returns>
        Bitmap ToSystemDrawingBitmap();
    }
}
