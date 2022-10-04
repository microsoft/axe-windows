// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions.Contexts;
using Axe.Windows.Core.CustomObjects;
using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using System;
using System.Collections.Generic;

namespace Axe.Windows.Actions
{
    public static class CustomUIAAction
    {
        public static Config ReadConfigFromFile(string path) { return Config.ReadFromFile(path); }

        public static void RegisterCustomProperties(IEnumerable<CustomProperty> properties)
        {
            RegisterCustomProperties(properties, DefaultScanContext.GetDefaultInstance());
        }

        internal static void RegisterCustomProperties(IEnumerable<CustomProperty> properties, IScanContext scanContext)
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));
            foreach (CustomProperty p in properties)
            {
                scanContext.Registrar.RegisterCustomProperty(p);
            }
        }
    }
}
