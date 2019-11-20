// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Actions.Contexts.Fakes;
using Axe.Windows.Actions.Fakes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Fakes;

namespace Axe.Windows.ActionsTests.Actions
{
    [TestClass]
    public class ScreenShotActionUnitTests
    {
        // This sets the Bounding Rectangle. It breaks encapsulation in
        // many reproachable ways
        private void SetBoundingRectangle(A11yElement element, Rectangle b)
        {
            int typeId = PropertyType.UIA_BoundingRectanglePropertyId;
            double[] data = { b.Left, b.Top, b.Right - b.Left, b.Bottom - b.Top };
            if (element.Properties == null)
                element.Properties = new Dictionary<int, A11yProperty>();
            element.Properties.Add(typeId, new A11yProperty(typeId, data));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ScreenShotAction.GetDataManager = () => DataManager.GetDefaultInstance();
            ScreenShotAction.CreateBitmap = (w, h) => new Bitmap(w, h);
        }

        [TestMethod]
        [Timeout(2000)]
        public void CaptureScreenShot_ElementWithoutBoundingRectangle_NoScreenShot()
        {
            using (var dm = new DataManager())
            {
                // no bounding rectangle.
                A11yElement element = new A11yElement
                {
                    UniqueId = 42,
                };

                var dc = new ElementDataContext(element, 1);

                var elementContext = new ElementContext(element)
                {
                    DataContext = dc,
                };

                dm.AddElementContext(elementContext);

                ScreenShotAction.GetDataManager = () => dm;
                
                ScreenShotAction.CaptureScreenShot(elementContext.Id);

                Assert.IsNull(dc.Screenshot);
                Assert.AreEqual(default(int), dc.ScreenshotElementId);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void CaptureScreenShot_ElementWithBoundingRectangle_ScreenShotCreated()
        {
            using (var dm = new DataManager())
            {
                A11yElement element = new A11yElement
                {
                    UniqueId = 42,
                };

                SetBoundingRectangle(element, new Rectangle(0, 0, 10, 10));

                var dc = new ElementDataContext(element, 1);

                var elementContext = new ElementContext(element)
                {
                    DataContext = dc,
                };

                dm.AddElementContext(elementContext);

                ScreenShotAction.GetDataManager = () => dm;

                ScreenShotAction.CaptureScreenShot(elementContext.Id);

                Assert.IsNotNull(dc.Screenshot);
                Assert.AreEqual(element.UniqueId, dc.ScreenshotElementId);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void CaptureScreenShotOnWCOS_ElementWithBoundingRectangle_NoScreenShot()
        {
            using (var dm = new DataManager())
            {
                A11yElement element = new A11yElement
                {
                    UniqueId = 42,
                };

                SetBoundingRectangle(element, new Rectangle(0, 0, 10, 10));

                var dc = new ElementDataContext(element, 1);

                var elementContext = new ElementContext(element)
                {
                    DataContext = dc,
                };

                dm.AddElementContext(elementContext);

                ScreenShotAction.GetDataManager = () => dm;
                ScreenShotAction.CreateBitmap = (w, h) => throw new TypeInitializationException("Bitmap", null);

                ScreenShotAction.CaptureScreenShot(elementContext.Id);

                Assert.IsNull(dc.Screenshot);
                Assert.AreEqual(default(int), dc.ScreenshotElementId);
            }
        }
    }
}
