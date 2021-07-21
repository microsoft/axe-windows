// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Axe.Windows.Core.CustomObjects
{
    public class CustomProperty
    {
#pragma warning disable CA1720 // Identifier contains type name: name from JSON
        /// <summary>The RFC4122 globally unique identifier of this property.</summary>
        [JsonProperty("guid")]
        public Guid Guid { get; set; }
#pragma warning restore CA1720 // Identifier contains type name: name from JSON

        /// <summary>A textual description of this property.</summary>
        [JsonProperty("programmaticName")]
        public string ProgrammaticName { get; set; }

        ///  <summary>An internal representation of this property's type. For performance reasons, calling code should always use this property in place of the config representation.s</summary>
        [JsonIgnore]
        public CustomUIAPropertyType Type { get; private set; }

        public const string StringConfigType = "string";
        public const string IntConfigType = "int";
        public const string BoolConfigType = "bool";
        public const string DoubleConfigType = "double";
        public const string PointConfigType = "point";
        public const string ElementConfigType = "element";
        public const string EnumConfigType = "enum";

        private string _configType;
        /// <summary>The data type of this property's value as specified in user configuration, one of string, int, bool, double, point, element, or enum.</summary>
        [JsonProperty("uiaType")]
        public string ConfigType
        {
            get { return _configType; }
            set
            {
                switch (value)
                {
                    case StringConfigType:
                        Type = CustomUIAPropertyType.String;
                        break;
                    case IntConfigType:
                        Type = CustomUIAPropertyType.Int;
                        break;
                    case BoolConfigType:
                        Type = CustomUIAPropertyType.Bool;
                        break;
                    case DoubleConfigType:
                        Type = CustomUIAPropertyType.Double;
                        break;
                    case PointConfigType:
                        Type = CustomUIAPropertyType.Point;
                        break;
                    case ElementConfigType:
                        Type = CustomUIAPropertyType.Element;
                        break;
                    case EnumConfigType:
                        Type = CustomUIAPropertyType.Enum;
                        break;
                    default:
                        throw new ArgumentException($"'${value}' is not a supported type", nameof(value));
                }
                _configType = value;
            }
        }

        /// <summary>On enum types, a user-specified mapping of enumeration members to friendly descriptions.</summary>
        [JsonProperty("values")]
#pragma warning disable CA2227 // Collection properties should be read only: setter needed for deserialization
        public Dictionary<int, string> Values { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only: setter needed for deserialization

        ///<summary>Checks that a property is structurally well-formed. For instance, verifies that Values is unset on non-enum types.</summary>
        public void Validate()
        {
            if (Values == null && Type == CustomUIAPropertyType.Enum) 
                throw new InvalidDataException("Values required for enumeration types.");
            if (Values != null && Type != CustomUIAPropertyType.Enum) 
                throw new InvalidDataException("Values cannot be defined for non-enumeration types.");
        }
    }
}
