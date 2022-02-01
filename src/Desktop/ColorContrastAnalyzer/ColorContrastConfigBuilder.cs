// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    public class ColorContrastConfigBuilder
    {
        private int? _maxTextThickness;
        private int? _minNumberOfColorTransitions;
        private int? _minSpaceBetweenSamples;
        private int? _transitionCountDominanceFactor;
        private AnalyzerVersion? _analyzerVersion;
        private double? _highConfidenceThreshold;
        private double? _midConfidenceThreshold;
        private double? _lowConfidenceThreshold;

        public IColorContrastConfig Build() => new ColorContrastConfig(
            _maxTextThickness, _minNumberOfColorTransitions, _minSpaceBetweenSamples,
            _transitionCountDominanceFactor, _analyzerVersion,
            _highConfidenceThreshold,
            _midConfidenceThreshold,
            _lowConfidenceThreshold);

        public ColorContrastConfigBuilder WithMaxTextThickness(int maxTextThickness)
        {
            _maxTextThickness = maxTextThickness;
            return this;
        }

        public ColorContrastConfigBuilder WithMinNumberColorTransitions(int minNumberOfColorTransitions)
        {
            _minNumberOfColorTransitions = minNumberOfColorTransitions;
            return this;
        }

        public ColorContrastConfigBuilder WithMinSpaceBetweenSamples(int minSpaceBetweenSamples)
        {
            _minSpaceBetweenSamples = minSpaceBetweenSamples;
            return this;
        }

        public ColorContrastConfigBuilder WithTransitionCountDominanceFactor(int transitionCountDominanceFactor)
        {
            _transitionCountDominanceFactor = transitionCountDominanceFactor;
            return this;
        }

        public ColorContrastConfigBuilder WithAnalyzerVersion(AnalyzerVersion analyzerVersion)
        {
            _analyzerVersion = analyzerVersion;
            return this;
        }

        /// <summary>
        /// Sets the maximum threshold for high confidence. Default value is 0.9.
        /// Smaller values will report high confidence for more images, while
        /// larger values will report high confidence for fewer images.
        /// </summary>
        /// <param name="highConfidenceThreshold">The high confidence threshold</param>
        /// <returns>The updated ConfigBuilder</returns>
        public ColorContrastConfigBuilder WithHighConfidenceThreshold(double highConfidenceThreshold)
        {
            _highConfidenceThreshold = highConfidenceThreshold;
            return this;
        }

        /// <summary>
        /// Sets the maximum threshold for mid confidence. Default value is 0.7.
        /// Smaller values will report mid confidence for more images, while
        /// larger values will report mid confidence for fewer images.
        /// </summary>
        /// <param name="midConfidenceThreshold">The mid confidence threshold</param>
        /// <returns>The updated ConfigBuilder</returns>
        public ColorContrastConfigBuilder WithMidConfidenceThreshold(double midConfidenceThreshold)
        {
            _midConfidenceThreshold = midConfidenceThreshold;
            return this;
        }

        /// <summary>
        /// Sets the maximum threshold for low confidence. Default value is 0.5.
        /// Smaller values will report low confidence for more images, while
        /// larger values will report low confidence for fewer images.
        /// </summary>
        /// <param name="lowConfidenceThreshold">The low confidence threshold</param>
        /// <returns>The updated ConfigBuilder</returns>
        public ColorContrastConfigBuilder WithLowConfidenceThreshold(double lowConfidenceThreshold)
        {
            _lowConfidenceThreshold = lowConfidenceThreshold;
            return this;
        }
    }
}
