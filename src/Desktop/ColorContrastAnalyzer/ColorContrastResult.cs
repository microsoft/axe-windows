// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    internal class ColorContrastResult : IColorContrastResult
    {
        private readonly List<ColorPair> _alternatives = new List<ColorPair>();

        private ColorPair _mostContrastingPair;

        private Confidence _confidence;

        private int _numDifferentBackgroundColors;

        private int _numVisiblyDifferentTextColors;

        public ColorContrastResult()
        {
            _confidence = Confidence.None;
        }

        internal ColorContrastResult Add(ColorPair newColorPair)
        {
#if ENABLE_COLOR_CONTRAST_DEBUGGING
            var newTextColor = newColorPair.foregroundColor;
            var newBackgroundColor = newColorPair.backgroundColor;
#endif

            if (_mostContrastingPair == null ||
                _mostContrastingPair.ColorContrast() < newColorPair.ColorContrast())
            {
                _mostContrastingPair = newColorPair;
            }

            foreach (var alternativePair in _alternatives)
            {
                if (!alternativePair.IsVisiblySimilarTo(newColorPair))
                {
                    _numDifferentBackgroundColors++;
                    _numVisiblyDifferentTextColors++;
                }
            }

            if (_numDifferentBackgroundColors > 3 || _numVisiblyDifferentTextColors > 3)
            {
                _confidence = Confidence.Low;
            }
            else if (_numDifferentBackgroundColors > 1 || _numVisiblyDifferentTextColors > 1)
            {
                _confidence = Confidence.Mid;
            }
            else
            {
                _confidence = Confidence.High;
            }

            _alternatives.Add(newColorPair);

            return this;
        }

        public ColorPair MostLikelyColorPair => _mostContrastingPair;

        public Confidence Confidence => _confidence;
    }
}
