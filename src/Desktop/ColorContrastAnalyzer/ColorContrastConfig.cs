// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    class ColorContrastConfig : IColorContrastConfig
    {
        internal const int DefaultMaxTextThickness = 20;
        internal const int DefaultMinColorTransitions = 4;
        internal const int DefaultMinSpaceBetweenSamples = 12;
        internal const int DefaultTransitionCountDominanceFactor = 2;
        internal const AnalyzerVersion DefaultAnalyzerVersion = AnalyzerVersion.V1;
        internal const double DefaultHighConfidenceThreshold = 0.1;
        internal const double DefaultMidConfidenceThreshold = 0.3;
        internal const double DefaultLowConfidenceThreshold = 0.5;

        public int MaxTextThickness { get; }

        public int MinNumberColorTransitions { get; }

        public int MinSpaceBetweenSamples { get; }

        public int TransitionCountDominanceFactor { get; }

        public AnalyzerVersion AnalyzerVersion { get; }

        public double HighConfidenceThreshold { get; }

        public double MidConfidenceThreshold { get; }

        public double LowConfidenceThreshold { get; }

        public ColorContrastConfig(int? maxTextThickness = null,
            int? minNumberOfColorTransitions = null,
            int? minSpaceBetweenSamples = null,
            int? transitionCountDominanceFactor = null,
            AnalyzerVersion? analyzerVersion = null,
            double? highConfidenceThreshold = null,
            double? midConfidenceThreshold = null,
            double? lowConfidenceThreshold = null)
        {
            MaxTextThickness = maxTextThickness.HasValue ?
                maxTextThickness.Value : DefaultMaxTextThickness;
            MinNumberColorTransitions = minNumberOfColorTransitions.HasValue ?
                minNumberOfColorTransitions.Value : DefaultMinColorTransitions;
            MinSpaceBetweenSamples = minSpaceBetweenSamples.HasValue ?
                minSpaceBetweenSamples.Value : DefaultMinSpaceBetweenSamples;
            TransitionCountDominanceFactor = transitionCountDominanceFactor.HasValue ?
                transitionCountDominanceFactor.Value : DefaultTransitionCountDominanceFactor;
            AnalyzerVersion = analyzerVersion.HasValue ?
                analyzerVersion.Value : DefaultAnalyzerVersion;
            HighConfidenceThreshold = highConfidenceThreshold.HasValue ?
                highConfidenceThreshold.Value : DefaultHighConfidenceThreshold;
            MidConfidenceThreshold = midConfidenceThreshold.HasValue ?
                midConfidenceThreshold.Value : DefaultMidConfidenceThreshold;
            LowConfidenceThreshold = lowConfidenceThreshold.HasValue ?
                lowConfidenceThreshold.Value : DefaultLowConfidenceThreshold;

            if (HighConfidenceThreshold >= MidConfidenceThreshold)
            {
                throw new ArgumentException("High Confidence threshold must be smaller than Mid confidence threshold");
            }
            if (MidConfidenceThreshold >= LowConfidenceThreshold)
            {
                throw new ArgumentException("Mid Confidence threshold must be smaller than Low confidence threshold");
            }
            if (HighConfidenceThreshold < 0.0)
            {
                throw new ArgumentException("High Confidence threshold must be >= 0.0");
            }
            if (lowConfidenceThreshold >= 1.0)
            {
                throw new ArgumentException("Low Confidence threshold must be < 1.0");
            }
        }
    }
}
