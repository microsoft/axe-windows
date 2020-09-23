// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.Desktop.ColorContrastAnalyzer
{
    internal class ResultAggregator
    {
        private List<CachedColorContrastResult> _cachedResultsAtCurrentBestConfidence;
        private ColorContrastResult.Confidence _currentBestConfidence;
        private ColorContrastResult _convergedResult;
        private readonly CachedColorContrastResultComparer _resultComparer = new CachedColorContrastResultComparer();

        public ColorContrastResult MostLikelyResult => _convergedResult ?? _cachedResultsAtCurrentBestConfidence.FirstOrDefault()?.Result;
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
            _cachedResultsAtCurrentBestConfidence = new List<CachedColorContrastResult>();
        }

        private void AddResultAtCurrentBestConfidence(ColorContrastResult result)
        {
            _cachedResultsAtCurrentBestConfidence.Add(new CachedColorContrastResult(result));
            UpdateConvergedResult();
        }

        private void UpdateConvergedResult()
        {
            if (_currentBestConfidence == ColorContrastResult.Confidence.High)
            {
                _cachedResultsAtCurrentBestConfidence.Sort(_resultComparer);
                if (_cachedResultsAtCurrentBestConfidence.Count >= ColorContrastConfig.HighConfidenceConvergenceThreshold)
                {
                    CachedColorContrastResult baseResult = null;
                    int similarResults = 0;

                    foreach (var result in _cachedResultsAtCurrentBestConfidence)
                    {
                        if (baseResult != null &&
                            baseResult.MostLikelyColorPair.IsVisiblySimilarTo(result.MostLikelyColorPair))
                        {
                            if (++similarResults >= ColorContrastConfig.HighConfidenceConvergenceThreshold)
                            {
                                _convergedResult = baseResult.Result;
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
