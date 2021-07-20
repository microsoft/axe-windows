// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Axe.Windows.Core.CustomObjects.Converters
{
    public class EnumTypeConverter : ITypeConverter
    {
        /// <summary>A user-specified mapping of enumeration members to friendly descriptions.</summary>
        private IReadOnlyDictionary<int, string> _values { get; }

        public EnumTypeConverter(IReadOnlyDictionary<int, string> values) { _values = values; }

        public string Render(dynamic value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            int raw = (int)value;
            if (_values.TryGetValue(raw, out string friendlyName))
                return $"{friendlyName} ({raw})";
            return $"Unknown ({raw})";
        }
    }
}
