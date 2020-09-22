// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    internal class ColorContrastResultComparer : IComparer<ColorContrastResult>
    {
        int IComparer<ColorContrastResult>.Compare(ColorContrastResult x, ColorContrastResult y)
        {
            double xContrast = x.GetMostLikelyColorPair().ColorContrast();
            double yContrast = y.GetMostLikelyColorPair().ColorContrast();

            return xContrast.CompareTo(yContrast);
        }
    }
}
