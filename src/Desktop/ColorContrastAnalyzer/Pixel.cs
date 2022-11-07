// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    public class Pixel
    {
        public int Row { get; }
        public int Column { get; }
        public Color Color { get; }

        public Pixel(Color color, int row, int column)
        {
            Row = row;
            Column = column;
            Color = color;
        }
    }
}
