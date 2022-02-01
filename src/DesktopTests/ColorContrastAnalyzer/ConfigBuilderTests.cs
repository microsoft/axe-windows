// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.ColorContrastAnalyzer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.DesktopTests.ColorContrastAnalyzer
{
    [TestClass]
    public class ConfigBuilderTests
    {
        private ConfigBuilder _builder;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _builder = new ConfigBuilder();
        }

        private void EnsureConfigValues(IColorContrastConfig config,
            int maxTextThickness = ColorContrastConfig.DefaultMaxTextThickness,
            int minNumberColorTransitions = ColorContrastConfig.DefaultMinColorTransitions,
            int minSpaceBetweenSamples = ColorContrastConfig.DefaultMinSpaceBetweenSamples,
            int transitionCountDominanceFactor = ColorContrastConfig.DefaultTransitionCountDominanceFactor,
            AnalyzerVersion analyzerVersion = ColorContrastConfig.DefaultAnalyzerVersion,
            double highConfidenceThreshold = ColorContrastConfig.DefaultHighConfidenceThreshold,
            double midConfidenceThreshold = ColorContrastConfig.DefaultMidConfidenceThreshold,
            double lowConfidenceThreshold = ColorContrastConfig.DefaultLowConfidenceThreshold)
        {
            Assert.AreEqual(maxTextThickness, config.MaxTextThickness);
            Assert.AreEqual(minNumberColorTransitions, config.MinNumberColorTransitions);
            Assert.AreEqual(minSpaceBetweenSamples, config.MinSpaceBetweenSamples);
            Assert.AreEqual(transitionCountDominanceFactor, config.TransitionCountDominanceFactor);
            Assert.AreEqual(analyzerVersion, config.AnalyzerVersion);
            Assert.AreEqual(highConfidenceThreshold, config.HighConfidenceThreshold);
            Assert.AreEqual(midConfidenceThreshold, config.MidConfidenceThreshold);
            Assert.AreEqual(lowConfidenceThreshold, config.LowConfidenceThreshold);
        }

        [TestMethod]
        [Timeout(2000)]
        public void Build_DefaultInputs_ConfigIsDefault()
        {
            EnsureConfigValues(_builder.Build());
        }

        [TestMethod]
        [Timeout(2000)]
        public void Build_WithMaxTestThickness_ConfigIsCorrect()
        {
            const int maxTextThickness = 90;
            EnsureConfigValues(_builder.WithMaxTextThickness(maxTextThickness).Build(),
                maxTextThickness: maxTextThickness);
        }

        [TestMethod]
        [Timeout(2000)]
        public void Build_WithMinNumberColorTransitions_ConfigIsCorrect()
        {
            const int minNumberColorTransitions = 80;
            EnsureConfigValues(_builder.WithMinNumberColorTransitions(minNumberColorTransitions).Build(),
                minNumberColorTransitions: minNumberColorTransitions);
        }

        [TestMethod]
        [Timeout(2000)]
        public void Build_WithMinSpaceBetweenSamples_ConfigIsCorrect()
        {
            const int minSpaceBetweenSamples = 3;
            EnsureConfigValues(_builder.WithMinSpaceBetweenSamples(minSpaceBetweenSamples).Build(),
                minSpaceBetweenSamples: minSpaceBetweenSamples);
        }

        [TestMethod]
        [Timeout(2000)]
        public void Build_WithTransitionCountDominanceFactor_ConfigIsCorrect()
        {
            const int transitionCountDominanceFactor = 4;
            EnsureConfigValues(_builder.WithTransitionCountDominanceFactor(transitionCountDominanceFactor).Build(),
                transitionCountDominanceFactor: transitionCountDominanceFactor);
        }

        [TestMethod]
        [Timeout(2000)]
        public void Build_WithAnalyzerVersion_ConfigIsCorrect()
        {
            const AnalyzerVersion analyzerVersion = AnalyzerVersion.V2;
            EnsureConfigValues(_builder.WithAnalyzerVersion(analyzerVersion).Build(),
                analyzerVersion: analyzerVersion);
        }

        [TestMethod]
        [Timeout(2000)]
        public void Build_WithHighConfidenceThreshold_ConfigIsCorrect()
        {
            const double highConfidenceThreshold = 0.2;
            EnsureConfigValues(_builder.WithHighConfidenceThreshold(highConfidenceThreshold).Build(),
                highConfidenceThreshold: highConfidenceThreshold);
        }

        [TestMethod]
        [Timeout(2000)]
        public void Build_WithMidConfidenceThreshold_ConfigIsCorrect()
        {
            const double midConfidenceThreshold = 0.4;
            EnsureConfigValues(_builder.WithMidConfidenceThreshold(midConfidenceThreshold).Build(),
                midConfidenceThreshold: midConfidenceThreshold);
        }

        [TestMethod]
        [Timeout(2000)]
        public void Build_WithLowConfidenceThreshold_ConfigIsCorrect()
        {
            const double lowConfidenceThreshold = 0.6;
            EnsureConfigValues(_builder.WithLowConfidenceThreshold(lowConfidenceThreshold).Build(),
                lowConfidenceThreshold: lowConfidenceThreshold);
        }
    }
}
