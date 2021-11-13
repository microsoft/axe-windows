// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

            public ColorVoteInfo(Color color, int votes)
            {
                Color = color;
                Votes = votes;
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
                new ColorPair(backgroundInfo.Color, foregroundInfo.Color), Confidence.High);
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

            var candidateBackgroundEntry= backgroundColorByFrequency.First();

            if (candidateBackgroundEntry.Value >= _rowResults.Count / 2)
            {
                return new ColorVoteInfo(candidateBackgroundEntry.Key, candidateBackgroundEntry.Value);
            }
            return null;
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

            var foregroundColorByFrequency = foregroundColors.OrderByDescending(x => x.Value);

            var candidateForegroundEntry = foregroundColorByFrequency.First();

            return new ColorVoteInfo(candidateForegroundEntry.Key, candidateForegroundEntry.Value);
        }
    }
}
