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
            CountMap<Color> backgroundColorBallots = new CountMap<Color>();

            _rowResults.ForEach(r =>
            {
                Color backgroundColor = r.BackgroundColor;
                if (backgroundColor != null)
                {
                    backgroundColorBallots.Increment(r.BackgroundColor);
                }
            });

            var sortedBackgroundColorBallots = backgroundColorBallots.OrderByDescending(x => x.Value);

            return MeasureConfidence(sortedBackgroundColorBallots);
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

            var foregroundColorsSortedByContrast = foregroundColors.OrderByDescending(x =>
            {
                ColorPair cp = new ColorPair(backgroundColor, x.Key);
                return cp.ColorContrast();
            });

            CountMap<Color> unsortedForegroundColorBallots = new CountMap<Color>();
            Color currentSimilarColor = null;

            foreach (var pair in foregroundColorsSortedByContrast)
            {
                if (currentSimilarColor == null)
                {
                    unsortedForegroundColorBallots.Increment(pair.Key, pair.Value);
                    currentSimilarColor = pair.Key;
                }
                else if (currentSimilarColor.IsSimilarColor(pair.Key))
                {
                    unsortedForegroundColorBallots.Increment(currentSimilarColor, pair.Value);
                }
                else
                {
                    unsortedForegroundColorBallots.Increment(pair.Key, pair.Value);
                    currentSimilarColor = pair.Key;
                }
            }

            var sortedForegroundColorBallots = unsortedForegroundColorBallots.OrderByDescending(x => x.Value);

            return MeasureConfidence(sortedForegroundColorBallots);
        }

        private static ColorVoteInfo MeasureConfidence(IOrderedEnumerable<KeyValuePair<Color, int>> sortedBallots)
        {
            var ballotsAsList = new List<KeyValuePair<Color, int>>(sortedBallots);
            int pluralityBallots = 0;
            int pluralityVotes = 0;
            int totalVotes = 0;

            foreach (var colorInput in ballotsAsList)
            {
                totalVotes += colorInput.Value;
            }

            int plurality = totalVotes / 2 + 1;

            foreach (var colorCount in ballotsAsList)
            {
                pluralityBallots++;
                pluralityVotes += colorCount.Value;

                if (pluralityVotes >= plurality)
                {
                    break;
                }
            }

            return new ColorVoteInfo(
                ballotsAsList.First().Key,
                ballotsAsList.First().Value,
                DetermineConfidence(pluralityBallots, ballotsAsList.Count));
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
