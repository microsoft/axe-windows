// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    class ConfigBuilder
    {
        private int? _maxTextThickness;
        private int? _minNumberOfColorTransitions;
        private int? _minSpaceBetweenSamples;
        private int? _transitionCountDominanceFactor;
        private AnalyzerVersion? _analyzerVersion;

        public IColorContrastConfig Build() => new ColorContrastConfig(
            _maxTextThickness, _minNumberOfColorTransitions, _minSpaceBetweenSamples,
            _transitionCountDominanceFactor, _analyzerVersion);

        public ConfigBuilder WithMaxTextThickness(int maxTextThickness)
        {
            _maxTextThickness = maxTextThickness;
            return this;
        }

        public ConfigBuilder WithMinNumberOfColorTransitions(int minNumberOfColorTransitions)
        {
            _minNumberOfColorTransitions = minNumberOfColorTransitions;
            return this;
        }

        public ConfigBuilder WithMinSpaceBetweenSamples(int minSpaceBetweenSamples)
        {
            _minSpaceBetweenSamples = minSpaceBetweenSamples;
            return this;
        }

        public ConfigBuilder WithTransitionCountDominanceFactor(int transitionCountDominanceFactor)
        {
            _transitionCountDominanceFactor = transitionCountDominanceFactor;
            return this;
        }

        public ConfigBuilder WithAnalyzerVersion(AnalyzerVersion analyzerVersion)
        {
            _analyzerVersion = analyzerVersion;
            return this;
        }
    }
}
