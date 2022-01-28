// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.ColorContrastAnalyzer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.IO;
using CCColor = Axe.Windows.Desktop.ColorContrastAnalyzer.Color;
using System.Reflection;

namespace Axe.Windows.DesktopTests.ColorContrastAnalyzer
{
    [TestClass()]
    public class ImageTestsV2
    {
        // A convenience method for loading Bitmap images from test resources.
        public static BitmapCollection LoadFromResources(string name)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("Axe.Windows.DesktopTests.TestImages." + name);
            Bitmap bmp = new Bitmap(myStream);
            return new BitmapCollection(bmp, new ConfigBuilder().WithAnalyzerVersion(AnalyzerVersion.V2).Build());
        }

        [TestMethod]
        [Timeout(1000)]
        public void BitmapCollection_Ctor_ConfigIsNull_Throws()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new BitmapCollection(new Bitmap(1, 1), null));
        }

        [TestMethod]
        [Timeout(2000)]
        public void SimpleBlackAndGreyButton()
        {
            var image = LoadFromResources("simple_black_and_grey_button.bmp");

            var result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(204, 204, 204), new CCColor(0, 0, 0)),
                result.MostLikelyColorPair);
        }

        [TestMethod] 
        [Timeout(2000)]
        public void SimplePurpleAndWhiteButton()
        {
            var image = LoadFromResources("simple_purple_and_white_button.bmp");

            var result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(55, 0, 110), new CCColor(255, 255, 255)),
                result.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void SimpleBlueAndWhiteText()
        {
            var image = LoadFromResources("simple_blue_and_white_text.bmp");

            var result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(255, 255, 255), new CCColor(32, 13, 159)),
                result.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void SimpleGreyAndWhiteTitle()
        {
            var image = LoadFromResources("simple_grey_and_white_title.bmp");

            var result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(255, 255, 255), new CCColor(153, 153, 153)),
                result.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void SimpleBlackAndWhiteTitle()
        {
            var image = LoadFromResources("simple_black_and_white_title.bmp");

            var result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(0, 0, 0), new CCColor(255, 255, 255)),
                result.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
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
         * and the other with the text near the top. The results should be similar.
         */
        [TestMethod]
        [Timeout(2000)]
        public void CortanaImagesWithDifferentOffsets()
        {
            ColorPair expectedUp = new ColorPair(new CCColor(0, 0, 0), new CCColor(139, 204, 41));
            ColorPair expectedDown = new ColorPair(new CCColor(0, 0, 0), new CCColor(142, 208, 42));

            IColorContrastResult resultOffsetDownImage = LoadFromResources("cortana_with_offset_down.bmp")
                .RunColorContrastCalculation();

            IColorContrastResult resultOffsetUpImage = LoadFromResources("cortana_with_offset_up.bmp")
                .RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, resultOffsetDownImage.Confidence);
            Assert.AreEqual(Confidence.High, resultOffsetUpImage.Confidence);

            Assert.AreEqual(expectedUp, resultOffsetUpImage.MostLikelyColorPair);
            Assert.AreEqual(expectedDown, resultOffsetDownImage.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void VisualStudioTab()
        {
            var colorContrastResult = LoadFromResources("visual_studio_tab.bmp").RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, colorContrastResult.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(6, 135, 217), new CCColor(37, 37, 38)),
                colorContrastResult.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void WeirdTextArrangement()
        {
            var image = LoadFromResources("weird_text_arrangement.bmp");

            IColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.Mid, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(30, 30, 30), new CCColor(199, 207, 188)),
                result.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void WildlifeManager_ListboxBeetle()
        {
            var image = LoadFromResources("wildlife_manager_listbox_beetle.bmp");

            IColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(229, 243, 251), new CCColor(0, 0, 0)),
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

            Assert.AreEqual(new ColorPair(new CCColor(229, 243, 251), new CCColor(0, 0, 0)),
                result.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void WildlifeManager_ListboxOwl_Cropped()
        {
            var image = LoadFromResources("wildlife_manager_listbox_owl_cropped.bmp");

            IColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(229, 243, 251), new CCColor(0, 0, 0)),
                result.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void WildlifeManager_SpeciesLabel()
        {
            var image = LoadFromResources("wildlife_manager_species_label.bmp");

            IColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(255, 255, 255), new CCColor(0, 0, 0)),
                result.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void ButtonIconAntialiased()
        {
            var image = LoadFromResources("button_icon_antialiased.bmp");

            IColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(247, 247, 247), new CCColor(0, 0, 0)),
                result.MostLikelyColorPair);
        }

        [TestMethod]
        [Timeout(2000)]
        public void OutlookTranslate()
        {
            // TODO: Can V2 analyzer return more accurate results?
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
            // TODO: Can V2 analyzer return more accurate results?
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
            // TODO: Can V2 analyzer return more accurate results?
            var image = LoadFromResources("outlook_get_add_ins.bmp");

            IColorContrastResult result = image.RunColorContrastCalculation();

            Assert.AreEqual(Confidence.High, result.Confidence);

            Assert.AreEqual(new ColorPair(new CCColor(216, 59, 1), new CCColor(41, 41, 41)),
                result.MostLikelyColorPair);
        }
    }
}
