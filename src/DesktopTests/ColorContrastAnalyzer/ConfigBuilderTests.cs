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
            int maxTextThickness = 20, int minNumberColorTransitions = 4,
            int minSpaceBetweenSamples = 12, int transitionCountDominanceFactor = 2,
            AnalyzerVersion analyzerVersion = AnalyzerVersion.V1)
        {
            Assert.AreEqual(maxTextThickness, config.MaxTextThickness);
            Assert.AreEqual(minNumberColorTransitions, config.MinNumberColorTransitions);
            Assert.AreEqual(minSpaceBetweenSamples, config.MinSpaceBetweenSamples);
            Assert.AreEqual(transitionCountDominanceFactor, config.TransitionCountDominanceFactor);
            Assert.AreEqual(analyzerVersion, config.AnalyzerVersion);
        }

        [TestMethod]
        public void Build_DefaultInputs_ConfigIsDefault()
        {
            EnsureConfigValues(_builder.Build());
        }

        [TestMethod]
        public void Build_SetMaxTestThickness_ConfigIsCorrect()
        {
            const int maxTextThickness = 90;
            EnsureConfigValues(_builder.WithMaxTextThickness(maxTextThickness).Build(),
                maxTextThickness: maxTextThickness);
        }

        [TestMethod]
        public void Build_SetMinNumberColorTransitions_ConfigIsCorrect()
        {
            const int minNumberColorTransitions = 80;
            EnsureConfigValues(_builder.WithMinNumberColorTransitions(minNumberColorTransitions).Build(),
                minNumberColorTransitions: minNumberColorTransitions);
        }

        [TestMethod]
        public void Build_SetMinSpaceBetweenSamples_ConfigIsCorrect()
        {
            const int minSpaceBetweenSamples = 3;
            EnsureConfigValues(_builder.WithMinSpaceBetweenSamples(minSpaceBetweenSamples).Build(),
                minSpaceBetweenSamples: minSpaceBetweenSamples);
        }

        [TestMethod]
        public void Build_SetTransitionCountDominanceFactor_ConfigIsCorrect()
        {
            const int transitionCountDominanceFactor = 4;
            EnsureConfigValues(_builder.WithTransitionCountDominanceFactor(transitionCountDominanceFactor).Build(),
                transitionCountDominanceFactor: transitionCountDominanceFactor);
        }

        [TestMethod]
        public void Build_SetAnalyzerVersion_ConfigIsCorrect()
        {
            const AnalyzerVersion analyzerVersion = AnalyzerVersion.V2;
            EnsureConfigValues(_builder.WithAnalyzerVersion(analyzerVersion).Build(),
                analyzerVersion: analyzerVersion);
        }
    }
}
