// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using System.Drawing;
using System.Windows.Forms;

namespace Axe.Windows.Actions
{
    internal static class PrivacyExtensions
    {
        /// <summary>
        /// Given the root to an element tree, synthesize a "yellow box" bitmap to describe it
        /// </summary>
        /// <param name="rootElement">The root element for the tree</param>
        /// <returns>A bitmap representing the element tee (normalized to having the app window at location (0,0)</returns>
        internal static Bitmap SynthesizeBitmapFromElements(this IA11yElement rootElement)
        {
            const int penWidth = 4;  // This is the same width as our highlighter

            if (rootElement == null) return null;

            Rectangle rootBoundingRectangle = rootElement.BoundingRectangle;

            // Synthesize the bitmap
            bool isHighContrast = SystemInformation.HighContrast;
            Color brushColor = isHighContrast ? SystemColors.Window : Color.DarkGray;
            Color penColor = isHighContrast ? SystemColors.WindowFrame : Color.Yellow;
            Bitmap bitmap = new Bitmap(rootBoundingRectangle.Width, rootBoundingRectangle.Height);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            using (SolidBrush brush = new SolidBrush(brushColor))
            using (Pen pen = new Pen(penColor, penWidth))
            {
                graphics.FillRectangle(brush, 0, 0, rootBoundingRectangle.Width, rootBoundingRectangle.Height);
                graphics.TranslateTransform(-rootBoundingRectangle.Left, -rootBoundingRectangle.Top);
                AddBoundingRectsRecursively(graphics, pen, rootElement);
            }

            return bitmap;
        }

        /// <summary>
        /// Add the bounding rect for this element to the Graphics surface
        /// </summary>
        /// <param name="graphics">Graphics surface to manipulate</param>
        /// <param name="pen">The pen to use (cached for performance)</param>
        /// <param name="element">The element to add--its children will also be added</param>
        private static void AddBoundingRectsRecursively(Graphics graphics, Pen pen, IA11yElement element)
        {
            graphics.DrawRectangle(pen, element.BoundingRectangle);

            if (element.Children != null)
            {
                foreach (var child in element.Children)
                {
                    AddBoundingRectsRecursively(graphics, pen, child);
                }
            }
        }
    }
}
