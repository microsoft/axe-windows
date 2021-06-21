// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using System;
using System.Collections.Generic;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation
{
    /// <summary>
    /// A helper class to manage the cached properties that we always request from UIA
    /// </summary>
    public static class DesktopElementHelper
    {
        /// <summary>
        /// Build a cacherequest for properties and patterns
        /// </summary>
        /// <param name="pps">Property ids</param>
        /// <param name="pts">Pattern ids</param>
        public static IUIAutomationCacheRequest BuildCacheRequest(IEnumerable<int> pps, IEnumerable<int> pts)
        {
            return GetPropertiesCache(A11yAutomation.UIAutomationObject, pps, pts);
        }

        /// <summary>
        /// Build a cacherequest for properties and patterns
        /// </summary>
        /// <param name="uia"></param>
        /// <param name="pps">Property ids</param>
        /// <param name="pts">Pattern ids</param>
        /// <returns></returns>
        public static IUIAutomationCacheRequest GetPropertiesCache(IUIAutomation uia, IEnumerable<int> pps, IEnumerable<int> pts)
        {
            if (uia == null) throw new ArgumentNullException(nameof(uia));

            var cr = uia.CreateCacheRequest();

            if (pps != null)
            {
                foreach (var pp in pps)
                {
                    cr.AddProperty(pp);
                }
            }

            IEnumerable<int> cps = Registrar.GetDefaultInstance().GetCustomPropertyRegistrations().Keys;
            foreach (var cp in cps)
            {
                cr.AddProperty(cp);
            }

            if (pts != null)
            {
                foreach (var pt in pts)
                {
                    if (pt != 0)
                    {
                        cr.AddPattern(pt);
                    }
                }
            }

            return cr;
        }
    }
}
