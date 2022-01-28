﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    public class ColorContrastResultV2: IColorContrastResult
    {
        private readonly ColorPair _mostLikelyColorPair;
        private readonly Confidence _confidence;

        public ColorContrastResultV2(ColorPair mostLikelycolorPair, Confidence confidence)
        {
            _mostLikelyColorPair = mostLikelycolorPair;
            _confidence = confidence;
        }

        public ColorPair MostLikelyColorPair => _mostLikelyColorPair;

        public Confidence Confidence => _confidence;
    }
}