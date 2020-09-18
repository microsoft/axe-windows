// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    internal class ResultAggregator
    {
        private ColorContrastResult.Confidence _bestConfidence;

        public ColorContrastResult ConvergedResult { get; private set; }

        public void AddResult(ColorContrastResult result)
        {
            ColorContrastResult.Confidence resultConfidence = result.ConfidenceValue();

            if (resultConfidence > _bestConfidence)
            {
                _bestConfidence = resultConfidence;
                ConvergedResult = result;
            }
        }

        public bool HasConverged => _bestConfidence == ColorContrastResult.Confidence.High;
    }
}
