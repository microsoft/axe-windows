// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Axe.Windows.Actions.Misc.Tests
{
    [TestClass()]
    public class ExtensionMethodsTests
    {
        /// <summary>
        /// Create 10 elements with bounding rectangle (i, i, i, i) from [0, 9]
        /// See which one is identified as smallest enclosing area of (5, 5)
        /// </summary>
        [TestMethod()]
        public void GetSmallestElementFromPointTest()
        {
            using (ShimsContext.Create())
            {
                var boundingRects = new List<System.Drawing.Rectangle>();
                var offscreen = new List<bool>();
                for (int i = 0; i < 10; i++)
                {
                    boundingRects.Add(new System.Drawing.Rectangle(i, i, i, i));
                    offscreen.Add(false);
                }
                var elements = CreateA11yElementsFromBoundingRectangles(boundingRects, offscreen);
                var answer = ExtensionMethods.GetSmallestElementFromPoint(elements.ToDictionary(e => e.UniqueId, e => e), new System.Drawing.Point(5, 5));
                Assert.AreEqual(3, answer.UniqueId);
            }
        }

        /// <summary>
        /// Create 10 elements with bounding rectangle (i, i, i, i) from [0, 9]
        /// See which one is identified as smallest enclosing area of (5, 5)
        /// Normal answer (3, 3, 3, 3) is offscreen
        /// </summary>
        [TestMethod()]
        public void GetSmallestElementFromPointTest_Offscreen()
        {
            using (ShimsContext.Create())
            {
                var boundingRects = new List<System.Drawing.Rectangle>();
                var offscreen = new List<bool>();
                for (int i = 0; i < 10; i++)
                {
                    boundingRects.Add(new System.Drawing.Rectangle(i, i, i, i));
                    offscreen.Add(false);
                }
                offscreen[3] = true;
                var elements = CreateA11yElementsFromBoundingRectangles(boundingRects, offscreen);
                var answer = ExtensionMethods.GetSmallestElementFromPoint(elements.ToDictionary(e => e.UniqueId, e => e), new System.Drawing.Point(5, 5));
                Assert.AreEqual(4, answer.UniqueId);
            }
        }

        /// <summary>
        /// Create 10 elements with bounding rectangle (i, i, i, i) from [0, 9]
        /// See which one is identified as smallest enclosing area of (200, 200)
        /// </summary>
        [TestMethod()]
        public void GetSmallestElementFromPointTest_NoneOverlaps()
        {
            using (ShimsContext.Create())
            {
                var boundingRects = new List<System.Drawing.Rectangle>();
                var offscreen = new List<bool>();
                for (int i = 0; i < 10; i++)
                {
                    boundingRects.Add(new System.Drawing.Rectangle(i, i, i, i));
                    offscreen.Add(false);
                }
                var elements = CreateA11yElementsFromBoundingRectangles(boundingRects, offscreen);
                var answer = ExtensionMethods.GetSmallestElementFromPoint(elements.ToDictionary(e => e.UniqueId, e => e), new System.Drawing.Point(200, 200));
                Assert.AreEqual(null, answer);
            }
        }
        
        [TestMethod()]
        public void GetSmallestElementFromPointTest_NullArgument()
        {
            using (ShimsContext.Create())
            {
                Assert.ThrowsException<ArgumentNullException>(() => ExtensionMethods.GetSmallestElementFromPoint(null, System.Drawing.Point.Empty));
            }
        }

        [TestMethod()]
        public void GetSmallestElementFromPointTest_NoElementsProvided()
        {
            using (ShimsContext.Create())
            {
                Assert.ThrowsException<ArgumentException>(() => ExtensionMethods.GetSmallestElementFromPoint(new Dictionary<int, ICoreA11yElement>(), System.Drawing.Point.Empty));
            }
        }

        /// <summary>
        /// Creates and returns list of IA11yElements where 
        ///     returnedList[i].BoundingRectangle == boundingRects[i] and
        ///     returnedList[i].UniqueId == i and
        ///     returnedList[i].IsOffScreen() == offScreen[i]
        /// </summary>
        /// <returns></returns>
        private static List<ICoreA11yElement> CreateA11yElementsFromBoundingRectangles(List<System.Drawing.Rectangle> boundingRects, List<bool> offScreen)
        {
            var elements = new List<ICoreA11yElement>();
            for (int i = 0; i < boundingRects.Count; i++)
            {
                Mock<ICoreA11yElement> mockElement = new Mock<ICoreA11yElement>();

                mockElement.Setup(x => x.UniqueId).Returns(i);
                mockElement.Setup(x => x.BoundingRectangle).Returns(boundingRects[i]);
                mockElement.Setup(x => x.RuntimeId).Returns(i.ToString(CultureInfo.InvariantCulture));
                mockElement.Setup(x => x.IsOffScreen).Returns(offScreen[i]);
                elements.Add(mockElement.Object);
            }
            return elements;
        }
    }
}
 
