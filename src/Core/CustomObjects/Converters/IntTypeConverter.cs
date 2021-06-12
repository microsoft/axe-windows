﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

namespace Axe.Windows.Core.CustomObjects.Converters
{
    class IntTypeConverter : ITypeConverter
    {
        public string Render(dynamic value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return ((int)value).ToString(CultureInfo.InvariantCulture);
        }
    }
}
