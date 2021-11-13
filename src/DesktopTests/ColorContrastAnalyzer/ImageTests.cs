// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Desktop.ColorContrastAnalyzer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using static Axe.Windows.Desktop.ColorContrastAnalyzer.ColorContrastResult;
using CCColor = Axe.Windows.Desktop.ColorContrastAnalyzer.Color;
using System.Reflection;

namespace Axe.Windows.DesktopTests.ColorContrastAnalyzer
{
    [TestClass()]
    public class ImageTests
    {

        // A convenience method for loading Bitmap images from test resources.
        public static BitmapCollection LoadFromResources(string name)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("Axe.Windows.DesktopTests.TestImages." + name);
            Bitmap bmp = new Bitmap(myStream);
            return new BitmapCollection(bmp, new DefaultColorContrastConfig());
        }

        [TestMethod]
        [Timeout(1000)]
        public void BitmapCollection_Ctor_ConfigIsNull_Throws()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new BitmapCollection(new Bitmap(1, 1), null));
        }

        [TestMethod, Timeout(2000)]
        public void SimpleBlackAndGreyButton()
        {
            var image = LoadFromResources("simple_black_and_grey_button.bmp");

            var result = image.RunColorContrastCalculation();

            Assert.AreEqual(new ColorPair(new CCColor(204, 204, 204), new CCColor(0, 0, 0)),
                result.MostLikelyColorPair);

            Assert.AreEqual(Confidence.High, result.Confidence);
        }

        // [TestMethod] TODO: we will revisit this test case after issue#231 has been fixed
        [Timeout(2000)]
        public void FutureSimplePurpleAndWhiteButton()
        {
            var image = LoadFromResources("simple_purple_and_white_button.bmp");

            var result = image.RunColorContrastCalculation();

            Assert.AreEqual(new ColorPair(new CCColor(55, 0, 110), new CCColor(255, 255, 255)),
                result.MostLikelyColorPair);

            Assert.AreEqual(Confidence.Low, result.Confidence);
        }

        // [TestMethod] TODO: we will revisit this test case after issue#231 has been fixed
        [Timeout(2000)]
        public void FutureSimpleBlueAndWhiteText()
        {
            var image = LoadFromResources("simple_blue_and_white_text.bmp");

            var result = image.RunColorContrastCalculation();

            Assert.AreEqual(new ColorPair(new CCColor(27, 12, 170), new CCColor(255, 255, 255)),
                result.MostLikelyColorPair);

            Assert.AreEqual(Confidence.Low, result.Confidence);
        }

        [TestMethod, Timeout(2000)]
        public void SimpleGreyAndWhiteTitle()
        {
            var image = LoadFromResources("simple_grey_and_white_title.bmp");

            var result = image.RunColorContrastCalculation();

            Assert.AreEqual(new ColorPair(new CCColor(171, 153, 153), new CCColor(255, 255, 255)),
                result.MostLikelyColorPair);

            Assert.AreEqual(Confidence.High, result.Confidence);
        }

        [TestMethod, Timeout(2000)]
        public void SimpleBlackAndWhiteTitle()
        {
            var image = LoadFromResources("simple_black_and_white_title.bmp");

            var result = image.RunColorContrastCalculation();

            Assert.AreEqual(new ColorPair(new CCColor(0, 0, 0), new CCColor(255, 255, 255)),
                result.MostLikelyColorPair);

            Assert.AreEqual(Confidence.Low, result.Confidence);
        }

        [TestMethod, Timeout(2000)]
        public void SimpleWhiteAndBlackText()
        {
            var image = LoadFromResources("simple_white_and_black_text.bmp");

            var result = image.RunColorContrastCalculation();

            Assert.AreEqual(new ColorPair(new CCColor(0, 0, 0), new CCColor(255, 255, 255)),
                result.MostLikelyColorPair);

            Assert.AreEqual(Confidence.High, result.Confidence);
        }

        /**
         * In this test we are analyzing two similar images. One with text near the bottom of the image
         * and the other with the text near the top. The results should be identical.
         */
        [TestMethod, Timeout(2000)]
        public void CortanaImagesWithDifferentOffsets()
        {
            ColorPair expected = new ColorPair(new CCColor(0, 0, 0), new CCColor(139, 204, 41));

            ColorContrastResult resultOffsetDownImage = LoadFromResources("cortana_with_offset_down.bmp")
                .RunColorContrastCalculation();

            ColorContrastResult resultOffsetUpImage = LoadFromResources("cortana_with_offset_up.bmp")
                .RunColorContrastCalculation();

            Assert.AreEqual(expected, resultOffsetUpImage.MostLikelyColorPair);
            Assert.AreEqual(expected, resultOffsetDownImage.MostLikelyColorPair);

            Assert.AreEqual(Confidence.High, resultOffsetDownImage.Confidence);
            Assert.AreEqual(Confidence.High, resultOffsetUpImage.Confidence);
        }

        [TestMethod, Timeout(2000)]
        public void VisualStudioTab()
        {
            ColorPair approximateColorPair = new ColorPair(new CCColor(9, 126, 206), new CCColor(37, 37, 38));

            var colorContrastResult = LoadFromResources("visual_studio_tab.bmp").RunColorContrastCalculation();

            Assert.AreEqual(Confidence.Mid, colorContrastResult.Confidence);

            Assert.AreEqual(approximateColorPair, colorContrastResult.MostLikelyColorPair);
        }

        /**
         * Note in this test case we have Mide confidence. As such, we also are asserting that the color is only
         * approximately what we expect, this allows our algorithm a little flexibility, without having to modify
         * these tests every time we sneeze on our configuration file.
         */
        [TestMethod, Timeout(2000)]
        public void WeirdTextArrangement()
        {
            var image = LoadFromResources("weird_text_arrangement.bmp");

            ColorPair approximateColorPair = new ColorPair(new CCColor(177, 199, 188), new CCColor(30, 30, 30));

            ColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(approximateColorPair, result.MostLikelyColorPair);

            Assert.AreEqual(Confidence.High, result.Confidence);
        }

        [TestMethod, Timeout(2000)]
        public void BinaryRowSearchIterator()
        {

            List<int> rowOrder = new List<int>
            {
                23, 11, 17, 34, 40
            };

            foreach (var pixel in LoadFromResources("simple_black_and_grey_button.bmp").GetBinaryRowSearchIterator())
            {
                if (pixel.Row != rowOrder.First())
                {
                    rowOrder.RemoveAt(0);
                }

                Assert.AreEqual(rowOrder.First(), pixel.Row);
            }

            rowOrder.RemoveAt(0);

            Assert.AreEqual(0, rowOrder.Count(), "We should have inspected all expected rows.");
        }
    }
}
