// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using System.IO;

namespace Axe.Windows.Core.CustomObjects
{
    public class Config
    {
#pragma warning disable CA1819 // Properties should not return arrays: represents a JSON collection
        [JsonProperty("properties")]
        public CustomProperty[] Properties { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays: represents a JSON collection

        public static Config ReadFromText(string text) { return JsonConvert.DeserializeObject<Config>(text); }

        public static Config ReadFromFile(string path) { return Config.ReadFromText(File.ReadAllText(path)); }
    }
}
