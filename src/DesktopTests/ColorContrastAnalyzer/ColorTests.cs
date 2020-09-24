// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.ColorContrastAnalyzer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Axe.Windows.DesktopTests.ColorContrastAnalyzer
{
    [TestClass()]
    public class ColorTests
    {
        [TestMethod, Timeout(2000)]
        public void ColorContrastTest_BlackAndWhite_CorrectAndReflexive()
        {
            Color black = new Color(0, 0, 0);
            Color white = new Color(255, 255, 255);

            Assert.AreEqual(21.0, black.Contrast(white));
            Assert.AreEqual(21.0, white.Contrast(black));
        }

        [TestMethod, Timeout(2000)]
        public void ColorContrastTest_WhiteAndRed_CorrectAndReflexive()
        {
            Assert.AreEqual(4.00, Math.Round(Color.RED.Contrast(Color.WHITE), 2));
            Assert.AreEqual(4.00, Math.Round(Color.WHITE.Contrast(Color.RED), 2));
        }

        [TestMethod, Timeout(2000)]
        public void ColorContrastTest_RedAndBlue_CorrectAndReflexive()
        {
            Assert.AreEqual(2.15, Math.Round(Color.RED.Contrast(Color.BLUE), 2));
            Assert.AreEqual(2.15, Math.Round(Color.BLUE.Contrast(Color.RED), 2));
        }

        [TestMethod, Timeout(2000)]
        public void ColorContrastTest_RedAndGreen_CorrectAndReflexive()
        {
            Assert.AreEqual(2.91, Math.Round(Color.RED.Contrast(Color.GREEN), 2));
            Assert.AreEqual(2.91, Math.Round(Color.GREEN.Contrast(Color.RED), 2));
        }

        [TestMethod, Timeout(2000)]
        public void LuminanceOf_White()
        {
            Assert.AreEqual(1, new Color(255, 255, 255).Luminance());
        }

        [TestMethod, Timeout(2000)]
        public void LuminanceOf_Black()
        {
            Assert.AreEqual(0, new Color(0, 0, 0).Luminance());
        }
    }
}