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
        private Guid _Guid;
        /// <summary>The RFC4122 globally unique identifier of this property.</summary>
        [JsonProperty("guid")]
        public Guid Guid {
            get { return _Guid; }
            set
            {
                if (value == Guid.Empty) throw new InvalidDataException("Missing GUID in custom property definition.");
                _Guid = value;
            }
        }
#pragma warning restore CA1720 // Identifier contains type name: name from JSON

        private string _ProgrammaticName;
        /// <summary>A textual description of this property.</summary>
        [JsonProperty("programmaticName")]
        public string ProgrammaticName {
            get { return _ProgrammaticName; }
            set
            {
                if (value == null) throw new InvalidDataException("Missing programmatic name in custom property definition.");
                _ProgrammaticName = value;
            }
        }

        ///  <summary>An internal representation of this property's type. For performance reasons, calling code should always use this property in place of the config representation.s</summary>
        public CustomUIAPropertyType Type { get; private set; }

        private string _ConfigType;
        /// <summary>The data type of this property's value as specified in user configuration, one of string, int, bool, double, point, or element.</summary>
        [JsonProperty("uiaType")]
        public string ConfigType
        {
            get { return _ConfigType; }
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
                        throw new InvalidDataException("Type in custom property definition is missing or invalid.");
                }
                _ConfigType = value;
            }
        }
    }
}
