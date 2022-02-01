// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    public enum AnalyzerVersion
    {
        Default, // Use whatever the current default happens to be
        V1,      // Decides based on a single scanline
        V2,      // Decides based on all scanlines in the image
    }
}
