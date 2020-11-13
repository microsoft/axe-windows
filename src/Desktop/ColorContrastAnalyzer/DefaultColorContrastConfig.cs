// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    public class DefaultColorContrastConfig : IColorContrastConfig
    {
        public int MaxTextThickness => 20;

        public int MinNumberColorTransitions => 4;

        public int MinSpaceBetweenSamples => 12;

        public int TransitionCountDominanceFactor => 2;

        public int HighConfidenceConvergenceThreshold => 3;
    }
}
