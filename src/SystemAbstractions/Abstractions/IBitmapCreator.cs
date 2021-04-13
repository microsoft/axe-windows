// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Drawing;

namespace Axe.Windows.SystemAbstractions
{
    /// <summary>
    /// Interface to encapsulate creation of IBitmap objects. To work correctly,
    /// implementions of this interface MUST be public and have a parameterless constructor
    /// </summary>
    public interface IBitmapCreator
    {
        /// <summary>
        /// Given a screen rectangle, capture the contents and return them
        /// in an IBitmap object
        /// </summary>
        /// <param name="rect">The screen coordinates</param>
        /// <returns>An IBitmap object containing the data.</returns>
        IBitmap FromScreenRectangle(Rectangle rect);

        /// <summary>
        /// Given a System.Drawing.Bitmap object, create a corresponding IBitmap
        /// object. This is not necessary for capture-only implementations, which
        /// should simply throw a NotImplementedException
        /// </summary>
        /// <param name="bitmap">The System.Drawing.Bitmap object</param>
        /// <returns>An IBitmap object containing equivalent data.</returns>
        IBitmap FromSystemDrawingBitmap(Bitmap bitmap);
    }
}
