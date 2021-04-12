// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions.Attributes;
using Axe.Windows.Actions.Enums;
using Axe.Windows.Desktop.Drawing;
using Axe.Windows.Desktop.Utility;
using Axe.Windows.SystemAbstractions;
using Axe.Windows.Telemetry;
using System;

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
        internal static Func<DataManager> GetDataManager = () => DataManager.GetDefaultInstance();
        internal static IBitmapCreator BitmapCreator = new FrameworkBitmapCreator();

        /// <summary>
        /// Take a screenshot of the given element's parent window, if it has one
        ///     returns null if the bounding rectangle is 0-sized
        /// </summary>
        /// <param name="ecId">Element Context Id</param>
        public static void CaptureScreenShot(Guid ecId)
        {
            try
            {
                var ec = GetDataManager().GetElementContext(ecId);
                var el = ec.Element;

                var win = el.GetParentWindow();
                el = win ?? el;
                var rect = el.BoundingRectangle;
                if (rect.IsEmpty)
                {
                    return; // no capture.
                }

                ec.DataContext.Screenshot = BitmapCreator.FromScreenRectangle(rect);
                ec.DataContext.ScreenshotElementId = el.UniqueId;
            }
            catch(TypeInitializationException e)
            {
                e.ReportException();
                // silently ignore. since it happens only on WCOS.
                // in this case, the results file will be loaded with yellow box.
            }
        }
    }
}
