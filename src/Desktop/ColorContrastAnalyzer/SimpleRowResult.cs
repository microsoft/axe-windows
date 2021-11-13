// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    public class SimpleRowResult
    {
        internal Color BackgroundColor { get; }

        internal Color ForegroundColor { get; }

        public SimpleRowResult(Color backgroundColor = null,
            Color foregroundColor = null)
        {
            BackgroundColor = backgroundColor;
            ForegroundColor = foregroundColor;
        }
    }
}
