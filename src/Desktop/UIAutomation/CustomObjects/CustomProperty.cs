// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.UIAutomation.CustomObjects.Converters;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Axe.Windows.Desktop.UIAutomation.CustomObjects
{
    public class CustomProperty
    {
        /// <summary>The RFC4122 globally unique identifier of this property.</summary>
        [JsonProperty("guid")]
#pragma warning disable CA1720 // Identifier contains type name: name from JSON
        public Guid Guid { get; set; }
#pragma warning restore CA1720 // Identifier contains type name: name from JSON
        /// <summary>A textual description of this property.</summary>
        [JsonProperty("programmaticName")]
        public string ProgrammaticName { get; set; }

        /// <summary>The data type of this property's value as specified by the user, one of string, int, bool, double, point, or element.</summary>
        // TODO Bill: add enum (with values member)
        [JsonProperty("uiaType")]
        public string DataType { get; set; }

        /// <summary>A type converter for this property, representing its underlying UIA type and providing string rendering.</summary>
        [JsonIgnore]
        public ITypeConverter TypeConverter { get; private set; }

        /// <summary>The dynamic ID assigned to this property by the system.</summary>
        [JsonIgnore]
        public int DynamicId { get; set; }

        internal void Validate()
        {
            if (Guid == Guid.Empty) throw new InvalidDataException("Missing GUID in custom property definition.");
            if (ProgrammaticName == null) throw new InvalidDataException("Missing programmatic name in custom property definition.");
            if (DataType == null) throw new InvalidDataException("Missing type in custom property definition.");
            TypeConverter = CreateTypeConverter();
        }

        internal ITypeConverter CreateTypeConverter()
        {
            switch (DataType)
            {
                case "string":
                    return new StringTypeConverter();
                case "int":
                    return new IntTypeConverter();
                case "bool":
                    return new BoolTypeConverter();
                case "double":
                    return new DoubleTypeConverter();
                case "point":
                    return new PointTypeConverter();
                case "element":
                    return new ElementTypeConverter();
                default:
                    throw new InvalidDataException($"Unknown type {DataType}.");
            }
        }
    }
}
