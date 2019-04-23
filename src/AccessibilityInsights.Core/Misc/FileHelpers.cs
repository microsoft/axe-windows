﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

using static System.FormattableString;

namespace Axe.Windows.Core.Misc
{
    /// <summary>
    /// File serialization and deserialization utilities
    /// </summary>
    public static class FileHelpers
    {
        /// <summary>
        /// Serialize data to JSON format at the specified path
        /// </summary>
        /// <param name="data">The data being serialized</param>
        /// <param name="path">Config file location and name</param>
        public static void SerializeDataToJSON(object data, string path)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, json, Encoding.UTF8);
        }

        /// <summary>
        /// Load JSON serialized data from the specified path
        /// </summary>
        /// <typeparam name="T">Configuration type</typeparam>
        /// <param name="path">Config file location and name</param>
        /// <returns>The serialized data if successful, default(T) if not</returns>
        public static T LoadDataFromJSON<T>(string path)
        {
            T data = default(T);

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                data = JsonConvert.DeserializeObject<T>(json);
            }

            return data;
        }

        /// <summary>
        /// Rename the existing configuration to .bak file.
        /// </summary>
        /// <param name="path">The file to rename</param>
        /// <returns>true if the file was found and renamed</returns>
        public static bool RenameFileAsBackup(string path)
        {
            if (File.Exists(path))
            {
                File.Move(path, Invariant($"{path}.{DateTime.Now.Ticks}.bak"));
                return true;
            }

            return false;
        }
    }
}
