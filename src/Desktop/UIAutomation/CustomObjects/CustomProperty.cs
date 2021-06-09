// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using System;

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

        /// <summary>The dynamic ID assigned to this property by the system.</summary>
        [JsonIgnore]
        public int DynamicId { get; set; }

        internal void Validate()
        {
            if (Guid == Guid.Empty) throw new ArgumentException("Missing GUID in custom property definition.");
            if (ProgrammaticName == null) throw new ArgumentException("Missing programmatic name in custom property definition.");
            if (DataType == null) throw new ArgumentException("Missing type in custom property definition.");
            ValidateType();
        }

        internal void ValidateType()
        {
            switch (DataType)
            {
                case "string":
                    // Valid type, further processing in a subsequent PR.
                    break;
                case "int":
                    // Valid type, further processing in a subsequent PR.
                    break;
                case "bool":
                    // Valid type, further processing in a subsequent PR.
                    break;
                case "double":
                    // Valid type, further processing in a subsequent PR.
                    break;
                case "point":
                    // Valid type, further processing in a subsequent PR.
                    break;
                case "element":
                    // Valid type, further processing in a subsequent PR.
                    break;
                default:
                    throw new ArgumentException($"Unknown type {this.DataType}.");
            }
        }
    }
}
