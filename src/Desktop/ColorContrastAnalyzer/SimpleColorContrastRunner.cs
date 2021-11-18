// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    /**
    * Manages running of the color contrast calculation. Collects transition and
    * color counts and attempts to come to conclusions based on these values.
    */
    internal class SimpleColorContrastRunner
    {
        private readonly IColorContrastConfig _colorContrastConfig;

        private CountMap<Color> countExactColors = new CountMap<Color>();

        internal SimpleColorContrastRunner(IColorContrastConfig colorContrastConfig)
        {
            _colorContrastConfig = colorContrastConfig;
        }

        internal void OnPixel(Color color)
        {
            countExactColors.Increment(color);
        }

        // Returns true when entries have lead to a confident conclusion about Text and Background color.

        internal SimpleRowResult OnRowEnd(int countOfPixelsInRow)
        {
            SimpleRowResult result = new SimpleRowResult();

            var colorsByFrequency = countExactColors.OrderByDescending(x => x.Value);

            // Assume that the background color is the most common color in the row
            var backgroundColor = colorsByFrequency.First().Key;

            Color foregroundColor = FindForegroundColor(backgroundColor);

            countExactColors.Clear();

            return new SimpleRowResult(backgroundColor, foregroundColor);
        }

        private Color FindForegroundColor(Color backgroundColor)
        {
            var contrastingColors = countExactColors.OrderByDescending(x =>
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
