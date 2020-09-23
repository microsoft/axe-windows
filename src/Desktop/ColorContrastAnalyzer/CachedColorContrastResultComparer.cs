// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    internal class CachedColorContrastResultComparer : IComparer<CachedColorContrastResult>
    {
        int IComparer<CachedColorContrastResult>.Compare(CachedColorContrastResult x, CachedColorContrastResult y)
        {
            return x.Contrast.CompareTo(y.Contrast);
        }
    }
}
