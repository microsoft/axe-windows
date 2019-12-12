// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Types;
using System;
using UIAutomationClient;

namespace Axe.Windows.Rules.Misc
{
    static class Helpers
    {
        private static readonly Lazy<IUIAutomationElement> _desktop = new Lazy<IUIAutomationElement>(GetDesktopElement);
        public static IUIAutomationElement Desktop => _desktop.Value;

        private static IUIAutomationElement GetDesktopElement()
        {
            IUIAutomation uia = new CUIAutomation();

            var cacheRequest = uia.CreateCacheRequest();
            cacheRequest.AddProperty(PropertyType.UIA_BoundingRectanglePropertyId);

            return uia.GetRootElementBuildCache(cacheRequest);
        }
    } // class
} // namespace
