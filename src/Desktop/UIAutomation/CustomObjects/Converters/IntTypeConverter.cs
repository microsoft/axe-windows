// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Interop.UIAutomationCore;
using System;
using System.Globalization;

namespace Axe.Windows.Desktop.UIAutomation.CustomObjects.Converters
{
    class IntTypeConverter : ITypeConverter
    {
        public UIAutomationType UnderlyingUiaType => UIAutomationType.UIAutomationType_Int;

        public string Render(dynamic value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return ((int)value).ToString(CultureInfo.InvariantCulture);
        }
    }
}
