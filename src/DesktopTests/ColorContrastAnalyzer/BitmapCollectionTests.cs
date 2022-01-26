﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.ColorContrastAnalyzer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CCColor = Axe.Windows.Desktop.ColorContrastAnalyzer.Color;
using System.Reflection;
using Moq;

namespace Axe.Windows.DesktopTests.ColorContrastAnalyzer
{
    [TestClass()]
    public class BitmapCollectionTests
    {
        private Mock<IColorContrastConfig> _configMock;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _configMock = new Mock<IColorContrastConfig>(MockBehavior.Strict);
        }

        private void VerifyAllMocks()
        {
            _configMock.VerifyAll();
        }

        private void ConfigureConfigMockForAzalyzerVersion(AnalyzerVersion analyzerVersion)
        {
            _configMock.Setup(x => x.AnalyzerVersion)
                .Returns(analyzerVersion)
                .Verifiable();
        }

        [TestMethod]
        public void GetColorContrastCalculator_OptionsSpecifyNoVersion_ReturnsV1()
        {
            ConfigureConfigMockForAzalyzerVersion(AnalyzerVersion.None);
            BitmapCollection collection = new BitmapCollection(null, _configMock.Object);
            Assert.AreEqual(collection.RunColorContrastCalculationV1,
                collection.GetColorContrastCalculator());
            VerifyAllMocks();
        }


        [TestMethod]
        public void GetColorContrastCalculator_OptionsSpecifyV1_ReturnsV1()
        {
            ConfigureConfigMockForAzalyzerVersion(AnalyzerVersion.V1);
            BitmapCollection collection = new BitmapCollection(null, _configMock.Object);
            Assert.AreEqual(collection.RunColorContrastCalculationV1,
                collection.GetColorContrastCalculator());
            VerifyAllMocks();
        }

        [TestMethod]
        public void GetColorContrastCalculator_OptionsSpecifyV2_ReturnsV2()
        {
            ConfigureConfigMockForAzalyzerVersion(AnalyzerVersion.V2);
            BitmapCollection collection = new BitmapCollection(null, _configMock.Object);
            Assert.AreEqual(collection.RunColorContrastCalculationV2,
                collection.GetColorContrastCalculator());
            VerifyAllMocks();
        }
    }
}
