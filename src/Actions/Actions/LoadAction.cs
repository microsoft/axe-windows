// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions.Attributes;
using Axe.Windows.Actions.Enums;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.CustomObjects;
using Axe.Windows.Desktop.Settings;
using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using Axe.Windows.Telemetry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Packaging;
using System.Linq;

namespace Axe.Windows.Actions
{
    /// <summary>
    /// Action to load snapshot file
    /// </summary>
    [InteractionLevel(UxInteractionLevel.NoUxInteraction)]
    public static class LoadAction
    {
        /// <summary>
        /// Extract snapshot file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static LoadActionParts LoadSnapshotZip(string path)
        {
            using (FileStream str = File.Open(path, FileMode.Open, FileAccess.Read))
            using (Package package = ZipPackage.Open(str, FileMode.Open, FileAccess.Read))
            {
                var parts = package.GetParts();

                var elementPart = (from p in parts where p.Uri.OriginalString == "/" + SaveAction.elementFileName select p.GetStream()).First();
                A11yElement element = A11yElement.FromStream(elementPart);
                elementPart.Close();

                Bitmap bmp;
                try
                {
                    var bmpPart = (from p in parts where p.Uri.OriginalString == "/" + SaveAction.screenshotFileName select p.GetStream()).First();
                    bmp = LoadBmp(bmpPart);
                    bmpPart.Close();
                }
                catch (InvalidOperationException e)  // Gets thrown if screenshot doesn't exist in file
                {
                    e.ReportException();
                    bmp = null;
                }

                var metadataPart = (from p in parts where p.Uri.OriginalString == "/" + SaveAction.metadataFileName select p.GetStream()).First();
                SnapshotMetaInfo meta = SnapshotMetaInfo.DeserializeFromStream(metadataPart);
                metadataPart.Close();

                IReadOnlyDictionary<int, CustomProperty> CustomProperties;
                try
                {
                    var customPropertiesPart = (from p in parts where p.Uri.OriginalString == "/" + SaveAction.customPropsFileName select p.GetStream()).First();
                    CustomProperties = LoadCustomProperties(customPropertiesPart);
                    customPropertiesPart.Close();
                }
#pragma warning disable CA1031 // Do not catch general exception types: specific handlers placed in conditional below
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types: specific handlers placed in conditional below
                {
                    if (!(e is InvalidOperationException))  // An expected exception thrown when file does not exist, such as in old a11ytest files
                        e.ReportException();
                    CustomProperties = null;
                }

                var selectedElement = element.FindDescendant(k => k.UniqueId == meta.ScreenshotElementId);

                return new LoadActionParts(element, bmp, selectedElement.SynthesizeBitmapFromElements(), meta);
            }
        }

        /// <summary>
        /// Load bmp from stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static Bitmap LoadBmp(Stream stream)
        {
            Image img = Image.FromStream(stream);
            return new Bitmap(img);
        }

        /// <summary>
        /// Load JSON stored metadata file from stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static SnapshotMetaInfo LoadMetadata(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                string jsonMeta = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<SnapshotMetaInfo>(jsonMeta);
            }
        }

        /// <summary>
        /// Load JSON stored custom property registrations from stream
        /// </summary>
        private static IReadOnlyDictionary<int, CustomProperty> LoadCustomProperties(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                string jsonProps = reader.ReadToEnd();
                Dictionary<int, CustomProperty> res = JsonConvert.DeserializeObject<Dictionary<int, CustomProperty>>(jsonProps);
                Registrar.GetDefaultInstance().MergeCustomPropertyRegistrations(res);
                return res;
            }
        }
    }
}
