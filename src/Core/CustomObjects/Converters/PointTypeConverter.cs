// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Resources;
using System;
using System.Globalization;

namespace Axe.Windows.Core.CustomObjects.Converters
{
    public class PointTypeConverter : ITypeConverter
    {
        public string Render(dynamic value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            double[] arr = (double[])value;
            return String.Format(CultureInfo.CurrentCulture, DisplayStrings.PointFormat, arr[0], arr[1]);
        }
    }
}
