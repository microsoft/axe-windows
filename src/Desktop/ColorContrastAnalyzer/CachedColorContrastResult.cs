// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    /// <summary>
    /// Cache expensive to compute data in a single object
    /// </summary>
    internal class CachedColorContrastResult
    {
        public ColorPair MostLikelyColorPair { get; }
        public double Contrast { get; }
        public ColorContrastResult Result { get; }

        public CachedColorContrastResult(ColorContrastResult result)
        {
            Result = result;
            MostLikelyColorPair = result.GetMostLikelyColorPair();
            Contrast = MostLikelyColorPair == null ? 0 : MostLikelyColorPair.ColorContrast();
        }
    }
}
