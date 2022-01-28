// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    /// <summary>
    /// Runs color contrast V2 analysis
    /// </summary>
    internal class ColorContrastRunnerV2
    {
        private readonly IColorContrastConfig _colorContrastConfig;

        private CountMap<Color> _countExactColors = new CountMap<Color>();

        internal ColorContrastRunnerV2(IColorContrastConfig colorContrastConfig)
        {
            _colorContrastConfig = colorContrastConfig;
        }

        internal void OnPixel(Color color)
        {
            _countExactColors.Increment(color);
        }

        internal RowResultV2 OnRowEnd(int countOfPixelsInRow)
        {
            RowResultV2 result = new RowResultV2();

            var colorsByFrequency = _countExactColors.OrderByDescending(x => x.Value);

            // Assume that the background color is the most common color in the row
            var backgroundColor = colorsByFrequency.First().Key;

            Color foregroundColor = FindForegroundColor(backgroundColor);

            _countExactColors.Clear();

            return new RowResultV2(backgroundColor, foregroundColor);
        }

        private Color FindForegroundColor(Color backgroundColor)
        {
            var contrastingColors = _countExactColors.OrderByDescending(x =>
            {
                ColorPair cp = new ColorPair(backgroundColor, x.Key);
                return cp.ColorContrast();
            });

            // Assume that the foreground color has the greatest contrast from the background
            var mostContrastingColor = contrastingColors.First().Key;
            var foregroundColor = mostContrastingColor.Equals(backgroundColor) ?
                null : mostContrastingColor;

            return foregroundColor;
        }
    }
}
