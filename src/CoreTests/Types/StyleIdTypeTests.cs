// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.Core.Types.Tests
{
    /// <summary>
    /// Tests for StyleIdType class
    /// </summary>
    [TestClass()]
    public class StyleIdTypeTests
    {
        [TestMethod()]
        public void StyleId_Custom()
        {
            Assert.AreEqual("Custom(70000)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_Custom));
        }

        [TestMethod()]
        public void StyleId_Heading1()
        {
            Assert.AreEqual("Heading1(70001)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_Heading1));
        }

        [TestMethod()]
        public void StyleId_Heading2()
        {
            Assert.AreEqual("Heading2(70002)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_Heading2));
        }

        [TestMethod()]
        public void StyleId_Heading3()
        {
            Assert.AreEqual("Heading3(70003)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_Heading3));
        }

        [TestMethod()]
        public void StyleId_Heading4()
        {
            Assert.AreEqual("Heading4(70004)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_Heading4));
        }

        [TestMethod()]
        public void StyleId_Heading5()
        {
            Assert.AreEqual("Heading5(70005)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_Heading5));
        }

        [TestMethod()]
        public void StyleId_Heading6()
        {
            Assert.AreEqual("Heading6(70006)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_Heading6));
        }

        [TestMethod()]
        public void StyleId_Heading7()
        {
            Assert.AreEqual("Heading7(70007)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_Heading7));
        }

        [TestMethod()]
        public void StyleId_Heading8()
        {
            Assert.AreEqual("Heading8(70008)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_Heading8));
        }

        [TestMethod()]
        public void StyleId_Heading9()
        {
            Assert.AreEqual("Heading9(70009)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_Heading9));
        }

        [TestMethod()]
        public void StyleId_Title()
        {
            Assert.AreEqual("Title(70010)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_Title));
        }

        [TestMethod()]
        public void StyleId_Subtitle()
        {
            Assert.AreEqual("Subtitle(70011)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_Subtitle));
        }

        [TestMethod()]
        public void StyleId_Normal()
        {
            Assert.AreEqual("Normal(70012)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_Normal));
        }

        [TestMethod()]
        public void StyleId_Emphasis()
        {
            Assert.AreEqual("Emphasis(70013)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_Emphasis));
        }

        [TestMethod()]
        public void StyleId_Quote()
        {
            Assert.AreEqual("Quote(70014)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_Quote));
        }

        [TestMethod()]
        public void StyleId_BulletedList()
        {
            Assert.AreEqual("BulletedList(70015)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_BulletedList));
        }

        [TestMethod()]
        public void StyleId_NumberedList()
        {
            Assert.AreEqual("NumberedList(70016)", StyleIdType.GetInstance().GetNameById(StyleIdType.StyleId_NumberedList));
        }

        [TestMethod()]
        public void StyleId_Unknown()
        {
            Assert.AreEqual("Unknown(70099)", StyleIdType.GetInstance().GetNameById(70099));
        }
    }
}
