﻿// Copyright (c) Microsoft. All rights reserved.
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

        private readonly IColorContrastConfig _colorContrastConfig;
        private readonly List<RowResultV2> _rowResults = new List<RowResultV2>();

        internal RowColorAccumulator(IColorContrastConfig colorContastConfig)
        {
            _colorContrastConfig = colorContastConfig;
        }

        internal void AddRowResult(RowResultV2 rowResult)
        {
            _rowResults.Add(rowResult);
        }

        internal IColorContrastResult GetResult()
        {
            ColorVoteInfo backgroundInfo = GetBackgroundInfo();
            ColorVoteInfo foregroundInfo = GetForegroundInfo(backgroundInfo.Color);

            // TODO: Non-trivial cases

            return new ColorContrastResultV2(
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

            return MeasureConfidence(sortedBackgroundColorBallots, CountVotesViaSimpleBallots);
        }

        private ColorVoteInfo GetForegroundInfo(Color backgroundColor)
        {
            CountMap<Color> simpleForegroundColorBallots = new CountMap<Color>();

            _rowResults.ForEach(r =>
            {
                Color foregroundColor = r.ForegroundColor;
                Color testBackgroundColor = r.BackgroundColor;
                if (foregroundColor != null && backgroundColor.Equals(testBackgroundColor))
                {
                    simpleForegroundColorBallots.Increment(r.ForegroundColor, r.TransitionCount);
                }
            });

            if (!simpleForegroundColorBallots.Any())
            {
                return null;
            }

            var simpleForegroundBallotsSortedByContrast = simpleForegroundColorBallots.OrderByDescending(x =>
            {
                ColorPair cp = new ColorPair(backgroundColor, x.Key);
                return cp.ColorContrast();
            });

            CountMap<Color> aggregatedForegroundColorBallots = new CountMap<Color>();

            foreach (var pair in simpleForegroundBallotsSortedByContrast)
            {
                CountMap<Color> countsToAddOutsideOfIterator = new CountMap<Color>();

                foreach (var aggregateForegroundColorBallot in aggregatedForegroundColorBallots)
                {
                    if (aggregateForegroundColorBallot.Key.IsSimilarColor(pair.Key))
                    {
                        countsToAddOutsideOfIterator.Increment(aggregateForegroundColorBallot.Key, pair.Value);
                    }
                }

                foreach (var countToAdd  in countsToAddOutsideOfIterator)
                {
                    aggregatedForegroundColorBallots.Increment(countToAdd.Key, countToAdd.Value);
                }

                aggregatedForegroundColorBallots.Increment(pair.Key, pair.Value);
            }

            var sortedForegroundColorBallots = aggregatedForegroundColorBallots.OrderByDescending(x => x.Value);

            return MeasureConfidence(sortedForegroundColorBallots, CountVotesViaSimpleBallots);
        }

        private ColorVoteInfo MeasureConfidence(IOrderedEnumerable<KeyValuePair<Color, int>> sortedBallots,
            Func<IReadOnlyList<KeyValuePair<Color, int>>, int> voteCounter)
        {
            var ballotsAsList = new List<KeyValuePair<Color, int>>(sortedBallots);
            int pluralityBallots = 0;
            int pluralityVotes = 0;
            int targetVotes = voteCounter(ballotsAsList);

            int plurality = targetVotes / 2 + 1;

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

        private static int CountVotesViaSimpleBallots(IReadOnlyList<KeyValuePair<Color, int>> ballots)
        {
            int totalVotes = 0;
            foreach (var ballot in ballots)
            {
                totalVotes += ballot.Value;
            }
            return totalVotes;
        }

        private Confidence DetermineConfidence(int pluralityBlocks, int totalBlocks)
        {
            var pluralityBlockPercentage = pluralityBlocks * 1.0 / totalBlocks;

            if (pluralityBlocks == 1 ||
                pluralityBlockPercentage <= _colorContrastConfig.HighConfidenceThreshold)
            {
                return Confidence.High;
            }

            if (pluralityBlocks == 2 ||
                pluralityBlockPercentage <= _colorContrastConfig.MidConfidenceThreshold)
            {
                return Confidence.Mid;
            }

            if (pluralityBlockPercentage <= _colorContrastConfig.LowConfidenceThreshold)
            {
                return Confidence.Low;
            }

            return Confidence.None;
        }

        private static Confidence CombinedConfidence(Confidence confidence1, Confidence confidence2)
        {
            int combinedConfidence = Math.Min((int)confidence1, (int)confidence2);
            return (Confidence)combinedConfidence;
        }
    }
}
