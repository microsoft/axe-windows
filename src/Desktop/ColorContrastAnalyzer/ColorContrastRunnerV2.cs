// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    /// <summary>
    /// Runs color contrast V2 analysis
    /// </summary>
    internal class ColorContrastRunnerV2
    {
        private List<Color> _colorsInRow = new List<Color>();
        private CountMap<Color> _countExactColors = new CountMap<Color>();

        internal void OnPixel(Color color)
        {
            _colorsInRow.Add(color);
            _countExactColors.Increment(color);
        }

        internal RowResultV2 OnRowEnd()
        {
            Color backgroundColor = FindBackgroundColor();
            Color foregroundColor = FindForegroundColor(backgroundColor);
            int transitionCount = GetTransitionCount(backgroundColor);

            _countExactColors.Clear();
            _colorsInRow.Clear();

            return new RowResultV2(backgroundColor, foregroundColor, transitionCount);
        }

        private Color FindBackgroundColor()
        {
            var colorsByFrequency = _countExactColors.OrderByDescending(x => x.Value);

            // Assume that the most common color in the row is the background color
            return colorsByFrequency.First().Key;
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

        private int GetTransitionCount(Color backgroundColor)
        {
            int transitionCount = 0;
            Color previousColor = _colorsInRow.First();

            foreach (Color pixelColor in _colorsInRow)
            {
                if (!pixelColor.Equals(previousColor))
                {
                    if (pixelColor.Equals(backgroundColor))
                    {
                        transitionCount++;
                    }
                    previousColor = pixelColor;
                }
            }

            return transitionCount;
        }
    }
}
