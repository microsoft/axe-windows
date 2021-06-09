// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Interop.UIAutomationCore;
using System;

namespace Axe.Windows.Desktop.UIAutomation.CustomObjects.Converters
{
    class StringTypeConverter : ITypeConverter
    {
        public UIAutomationType UnderlyingUiaType => UIAutomationType.UIAutomationType_OutString;

        public string Render(dynamic value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return ((string)value);
        }
    }
}
