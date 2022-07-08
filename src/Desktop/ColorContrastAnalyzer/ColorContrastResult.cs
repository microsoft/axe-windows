// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    internal class ColorContrastResult : IColorContrastResult
    {
        private List<ColorPair> alternatives = new List<ColorPair>();

        private ColorPair mostContrastingPair;

        private Confidence confidence;

        private int numDifferentBackgroundColors;

        private int numVisiblyDifferentTextColors;

        public ColorContrastResult()
        {
            this.confidence = Confidence.None;
        }

        internal ColorContrastResult Add(ColorPair newColorPair)
        {
            var newTextColor = newColorPair.foregroundColor;

            var newBackgroundColor = newColorPair.backgroundColor;

            if (mostContrastingPair == null ||
                mostContrastingPair.ColorContrast() < newColorPair.ColorContrast())
            {
                mostContrastingPair = newColorPair;
            }

            foreach (var alternativePair in alternatives)
            {
                if (!alternativePair.IsVisiblySimilarTo(newColorPair))
                {
                    numDifferentBackgroundColors++;
                    numVisiblyDifferentTextColors++;
                }
            }

            if (numDifferentBackgroundColors > 3 || numVisiblyDifferentTextColors > 3)
            {
                confidence = Confidence.Low;
            }
            else if (numDifferentBackgroundColors > 1 || numVisiblyDifferentTextColors > 1)
            {
                confidence = Confidence.Mid;
            }
            else
            {
                this.confidence = Confidence.High;
            }

            alternatives.Add(newColorPair);

            return this;
        }

        public ColorPair MostLikelyColorPair => mostContrastingPair;

        public Confidence Confidence => this.confidence;
    }
}
