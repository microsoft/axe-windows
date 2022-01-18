// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    class ColorContrastConfig : IColorContrastConfig
    {
        public int MaxTextThickness { get; }

        public int MinNumberColorTransitions { get; }

        public int MinSpaceBetweenSamples { get; }

        public int TransitionCountDominanceFactor { get; }

        public AnalyzerVersion AnalyzerVersion { get; }

        public ColorContrastConfig(int? maxTextThickness, int? minNumberOfColorTransitions,
            int? minSpaceBetweenSamples, int? transitionCountDominanceFactor, AnalyzerVersion? analyzerVersion)
        {
            MaxTextThickness = maxTextThickness.HasValue ?
                maxTextThickness.Value : 20;
            MinNumberColorTransitions = minNumberOfColorTransitions.HasValue ?
                minNumberOfColorTransitions.Value : 4;
            MinSpaceBetweenSamples = minSpaceBetweenSamples.HasValue ?
                minSpaceBetweenSamples.Value : 12;
            TransitionCountDominanceFactor = transitionCountDominanceFactor.HasValue ?
                transitionCountDominanceFactor.Value : 2;
            AnalyzerVersion = analyzerVersion.HasValue ?
                analyzerVersion.Value : AnalyzerVersion.V1;
        }
    }
}
