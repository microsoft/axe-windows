// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    public class DefaultColorContrastConfig : IColorContrastConfig
    {
        public int MaxTextThickness => ColorContrastConfig.DefaultMaxTextThickness;

        public int MinNumberColorTransitions => ColorContrastConfig.DefaultMinColorTransitions;

        public int MinSpaceBetweenSamples => ColorContrastConfig.DefaultMinSpaceBetweenSamples;

        public int TransitionCountDominanceFactor => ColorContrastConfig.DefaultTransitionCountDominanceFactor;

        public AnalyzerVersion AnalyzerVersion => ColorContrastConfig.DefaultAnalyzerVersion;

        public double HighConfidenceThreshold => ColorContrastConfig.DefaultHighConfidenceThreshold;

        public double MidConfidenceThreshold => ColorContrastConfig.DefaultMidConfidenceThreshold;

        public double LowConfidenceThreshold => ColorContrastConfig.DefaultLowConfidenceThreshold;
    }
}
