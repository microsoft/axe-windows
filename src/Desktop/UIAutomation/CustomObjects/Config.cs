// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using System;
using System.IO;

namespace Axe.Windows.Desktop.UIAutomation.CustomObjects
{
    class Config
    {
        [JsonProperty("properties")]
        public CustomProperty[] Properties { get; set; }

        public static Config FromText(string text)
        {
            Config conf = JsonConvert.DeserializeObject<Config>(text);
            conf.Validate();
            // We made it here, so config must be valid.
            return conf;
        }

        public static Config FromFile(string path) { return Config.FromText(File.ReadAllText(path)); }

        private void Validate()
        {
            if (this.Properties == null || this.Properties.Length < 1) throw new ArgumentException("Empty or missing definition of custom properties.");
            foreach (CustomProperty p in this.Properties) p.Validate();
        }
    }
}
