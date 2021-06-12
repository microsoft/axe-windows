// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using System;

namespace Axe.Windows.Core.CustomObjects.Converters
{
    class ElementTypeConverter : ITypeConverter
    {
        public string Render(dynamic value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return ((A11yElement)value).Glimpse;
        }
    }
}
