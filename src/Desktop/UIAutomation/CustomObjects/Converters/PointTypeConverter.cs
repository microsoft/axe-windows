// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Axe.Windows.Desktop.UIAutomation.CustomObjects.Converters
{
    class PointTypeConverter : ITypeConverter
    {
        public string Render(dynamic value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            double[] arr = (double[])value;
            return $"[x={arr[0]},y={arr[1]}]";
        }
    }
}
