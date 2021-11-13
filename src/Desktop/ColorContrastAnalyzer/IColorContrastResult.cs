// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    public enum Confidence { None, Low, Mid, High }

    public interface IColorContrastResult
    {
        ColorPair MostLikelyColorPair { get; }

        Confidence ConfidenceValue();
    }
}
