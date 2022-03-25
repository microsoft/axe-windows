// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Desktop.ColorContrastAnalyzer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using CCColor = Axe.Windows.Desktop.ColorContrastAnalyzer.Color;

namespace Axe.Windows.DesktopTests.ColorContrastAnalyzer
{
    [TestClass()]
    public class ImageTests
    {
        private static readonly IColorContrastConfig ColorContrastConfig =
            new ColorContrastConfigBuilder().Build();

        // A convenience method for loading Bitmap images from test resources.
        public static BitmapCollection LoadFromResources(string name)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            using (Stream myStream = myAssembly.GetManifestResourceStream("Axe.Windows.DesktopTests.TestImages." + name))
            {
                Bitmap bmp = new Bitmap(myStream);
                return new BitmapCollection(bmp, ColorContrastConfig);
            }
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

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(204, 204, 204), new CCColor(0, 0, 0)),
                result.MostLikelyColorPair);
        }

        // [TestMethod] TODO: we will revisit this test case after issue#231 has been fixed
        [Timeout(2000)]
        public void FutureSimplePurpleAndWhiteButton()
        {
            var image = LoadFromResources("simple_purple_and_white_button.bmp");

            var result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.Low, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(55, 0, 110), new CCColor(255, 255, 255)),
                result.MostLikelyColorPair);
        }

        // [TestMethod] TODO: we will revisit this test case after issue#231 has been fixed
        [Timeout(2000)]
        public void FutureSimpleBlueAndWhiteText()
        {
            var image = LoadFromResources("simple_blue_and_white_text.bmp");

            var result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.Low, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(27, 12, 170), new CCColor(255, 255, 255)),
                result.MostLikelyColorPair);
        }

        [TestMethod, Timeout(2000)]
        public void SimpleGreyAndWhiteTitle()
        {
            var image = LoadFromResources("simple_grey_and_white_title.bmp");

            var result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(171, 153, 153), new CCColor(255, 255, 255)),
                result.MostLikelyColorPair);
        }

        [TestMethod, Timeout(2000)]
        public void SimpleBlackAndWhiteTitle()
        {
            var image = LoadFromResources("simple_black_and_white_title.bmp");

            var result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.Low, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(0, 0, 0), new CCColor(255, 255, 255)),
                result.MostLikelyColorPair);
        }

        [TestMethod, Timeout(2000)]
        public void SimpleWhiteAndBlackText()
        {
            var image = LoadFromResources("simple_white_and_black_text.bmp");

            var result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(0, 0, 0), new CCColor(255, 255, 255)),
                result.MostLikelyColorPair);
        }

        /**
         * In this test we are analyzing two similar images. One with text near the bottom of the image
         * and the other with the text near the top. The results should be identical.
         */
        [TestMethod, Timeout(2000)]
        public void CortanaImagesWithDifferentOffsets()
        {
            ColorPair expected = new ColorPair(new CCColor(0, 0, 0), new CCColor(139, 204, 41));

            IColorContrastResult resultOffsetDownImage = LoadFromResources("cortana_with_offset_down.bmp")
                .RunColorContrastCalculation();

            IColorContrastResult resultOffsetUpImage = LoadFromResources("cortana_with_offset_up.bmp")
                .RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, resultOffsetDownImage.Confidence);
            Assert.AreEqual(Confidence.High, resultOffsetUpImage.Confidence);

            Assert.AreEqual(expected, resultOffsetUpImage.MostLikelyColorPair);
            Assert.AreEqual(expected, resultOffsetDownImage.MostLikelyColorPair);
        }

        [TestMethod, Timeout(2000)]
        public void VisualStudioTab()
        {
            var colorContrastResult = LoadFromResources("visual_studio_tab.bmp").RunColorContrastCalculation();

            Assert.AreEqual(Confidence.Mid, colorContrastResult.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(9, 126, 206), new CCColor(37, 37, 38)),
                colorContrastResult.MostLikelyColorPair);
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

            IColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(177, 199, 188), new CCColor(30, 30, 30)),
                result.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void WildlifeManager_ListboxBeetle()
        {
            var image = LoadFromResources("wildlife_manager_listbox_beetle.bmp");

            IColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(229, 243, 251), new CCColor(0, 51, 143)),
                result.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void WildlifeManager_ListboxMouse()
        {
            var image = LoadFromResources("wildlife_manager_listbox_mouse.bmp");

            IColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(229, 243, 251), new CCColor(0, 0, 0)),
                result.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void WildlifeManager_ListboxOwl()
        {
            var image = LoadFromResources("wildlife_manager_listbox_owl.bmp");

            IColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(229, 243, 251), new CCColor(56, 3, 0)),
                result.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void WildlifeManager_ListboxOwl_Cropped()
        {
            var image = LoadFromResources("wildlife_manager_listbox_owl_cropped.bmp");

            IColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.None, result.Confidence);
        }

        [TestMethod]
        [Timeout(2000)]
        public void WildlifeManager_SpeciesLabel()
        {
            var image = LoadFromResources("wildlife_manager_species_label.bmp");

            IColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.Mid, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(255, 255, 255), new CCColor(0, 0, 0)),
                result.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void ButtonIconAntialiased()
        {
            var image = LoadFromResources("button_icon_antialiased.bmp");

            IColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.None, result.Confidence);
        }

        [TestMethod]
        [Timeout(2000)]
        public void OutlookTranslate()
        {
            var image = LoadFromResources("outlook_translate.bmp");

            IColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(16, 110, 190), new CCColor(41, 41, 41)),
                result.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void OutlookShareToTeams()
        {
            var image = LoadFromResources("outlook_share_to_teams.bmp");

            IColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(123, 131, 235), new CCColor(41, 41, 41)),
                result.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void OutlookGetAddIns()
        {
            var image = LoadFromResources("outlook_get_add_ins.bmp");

            IColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(216, 59, 1), new CCColor(41, 41, 41)),
                result.MostLikelyColorPair);
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
