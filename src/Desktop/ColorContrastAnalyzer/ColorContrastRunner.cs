// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    /**
    * Manages running of the color contrast calculation. Collects transition and
    * color counts and attempts to come to conclusions based on these values.
    */
    internal class ColorContrastRunner
    {
        private readonly IColorContrastConfig _colorContrastConfig;

        private readonly Dictionary<Color, ColorContrastTransition> _openTransitions = new Dictionary<Color, ColorContrastTransition>();

        private readonly CountMap<Color> _countExactColors = new CountMap<Color>();

        private readonly CountMap<ColorPair> _countExactPairs = new CountMap<ColorPair>();

        internal ColorContrastRunner(IColorContrastConfig colorContrastConfig)
        {
            _colorContrastConfig = colorContrastConfig;
        }

        internal void OnPixel(Color color, Color previousColor)
        {
            _countExactColors.Increment(color);

            var newlyClosedTransitions = new List<ColorContrastTransition>();

            foreach (var transition in _openTransitions.Values)
            {
                transition.AddColor(color);

                if (transition.IsClosed)
                {
                    newlyClosedTransitions.Add(transition);

                    if (transition.IsPotentialForegroundBackgroundPair())
                    {
                        _countExactPairs.Increment(new ColorPair(
                            transition.StartingColor,
                            transition.MostContrastingColor
                        ));
                    }
                }
            }

            foreach (ColorContrastTransition transition in newlyClosedTransitions)
            {
                _openTransitions.Remove(transition.StartingColor);
            }

            if (previousColor != null && !color.Equals(previousColor))
            {
                _openTransitions[previousColor] = new ColorContrastTransition(previousColor, _colorContrastConfig);
            }
        }

        internal void OnRowBegin()
        {
            _openTransitions.Clear();
            _countExactPairs.Clear();
            _countExactColors.Clear();
        }

        // Returns true when entries have lead to a confident conclusion about Text and Background color.

        internal ColorContrastResult OnRowEnd()
        {
            ColorContrastResult result = new ColorContrastResult();

            CountMap<ColorPair> pairsWithSimilarTextColor = new CountMap<ColorPair>();

            foreach (var exactPairOuter in _countExactPairs)
            {
                foreach (var exactPairInner in _countExactPairs)
                {
                    if (exactPairOuter.Key.BackgroundColor.Equals(exactPairInner.Key.BackgroundColor))
                    {
                        if (exactPairOuter.Key.ForegroundColor.IsSimilarColor(exactPairInner.Key.ForegroundColor))
                        {
                            pairsWithSimilarTextColor.Increment(exactPairOuter.Key, exactPairInner.Value);
                        }
                    }
                }
            }

            var sortedByValueAndContrast = pairsWithSimilarTextColor.OrderByDescending(x => x.Value)
                .ThenByDescending(x => x.Key.ColorContrast());

            if (!sortedByValueAndContrast.Any()) return result;

            var resultPairs = new HashSet<ColorPair>();

            var firstEntryCount = sortedByValueAndContrast.First().Value;

            if (firstEntryCount < _colorContrastConfig.MinNumberColorTransitions) return result;

            var firstEntryCountAdjusted = firstEntryCount / _colorContrastConfig.TransitionCountDominanceFactor;

            foreach (var entry in sortedByValueAndContrast)
            {
                // Only Collect Pairs that have a reasonable occurrence count.
                if (entry.Value < firstEntryCountAdjusted) break;

                resultPairs.Add(entry.Key);
            }

            foreach (var colorPair in resultPairs)
            {
                result.Add(colorPair);
            }

            _countExactColors.Clear();

            _openTransitions.Clear();

            return result;
        }
    }
}
