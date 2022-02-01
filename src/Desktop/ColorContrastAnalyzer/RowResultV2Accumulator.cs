// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    /// <summary>
    /// Accumulate the RowResultV2 of each row, then combine them into a single
    /// result for the entire image
    /// </summary>
    internal class RowResultV2Accumulator
    {
        /// <summary>
        /// Summarizes color information--what color, how many votes it had, and
        /// and the computed confidence of that color
        /// </summary>
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

        internal RowResultV2Accumulator(IColorContrastConfig colorContastConfig)
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

            return new ColorContrastResultV2(
                new ColorPair(backgroundInfo.Color, foregroundInfo.Color),
                CombinedConfidence(backgroundInfo.Confidence, foregroundInfo.Confidence));
        }

        /// <summary>
        /// Counts up the BackgroundColor from each entry in _rowResults, then
        /// selects the most frequent BackgroundColor as the overall BackgroundColor.
        /// </summary>
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

            return BuildColorVoteInfo(sortedBackgroundColorBallots, GetTotalVoteCountFromAllBallots);
        }

        /// <summary>
        /// Counts up the ForegroundColor from each entry in _rowResults, then
        /// combines similar colors to allow for things like antialiasing.
        /// Candidate colors are evaluated, beginning at the highest contrast
        /// from backgroundColor, until we find a candidate that seems to occur
        /// frequently enough that we accept it as the foreground color.
        /// </summary>
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
                // Remember counts so we can add them after we iterate the entire list
                CountMap<Color> rememberedCounts = new CountMap<Color>();

                foreach (var aggregateForegroundColorBallot in aggregatedForegroundColorBallots)
                {
                    if (aggregateForegroundColorBallot.Key.IsSimilarColor(pair.Key))
                    {
                        rememberedCounts
                            .Increment(aggregateForegroundColorBallot.Key, pair.Value);
                    }
                }

                foreach (var countToAdd in rememberedCounts)
                {
                    aggregatedForegroundColorBallots
                        .Increment(countToAdd.Key, countToAdd.Value);
                }

                aggregatedForegroundColorBallots.Increment(pair.Key, pair.Value);
            }

            var sortedForegroundColorBallots = aggregatedForegroundColorBallots
                .OrderByDescending(x => x.Value);

            return BuildColorVoteInfo(sortedForegroundColorBallots, GetTotalVoteCountFromAllBallots);
        }

        /// <summary>
        /// Build the ColorVoteInfo, using the first value in sortedBallots as
        /// the color. The Confidence will be determined by how much consensus
        /// exists among the ballots to support that color choice.
        /// </summary>
        /// <param name="sortedBallots">Ballots being considered, in order of consideration</param>
        /// <param name="voteCounter">A method to count the underlying votes 
        /// represented by the sortedBallots parameter</param>
        private ColorVoteInfo BuildColorVoteInfo(IOrderedEnumerable<KeyValuePair<Color, int>> sortedBallots,
            Func<IEnumerable<KeyValuePair<Color, int>>, int> voteCounter)
        {
            List<KeyValuePair<Color, int>> ballotList = new List<KeyValuePair<Color, int>>(sortedBallots);
            int pluralityBallots = 0;
            int pluralityVotes = 0;
            int targetVotes = voteCounter(ballotList);

            int plurality = targetVotes / 2 + 1;

            foreach (var colorCount in ballotList)
            {
                pluralityBallots++;
                pluralityVotes += colorCount.Value;

                if (pluralityVotes >= plurality)
                {
                    break;
                }
            }

            return new ColorVoteInfo(
                ballotList.First().Key,
                ballotList.First().Value,
                DetermineConfidence(pluralityBallots, ballotList.Count));
        }

        private static int GetTotalVoteCountFromAllBallots(IEnumerable<KeyValuePair<Color, int>> ballots)
        {
            int totalVotes = 0;
            foreach (var ballot in ballots)
            {
                totalVotes += ballot.Value;
            }
            return totalVotes;
        }

        /// <summary>
        /// Heuristically assign a Confidence based on how many total ballots
        /// we had and how many ballots we had to consider before reaching a
        /// plurality of votes. The confidence thresholds are configurable via
        /// the IColorContrastConfig.
        /// </summary>
        private Confidence DetermineConfidence(int pluralityBallotCount, int totalBallotCount)
        {
            var percentageToPlurality = pluralityBallotCount * 1.0 / totalBallotCount;

            if (pluralityBallotCount == 1 ||
                percentageToPlurality <= _colorContrastConfig.HighConfidenceThreshold)
            {
                return Confidence.High;
            }

            if (pluralityBallotCount == 2 ||
                percentageToPlurality <= _colorContrastConfig.MidConfidenceThreshold)
            {
                return Confidence.Mid;
            }

            if (percentageToPlurality <= _colorContrastConfig.LowConfidenceThreshold)
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
