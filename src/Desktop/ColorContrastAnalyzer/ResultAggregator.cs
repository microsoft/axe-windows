// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    internal class ResultAggregator
    {
        private const int ConvergenceThreshold = 3;

        private List<ColorContrastResult> _resultsAtCurrentBestConfidence;
        private ColorContrastResult.Confidence _currentBestConfidence;
        private ColorContrastResult _convergedResult;
        private readonly ColorContrastResultComparer _resultComparer = new ColorContrastResultComparer();

        public ColorContrastResult MostLikelyResult => _convergedResult ?? _resultsAtCurrentBestConfidence.FirstOrDefault();

        public ResultAggregator()
        {
            SetCurrentBestConfidence(ColorContrastResult.Confidence.None);
        }

        public void AddResult(ColorContrastResult result)
        {
            ColorContrastResult.Confidence resultConfidence = result.ConfidenceValue();

            if (resultConfidence < _currentBestConfidence)
                return;

            if (resultConfidence > _currentBestConfidence)
            {
                SetCurrentBestConfidence(resultConfidence);
            }

            AddResultAtCurrentBestConfidence(result);
        }
        
        private void SetCurrentBestConfidence(ColorContrastResult.Confidence confidence)
        {
            _currentBestConfidence = confidence;
            _resultsAtCurrentBestConfidence = new List<ColorContrastResult>();
        }

        private void AddResultAtCurrentBestConfidence(ColorContrastResult result)
        {
            _resultsAtCurrentBestConfidence.Add(result);
            UpdateConvergedResult();
        }

        private void UpdateConvergedResult()
        {
            if (_currentBestConfidence == ColorContrastResult.Confidence.High)
            {
                _resultsAtCurrentBestConfidence.Sort(_resultComparer);
                if (_resultsAtCurrentBestConfidence.Count >= ConvergenceThreshold)
                {
                    ColorContrastResult baseResult = null;
                    int similarResults = 0;

                    foreach (var result in _resultsAtCurrentBestConfidence)
                    {
                        if (baseResult != null &&
                            baseResult.GetMostLikelyColorPair().IsVisiblySimilarTo(result.GetMostLikelyColorPair()))
                        {
                            if (++similarResults >= ConvergenceThreshold)
                            {
                                _convergedResult = baseResult;
                                break;
                            }
                        }
                        else
                        {
                            baseResult = result;
                            similarResults = 1;
                        }
                    }
                }
            }
        }

        public bool HasConverged => _convergedResult != null;
    }
}
