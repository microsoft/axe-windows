// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    public class Pixel
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        public Color Color { get; private set; }

        public Pixel(Color color, int row, int column)
        {
            Row = row;
            Column = column;
            Color = color;
        }
    }
}
