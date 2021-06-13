// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.CustomObjects.Converters;
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

        private string _DataType;
        /// <summary>The data type of this property's value as specified by the user, one of string, int, bool, double, point, or element.</summary>
        [JsonProperty("uiaType")]
        public string DataType
        {
            get { return _DataType; }
            set
            {
                switch (value)
                {
                    case "string":
                    case "int":
                    case "bool":
                    case "double":
                    case "point":
                    case "element":
                        _DataType = value;
                        break;
                    default:
                        throw new InvalidDataException("Type in custom property definition is missing or invalid.");
                }
            }
        }
    }
}
