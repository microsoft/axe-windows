// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Enums;
using Newtonsoft.Json;
using System;
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
        public CustomUIAPropertyType Type { get; private set; }

        private string _configType;
        /// <summary>The data type of this property's value as specified in user configuration, one of string, int, bool, double, point, or element.</summary>
        [JsonProperty("uiaType")]
        public string ConfigType
        {
            get { return _configType; }
            set
            {
                switch (value)
                {
                    case "string":
                        Type = CustomUIAPropertyType.String;
                        break;
                    case "int":
                        Type = CustomUIAPropertyType.Int;
                        break;
                    case "bool":
                        Type = CustomUIAPropertyType.Bool;
                        break;
                    case "double":
                        Type = CustomUIAPropertyType.Double;
                        break;
                    case "point":
                        Type = CustomUIAPropertyType.Point;
                        break;
                    case "element":
                        Type = CustomUIAPropertyType.Element;
                        break;
                    default:
                        throw new ArgumentException($"'${value}' is not a supported type", nameof(value));
                }
                _configType = value;
            }
        }
    }
}
