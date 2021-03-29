// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Misc;
using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.UIAutomation;
using Axe.Windows.Telemetry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using UIAutomationClient;

namespace Axe.Windows.Desktop.Utility
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Get Process name
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetProcessName(this A11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            try
            {
                var prc = Process.GetProcessById(e.ProcessId);

                return prc.ProcessName;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                ex.ReportException();
                return null;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        /// <summary>
        /// Get Process Main Module information
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static ProcessModule GetProcessModule(this A11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            try
            {
                var prc = Process.GetProcessById(e.ProcessId);

                return prc.MainModule;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                ex.ReportException();
                return null;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        /// <summary>
        /// Change IUIAutomationElementArray To an IList of DesktopElement
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static IList<DesktopElement> ToListOfDesktopElements(this IUIAutomationElementArray array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            List<DesktopElement> list = new List<DesktopElement>();

            for (int i = 0; i < array.Length; i++)
            {
                list.Add(new DesktopElement(array.GetElement(i)));
            }

            return list;
        }

        /// <summary>
        /// Capture the bitmap of the given element
        /// </summary>
        /// <param name="e"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap CaptureBitmap(this A11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var rect = e.BoundingRectangle;

            Bitmap bmp = new Bitmap(rect.Width, rect.Height);
            Graphics g = Graphics.FromImage(bmp);

            g.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size);

            return bmp;
        }

        /// <summary>
        /// Gets window that is eventual parent of element. If there is no window parent, get
        /// highest element that isn't desktop and has a bounding rectangle
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        public static A11yElement GetParentWindow(this A11yElement el)
        {
            A11yElement prev = null;
            A11yElement curr = el;
            while (curr != null && curr.Parent != null && curr.ControlTypeId != ControlType.UIA_WindowControlTypeId && !curr.IsRootElement())
            {
                prev = curr;
                curr = curr.Parent;
            }

            if (curr.ControlTypeId != ControlType.UIA_WindowControlTypeId && prev != null)
            {
                curr = prev;
                while (curr.UniqueId < -1 && curr.Children?[0].BoundingRectangle == null)
                {
                    if (curr.Children.Count > 0)
                    {
                        curr = curr.Children[0];
                    }
                    else
                    {
                        // if no elements have bounding rectangles, give up
                        break;
                    }
                }
            }
            return curr;
        }

        /// <summary>
        /// Convert a Url string to a Uri, with proper handling of null inputs
        /// </summary>
        /// <param name="stringValue">The url (potentially empty or null) to convert</param>
        /// <returns>The Uri if the url is neither empty nor null</returns>
        public static Uri ToUri(this string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue))
                return null;

            return new Uri(stringValue);
        }
    }
}
