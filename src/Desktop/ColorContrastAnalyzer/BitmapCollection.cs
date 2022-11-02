// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    public class BitmapCollection : ImageCollection
    {
        private readonly System.Drawing.Bitmap _bitmap;

        public BitmapCollection(System.Drawing.Bitmap bitmap, IColorContrastConfig colorContrastConfig)
            : base(colorContrastConfig)
        {
            _bitmap = bitmap;
        }

        public override int NumColumns()
        {
            return _bitmap.Width;
        }

        public override int NumRows()
        {
            return _bitmap.Height;
        }

        public override Color GetColor(int row, int column)
        {
            System.Drawing.Color color = _bitmap.GetPixel(column, row);

            return new Color(color);
        }
    }
}
