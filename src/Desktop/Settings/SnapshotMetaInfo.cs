// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Axe.Windows.Desktop.Settings
{
    /// <summary>
    /// Contains metadata for a snapshot file. Created when saving snapshot,
    /// loaded and used when opening snapshot
    /// </summary>
    public class SnapshotMetaInfo
    {
        /// <summary>
        /// Mode to return to after loading
        /// </summary>
        public A11yFileMode Mode { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
#pragma warning disable CA1002 // Do not expose generic lists
        // these properties are serialized/deserialized via json. so can't make it readonly or an IList

        /// <summary>
        /// Selected elements' unique IDs
        /// </summary>
        public List<int> SelectedItems { get; set; }
#pragma warning restore CA1002 // Do not expose generic lists
#pragma warning restore CA2227 // Collection properties should be read only

        public int ScreenshotElementId { get; set; }

        /// <summary>
        /// Rule Version string
        /// </summary>
        public string RuleVersion { get; set; }

        /// <summary>
        /// Version of Axe.Windows.Core assembly
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SnapshotMetaInfo() { }

        /// <summary>
        /// Constructor that takes single ID
        /// </summary>
        /// <param name="mode">The save mode</param>
        /// <param name="ruleVersion">Indicate the version of rule</param>
        /// <param name="selected">The selected item (null if no item is selected)</param>
        /// <param name="screlementId">the ID of an element which was used to grab the screenshot</param>
        public SnapshotMetaInfo(A11yFileMode mode, string ruleVersion, int? selected, int screlementId)
        {
            Mode = mode;
            RuleVersion = ruleVersion;
            Version = Core.Misc.PackageInfo.InformationalVersion;

            if (selected.HasValue)
            {
                SelectedItems = new List<int> { selected.Value };
            }
            ScreenshotElementId = screlementId;
        }

        /// <summary>
        /// Deserializes from the given stream
        ///     Converts any values in OtherProperties dictionary to appropriate types - e.g FirstColor / SecondColor to System.Windows.Media.Color
        /// </summary>
        /// <param name="metadataPart"></param>
        /// <returns></returns>
        public static SnapshotMetaInfo DeserializeFromStream(Stream metadataPart)
        {
            using (StreamReader reader = new StreamReader(metadataPart))
            {
                string jsonMeta = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<SnapshotMetaInfo>(jsonMeta);
            }
        }
    }

    /// <summary>
    /// Modes to return to after loading
    /// </summary>
    public enum A11yFileMode
    {
        Inspect,
        Test,
        Contrast,
    }
}
