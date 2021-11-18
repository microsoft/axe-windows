// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    internal class RowColorAccumulator
    {
        private class ColorVoteInfo
        {
            public Color Color { get; }
            public int Votes { get; }
            public Confidence Confidence { get; }

            public ColorVoteInfo(Color color, int votes, Confidence confidence)
            {
                Color = color;
                Votes = votes;
                Confidence = confidence;
            }
        }

        private readonly List<SimpleRowResult> _rowResults = new List<SimpleRowResult>();

        internal void AddRowResult(SimpleRowResult rowResult)
        {
            _rowResults.Add(rowResult);
        }

        internal IColorContrastResult GetResult()
        {
            ColorVoteInfo backgroundInfo = GetBackgroundInfo();
            ColorVoteInfo foregroundInfo = GetForegroundInfo(backgroundInfo.Color);

            // TODO: Non-trivial cases

            return new SimpleColorContrastResult(
                new ColorPair(backgroundInfo.Color, foregroundInfo.Color),
                CombinedConfidence(backgroundInfo.Confidence, foregroundInfo.Confidence));
        }

        private ColorVoteInfo GetBackgroundInfo()
        {
            CountMap<Color> backgroundColors = new CountMap<Color>();

            _rowResults.ForEach(r =>
            {
                Color backgroundColor = r.BackgroundColor;
                if (backgroundColor != null)
                {
                    backgroundColors.Increment(r.BackgroundColor);
                }
            });

            var backgroundColorByFrequency = backgroundColors.OrderByDescending(x => x.Value);

            return MeasureConfidence(backgroundColorByFrequency);
        }

        private  ColorVoteInfo GetForegroundInfo(Color backgroundColor)
        {
            CountMap<Color> foregroundColors = new CountMap<Color>();

            _rowResults.ForEach(r =>
            {
                Color foregroundColor = r.ForegroundColor;
                Color testBackgroundColor = r.BackgroundColor;
                if (foregroundColor != null && backgroundColor.Equals(testBackgroundColor))
                {
                    foregroundColors.Increment(r.ForegroundColor);
                }
            });

            if (!foregroundColors.Any())
            {
                return null;
            }

            var foregroundColorByContrast = foregroundColors.OrderByDescending(x =>
            {
                ColorPair cp = new ColorPair(backgroundColor, x.Key);
                return cp.ColorContrast();
            });

            CountMap<Color> adjustColorsByContrast = new CountMap<Color>();
            Color currentSimilarColor = null;

            foreach (var pair in foregroundColorByContrast)
            {
                if (currentSimilarColor == null)
                {
                    adjustColorsByContrast.Increment(pair.Key, pair.Value);
                    currentSimilarColor = pair.Key;
                }
                else if (currentSimilarColor.IsSimilarColor(pair.Key))
                {
                    adjustColorsByContrast.Increment(currentSimilarColor, pair.Value);
                }
                else
                {
                    adjustColorsByContrast.Increment(pair.Key, pair.Value);
                    currentSimilarColor = pair.Key;
                }
            }

            var similarColorFrequencies = adjustColorsByContrast.OrderByDescending(x => x.Value);

            return MeasureConfidence(similarColorFrequencies);
        }

        private static ColorVoteInfo MeasureConfidence(IOrderedEnumerable<KeyValuePair<Color, int>> colorInputs)
        {
            var inputsAsList = new List<KeyValuePair<Color, int>>(colorInputs);
            int pluralityInputs = 0;
            int pluralityVotes = 0;
            int totalVotes = 0;

            foreach (var colorInput in inputsAsList)
            {
                totalVotes += colorInput.Value;
            }

            int plurality = totalVotes / 2 + 1;

            foreach (var colorCount in inputsAsList)
            {
                pluralityInputs++;
                pluralityVotes += colorCount.Value;

                if (pluralityVotes >= plurality)
                {
                    break;
                }
            }

            return new ColorVoteInfo(
                inputsAsList.First().Key,
                inputsAsList.First().Value,
                DetermineConfidence(pluralityInputs, inputsAsList.Count));
        }

        private static Confidence DetermineConfidence(int pluralityBlocks, int totalBlocks)
        {
            switch (pluralityBlocks)
            {
                case 1: return Confidence.High;  // Plurality in 1 block
                case 2: return Confidence.Mid;   // Plurality in 2 blocks
            }

            // No confidence if we had to consider at least half the blocks
            return ((2 * pluralityBlocks) < totalBlocks) ? Confidence.Low : Confidence.None;
        }

        private static Confidence CombinedConfidence(Confidence confidence1, Confidence confidence2)
        {
            int combinedConfidence = Math.Min((int)confidence1, (int)confidence2);
            return (Confidence)combinedConfidence;
        }
    }
}
