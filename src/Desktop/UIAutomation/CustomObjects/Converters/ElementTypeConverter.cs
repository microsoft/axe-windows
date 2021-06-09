// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Interop.UIAutomationCore;
using System;

namespace Axe.Windows.Desktop.UIAutomation.CustomObjects.Converters
{
    class ElementTypeConverter : ITypeConverter
    {
        public UIAutomationType UnderlyingUiaType => UIAutomationType.UIAutomationType_Element;

        public string Render(dynamic value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return ((A11yElement)value).Glimpse;
        }
    }
}
