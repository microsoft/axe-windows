// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions.Attributes;
using Axe.Windows.Actions.Enums;
using Axe.Windows.Core.Bases;
using Axe.Windows.Desktop.Settings;
using Axe.Windows.RuleSelection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Packaging;
using System.Text;

namespace Axe.Windows.Actions
{
    /// <summary>
    /// Action for saving snapshot data
    /// </summary>
    [InteractionLevel(UxInteractionLevel.NoUxInteraction)]
    public static class SaveAction
    {
        /// <summary>
        /// Names for files inside of zipped snapshot
        /// </summary>
        public const string elementFileName = "el.snapshot";
        public const string screenshotFileName = "scshot.png";
        public const string metadataFileName = "metadata.json";

        /// <summary>
        /// Buffer size for stream copying
        /// </summary>
        const int buffSize = 0x1000;

        /// <summary>
        /// Save snapshot zip
        /// </summary>
        /// <param name="ecId">ElementContext Id</param>
        /// <param name="path">The output file</param>
        /// <param name="focusedElementId">The ID of the element with the current focus</param>
        /// <param name="mode">The type of file being saved</param>
        public static void SaveSnapshotZip(string path, Guid ecId, int? focusedElementId, A11yFileMode mode)
        {
            var ec = DataManager.GetDefaultInstance().GetElementContext(ecId);

            using (FileStream str = File.Open(path, FileMode.Create))
            using (Package package = ZipPackage.Open(str, FileMode.Create))
            {
                SaveSnapshotFromElement(focusedElementId, mode, ec, package, ec.DataContext.RootElment);
            }
        }

        /// <summary>
        /// Private helper function (formerly in SaveSnapshotZip) to make it easier to call with different inputs
        /// </summary>
        private static void SaveSnapshotFromElement(int? focusedElementId, A11yFileMode mode, Contexts.ElementContext ec, Package package, A11yElement root)
        {
            var json = JsonConvert.SerializeObject(root, Formatting.Indented);
            using (MemoryStream mStrm = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                AddStream(package, mStrm, elementFileName);
            }

            if (ec.DataContext.Screenshot != null)
            {
                using (MemoryStream mStrm = new MemoryStream())
                {
                    ec.DataContext.Screenshot.Save(mStrm, System.Drawing.Imaging.ImageFormat.Png);
                    mStrm.Seek(0, SeekOrigin.Begin);

                    AddStream(package, mStrm, screenshotFileName);
                }
            }

            var meta = new SnapshotMetaInfo(mode, RuleRunner.RuleVersion, focusedElementId, ec.DataContext.ScreenshotElementId);
            var jsonMeta = JsonConvert.SerializeObject(meta, Formatting.Indented);
            using (MemoryStream mStrm = new MemoryStream(Encoding.UTF8.GetBytes(jsonMeta)))
            {
                AddStream(package, mStrm, metadataFileName);
            }
        }

        /// <summary>
        /// Add stream to package
        /// </summary>
        /// <param name="package"></param>
        /// <param name="stream"></param>
        /// <param name="Name"></param>
        private static void AddStream(Package package, Stream stream, string Name)
        {
            var partUri = PackUriHelper.CreatePartUri(new Uri(Name, UriKind.Relative));
            var part = package.CreatePart(partUri, "", CompressionOption.Normal);

            CopyStream(stream, part.GetStream());
        }

        /// <summary>
        /// Copy stream to target stream
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        private static void CopyStream(Stream source, Stream target)
        {
            byte[] buf = new byte[buffSize];
            int bytesRead = 0;

            while ((bytesRead = source.Read(buf, 0, buffSize)) > 0)
            {
                target.Write(buf, 0, bytesRead);
            }
        }
    }
}
