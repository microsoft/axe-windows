// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions.Attributes;
using Axe.Windows.Actions.Enums;
using Axe.Windows.Core.Bases;
using Axe.Windows.Desktop.Drawing;
using Axe.Windows.Desktop.Settings;
using Axe.Windows.SystemAbstractions;
using Axe.Windows.Telemetry;
using Newtonsoft.Json;
using System;
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
        internal static IBitmapCreator BitmapCreator = new FrameworkBitmapCreator();

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

                IBitmap bmp;
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

                var selectedElement = element.FindDescendant(k => k.UniqueId == meta.ScreenshotElementId);

#pragma warning disable CA2000 // Dispose objects before losing scope
                return new LoadActionParts(element, bmp, BitmapCreator.FromSystemDrawingBitmap(selectedElement.SynthesizeBitmapFromElements()), meta);
#pragma warning restore CA2000 // Dispose objects before losing scope
            }
        }

        /// <summary>
        /// Load bmp from stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static IBitmap LoadBmp(Stream stream)
        {
            Image img = Image.FromStream(stream);
#pragma warning disable CA2000 // Dispose objects before losing scope
            return BitmapCreator.FromSystemDrawingBitmap(new Bitmap(img));
#pragma warning restore CA2000 // Dispose objects before losing scope
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
    }
}
