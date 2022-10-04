// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions.Attributes;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Actions.Enums;
using Axe.Windows.Desktop.Utility;
using Axe.Windows.Telemetry;
using System;
using System.Drawing;

namespace Axe.Windows.Actions
{
    /// <summary>
    /// Class SelectionAction
    /// this class is to select unelement via focus or keyboard
    /// </summary>
    [InteractionLevel(UxInteractionLevel.NoUxInteraction)]
    public static class ScreenShotAction
    {
        // unit test hooks
        internal static Func<int, int, Bitmap> CreateBitmap = (width, height) => new Bitmap(width, height);
        internal static readonly Action<Graphics, int, int, Size> DefaultCopyFromScreen = (g, x, y, s) => g.CopyFromScreen(x, y, 0, 0, s);
        internal static Action<Graphics, int, int, Size> CopyFromScreen = DefaultCopyFromScreen;

        /// <summary>
        /// Take a screenshot of the given element's parent window, if it has one
        ///     returns null if the bounding rectangle is 0-sized
        /// </summary>
        /// <param name="ecId">Element Context Id</param>
        public static void CaptureScreenShot(Guid ecId)
        {
            CaptureScreenShot(ecId, DefaultActionContext.GetDefaultInstance());
        }

        /// <summary>
        /// Take a screenshot of the given element's parent window, if it has one
        ///     returns null if the bounding rectangle is 0-sized
        /// </summary>
        /// <param name="ecId">Element Context Id</param>
        internal static void CaptureScreenShot(Guid ecId, IActionContext actionContext)
        {
            try
            {
                var ec = actionContext.DataManager.GetElementContext(ecId);
                var el = ec.Element;

                var win = el.GetParentWindow();
                el = win ?? el;
                var rect = el.BoundingRectangle;
                if (rect.IsEmpty)
                {
                    return; // no capture.
                }

                Bitmap bmp = CreateBitmap(rect.Width, rect.Height);
                Graphics g = Graphics.FromImage(bmp);

                CopyFromScreen(g, rect.X, rect.Y, rect.Size);

                ec.DataContext.Screenshot = bmp;
                ec.DataContext.ScreenshotElementId = el.UniqueId;
            }
            catch (TypeInitializationException e)
            {
                e.ReportException();
                // silently ignore. since it happens only on WCOS.
                // in this case, the results file will be loaded with yellow box.
            }
        }
    }
}
