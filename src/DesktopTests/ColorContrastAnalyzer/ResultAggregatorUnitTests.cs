// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.ColorContrastAnalyzer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Axe.Windows.DesktopTests.ColorContrastAnalyzer
{
    [TestClass]
    public class ResultAggregatorUnitTests
    {
        private ResultAggregator _testSubject;

        private static readonly ColorContrastResult NoConfidenceResult = 
            BuildColorContrastResult(ColorContrastResult.Confidence.None, null);
        private static readonly ColorContrastResult LowConfidenceResult =
            BuildColorContrastResult(ColorContrastResult.Confidence.Low, Color.BLUE, Color.GREEN);
        private static readonly ColorContrastResult MidConfidenceResult =
            BuildColorContrastResult(ColorContrastResult.Confidence.Mid, Color.BLUE, Color.WHITE);
        private static readonly ColorContrastResult HighConfidenceResult =
            BuildColorContrastResult(ColorContrastResult.Confidence.High, Color.BLACK, Color.WHITE);

        private static ColorContrastResult BuildColorContrastResult(ColorContrastResult.Confidence confidence, Color color1, Color color2)
        {
            ColorPair colorPair = new ColorPair(color1, color2);
            return BuildColorContrastResult(confidence, colorPair);
        }

        private static ColorContrastResult BuildColorContrastResult(ColorContrastResult.Confidence confidence, ColorPair colorPair)
        {
            Mock<ColorContrastResult> resultMock = new Mock<ColorContrastResult>(MockBehavior.Strict);
            resultMock.Setup(x => x.ConfidenceValue())
                .Returns(confidence);
            resultMock.Setup(x => x.GetMostLikelyColorPair())
                .Returns(colorPair);

            return resultMock.Object;
        }

        [TestInitialize]
        public void BeforeEach()
        {
            _testSubject = new ResultAggregator(new DefaultColorContrastConfig());
        }

        [TestMethod]
        [Timeout(2000)]
        public void AssertStaticValues()
        {
            Assert.AreEqual(ColorContrastResult.Confidence.None, NoConfidenceResult.ConfidenceValue());
            Assert.AreEqual(ColorContrastResult.Confidence.Low, LowConfidenceResult.ConfidenceValue());
            Assert.AreEqual(ColorContrastResult.Confidence.Mid, MidConfidenceResult.ConfidenceValue());
            Assert.AreEqual(ColorContrastResult.Confidence.High, HighConfidenceResult.ConfidenceValue());
        }

        [TestMethod]
        [Timeout(2000)]
        public void HasConverged_EmptyAggregator_ReturnsFalse()
        {
            Assert.IsFalse(_testSubject.HasConverged);
        }

        [TestMethod]
        [Timeout(2000)]
        public void MostLikelyResult_EmptyAggregator_ReturnsNull()
        {
            Assert.IsNull(_testSubject.MostLikelyResult);
        }

        [TestMethod]
        [Timeout(2000)]
        public void MostLikelyResult_IncreasingConfidence_ReturnsHighestConfidence()
        {
            ColorContrastResult[] increasingConfidenceResults = 
                { NoConfidenceResult, LowConfidenceResult, MidConfidenceResult, HighConfidenceResult };
            foreach (var result in increasingConfidenceResults)
            {
                _testSubject.AddResult(result);
                Assert.AreEqual(result, _testSubject.MostLikelyResult);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void MostLikelyResult_DecreasingConfidence_ReturnsHighestConfidence()
        {
            ColorContrastResult[] increasingConfidenceResults =
                { HighConfidenceResult, MidConfidenceResult, LowConfidenceResult, NoConfidenceResult };
            foreach (var result in increasingConfidenceResults)
            {
                _testSubject.AddResult(result);
                Assert.AreEqual(HighConfidenceResult, _testSubject.MostLikelyResult);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void HasConverged_OneNoConfidenceResult_Returns_False()
        {
            _testSubject.AddResult(NoConfidenceResult);
            Assert.IsFalse(_testSubject.HasConverged);
        }

        [TestMethod]
        [Timeout(2000)]
        public void HasConverged_OnelOWConfidenceResult_Returns_False()
        {
            _testSubject.AddResult(LowConfidenceResult);
            Assert.IsFalse(_testSubject.HasConverged);
        }

        [TestMethod]
        [Timeout(2000)]
        public void HasConverged_OneMidConfidenceResult_Returns_False()
        {
            _testSubject.AddResult(MidConfidenceResult);
            Assert.IsFalse(_testSubject.HasConverged);
        }

        [TestMethod]
        [Timeout(2000)]
        public void HasConverged_OneHighConfidenceResult_Returns_False()
        {
            _testSubject.AddResult(HighConfidenceResult);
            Assert.IsFalse(_testSubject.HasConverged);
        }

        [TestMethod]
        [Timeout(2000)]
        public void HasConverged_TwoIdenticalHighConfidenceResults_Returns_False()
        {
            _testSubject.AddResult(HighConfidenceResult);
            _testSubject.AddResult(HighConfidenceResult);
            Assert.IsFalse(_testSubject.HasConverged);
        }

        [TestMethod]
        [Timeout(2000)]
        public void HasConverged_ThreeIdenticalHighConfidenceResults_Returns_True()
        {
            _testSubject.AddResult(HighConfidenceResult);
            _testSubject.AddResult(HighConfidenceResult);
            _testSubject.AddResult(HighConfidenceResult);
            Assert.IsTrue(_testSubject.HasConverged);
        }
    }
}
