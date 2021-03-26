// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    public interface IColorContrastConfig
    {
        /// <summary>
        /// The maximum width we expect Text to be. This number can be much larger than this
        /// and still be helpful.
        /// </summary>
        int MaxTextThickness { get; }

        /// <summary>
        /// The number of Color transitions we expect to make us believe a "Row" of Pixels
        /// contains Text.
        /// </summary>
        int MinNumberColorTransitions { get; }

        /// <summary>
        /// We're going to binary search sample rows. Once two samples are this
        /// far apart we should not sample between them as finding text is highly unlikely.
        /// A good value for this is the smallest reasonable font size for the DPI of the screen.
        ///
        /// Note: Making this number too low will have dramatic performance impacts when a HIGH
        /// confidence result is NOT found. EX: at 0, you would end up scanning the entire image.
        /// </summary>
        int MinSpaceBetweenSamples { get; }

        /// <summary>
        /// If a given pair of colors occurs this many times more frequently than other color combinations
        /// we are sure it is the Text/Background color pair. Otherwise, there are too many potential
        /// combinations in a "Row" of pixels for us to be positive about any given pair.
        /// </summary>
        int TransitionCountDominanceFactor { get; }
    }
}
