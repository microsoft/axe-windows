// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    internal class ColorContrastResultV2 : IColorContrastResult
    {
        public ColorPair MostLikelyColorPair { get; }
        public Confidence Confidence { get; }

        public ColorContrastResultV2(ColorPair mostLikelycolorPair, Confidence confidence)
        {
            MostLikelyColorPair = mostLikelycolorPair;
            Confidence = confidence;
        }
    }
}
