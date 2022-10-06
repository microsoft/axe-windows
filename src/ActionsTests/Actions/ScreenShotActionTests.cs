// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Axe.Windows.ActionsTests.Actions
{
    [TestClass]
    public class ScreenShotActionTests
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
            ScreenShotAction.CreateBitmap = (w, h) => new Bitmap(w, h);
            ScreenShotAction.CopyFromScreen = ScreenShotAction.DefaultCopyFromScreen;
        }

        [TestMethod]
        [Timeout(2000)]
        public void CaptureScreenShot_ElementWithoutBoundingRectangle_NoScreenShot()
        {
            using (var actionContext = ScopedActionContext.CreateInstance())
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

                actionContext.DataManager.AddElementContext(elementContext);

                ScreenShotAction.CaptureScreenShot(elementContext.Id, actionContext);

                Assert.IsNull(dc.Screenshot);
                Assert.AreEqual(default(int), dc.ScreenshotElementId);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void CaptureScreenShot_ElementWithBoundingRectangle_ScreenShotCreated()
        {
            using (var actionContext = ScopedActionContext.CreateInstance())
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

                actionContext.DataManager.AddElementContext(elementContext);

                ScreenShotAction.CopyFromScreen = (g, x, y, s) => { };

                ScreenShotAction.CaptureScreenShot(elementContext.Id, actionContext);

                Assert.IsNotNull(dc.Screenshot);
                Assert.AreEqual(element.UniqueId, dc.ScreenshotElementId);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void CaptureScreenShotOnWCOS_ElementWithBoundingRectangle_NoScreenShot()
        {
            using (var actionContext = ScopedActionContext.CreateInstance())
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

                actionContext.DataManager.AddElementContext(elementContext);

                ScreenShotAction.CreateBitmap = (w, h) => throw new TypeInitializationException("Bitmap", null);

                ScreenShotAction.CaptureScreenShot(elementContext.Id, actionContext);

                Assert.IsNull(dc.Screenshot);
                Assert.AreEqual(default(int), dc.ScreenshotElementId);
            }
        }
    }
}
