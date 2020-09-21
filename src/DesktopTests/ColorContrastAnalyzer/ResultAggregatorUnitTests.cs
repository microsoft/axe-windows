// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.ColorContrastAnalyzer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Cryptography.X509Certificates;

namespace Axe.Windows.DesktopTests.ColorContrastAnalyzer
{
    [TestClass]
    public class ResultAggregatorUnitTests
    {
        private static readonly ColorContrastResult NoConfidenceResult = 
            BuildColorContrastResult(ColorContrastResult.Confidence.None, Color.RED, Color.RED);
        private static readonly ColorContrastResult LowConfidenceResult =
            BuildColorContrastResult(ColorContrastResult.Confidence.Low, Color.BLUE, Color.GREEN);
        private static readonly ColorContrastResult MidConfidenceResult =
            BuildColorContrastResult(ColorContrastResult.Confidence.Mid, Color.BLUE, Color.WHITE);
        private static readonly ColorContrastResult HighConfidenceResult =
            BuildColorContrastResult(ColorContrastResult.Confidence.High, Color.BLACK, Color.WHITE);

        private static ColorContrastResult BuildColorContrastResult(ColorContrastResult.Confidence confidence, Color color1, Color color2)
        {
            ColorPair colorPair = new ColorPair(color1, color2);
            Mock<ColorContrastResult> resultMock = new Mock<ColorContrastResult>(MockBehavior.Strict);
            resultMock.Setup(x => x.ConfidenceValue())
                .Returns(confidence);
            resultMock.Setup(x => x.GetMostLikelyColorPair())
                .Returns(colorPair);

            return resultMock.Object;
        }

        [TestMethod]
        public void AssertStaticValues()
        {
            Assert.AreEqual(ColorContrastResult.Confidence.None, NoConfidenceResult.ConfidenceValue());
            Assert.AreEqual(ColorContrastResult.Confidence.Low, LowConfidenceResult.ConfidenceValue());
            Assert.AreEqual(ColorContrastResult.Confidence.Mid, MidConfidenceResult.ConfidenceValue());
            Assert.AreEqual(ColorContrastResult.Confidence.High, HighConfidenceResult.ConfidenceValue());
        }

        [TestMethod]
        public void HasConverged_EmptyAggregator_ReturnsFalse()
        {
            Assert.IsFalse(new ResultAggregator().HasConverged);
        }

        [TestMethod]
        public void BestEstimatedResult_EmptyAggregator_ReturnsNull()
        {
            Assert.IsNull(new ResultAggregator().BestEstimatedResult);
        }

        [TestMethod]
        public void BestEstimatedResult_IncreasingConfidence_ReturnsHighestConfidence()
        {
            ColorContrastResult[] increasingConfidenceResults = 
                { NoConfidenceResult, LowConfidenceResult, MidConfidenceResult, HighConfidenceResult };
            var aggregator = new ResultAggregator();
            foreach (var result in increasingConfidenceResults)
            {
                aggregator.AddResult(result);
                Assert.AreEqual(result, aggregator.BestEstimatedResult);
            }
        }

        [TestMethod]
        public void BestEstimatedResult_DecreasingConfidence_ReturnsHighestConfidence()
        {
            ColorContrastResult[] increasingConfidenceResults =
                { HighConfidenceResult, MidConfidenceResult, LowConfidenceResult, NoConfidenceResult };
            var aggregator = new ResultAggregator();
            foreach (var result in increasingConfidenceResults)
            {
                aggregator.AddResult(result);
                Assert.AreEqual(HighConfidenceResult, aggregator.BestEstimatedResult);
            }
        }

        [TestMethod]
        public void HasConverged_OneNoConfidenceResult_Returns_False()
        {
            var aggregator = new ResultAggregator();
            aggregator.AddResult(NoConfidenceResult);
            Assert.IsFalse(aggregator.HasConverged);
        }

        [TestMethod]
        public void HasConverged_OnelOWConfidenceResult_Returns_False()
        {
            var aggregator = new ResultAggregator();
            aggregator.AddResult(LowConfidenceResult);
            Assert.IsFalse(aggregator.HasConverged);
        }

        [TestMethod]
        public void HasConverged_OneMidConfidenceResult_Returns_False()
        {
            var aggregator = new ResultAggregator();
            aggregator.AddResult(MidConfidenceResult);
            Assert.IsFalse(aggregator.HasConverged);
        }

        [TestMethod]
        public void HasConverged_OneHighConfidenceResult_Returns_True()
        {
            var aggregator = new ResultAggregator();
            aggregator.AddResult(HighConfidenceResult);
            Assert.IsTrue(aggregator.HasConverged);
        }
    }
}
