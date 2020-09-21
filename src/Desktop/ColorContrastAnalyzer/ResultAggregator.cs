// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    internal class ResultAggregator
    {
        private List<ColorContrastResult> _results;
        private ColorContrastResult.Confidence _bestConfidence;

        public ColorContrastResult BestEstimatedResult => _results.FirstOrDefault();

        public ResultAggregator()
        {
            SetNewConfidence(ColorContrastResult.Confidence.None);
        }

        public void AddResult(ColorContrastResult result)
        {
            ColorContrastResult.Confidence resultConfidence = result.ConfidenceValue();

            if (resultConfidence < _bestConfidence)
                return;

            if (resultConfidence > _bestConfidence)
            {
                SetNewConfidence(resultConfidence);
            }

            _results.Add(result);
            HasConverged = CheckForConvergence();
        }
        
        private void SetNewConfidence(ColorContrastResult.Confidence confidence)
        {
            _bestConfidence = confidence;
            _results = new List<ColorContrastResult>();
        }

        private bool CheckForConvergence()
        {
            return _bestConfidence == ColorContrastResult.Confidence.High;
        }

        public bool HasConverged { get; private set; }
    }
}
