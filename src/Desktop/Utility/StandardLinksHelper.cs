// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Results;
using Axe.Windows.Core.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Axe.Windows.Desktop.Utility
{
    /// <summary>
    /// Class to access stored link information
    /// </summary>
    public class StandardLinksHelper
    {
        /// <summary>
        /// Dictionary of stored links
        /// </summary>
        private readonly IReadOnlyDictionary<string, string> _storedLinks;

        /// <summary>
        /// Constructor
        /// </summary>
        StandardLinksHelper()
        {
            // get the path of dictionary file.
            var json = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "links.json"));
            _storedLinks = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        /// <summary>
        /// Is there a stored link for the given scan
        /// </summary>
        /// <param name="mi"></param>
        /// <returns></returns>
        public bool HasStoredLink(ScanMetaInfo mi)
        {
            if (mi == null) throw new ArgumentNullException(nameof(mi));

            return _storedLinks.ContainsKey($"{mi.UIFramework}-{mi.ControlType}-{PropertyType.GetInstance().GetNameById(mi.PropertyId)}");
        }

#pragma warning disable CA1055 // Uri return values should not be strings
        /// <summary>
        /// Get CELA Snippet Query Url
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public string GetSnippetQueryUrl(ScanMetaInfo mi)
#pragma warning restore CA1055 // Uri return values should not be strings
        {
            if (mi == null) throw new ArgumentNullException(nameof(mi));

            return _storedLinks[$"{mi.UIFramework}-{mi.ControlType}-{PropertyType.GetInstance().GetNameById(mi.PropertyId)}"];
        }

        #region static members
        /// <summary>
        /// Default link helper
        /// </summary>
        static StandardLinksHelper DefaultInstance;

#pragma warning disable CA1024 // Use properties where appropriate
        /// <summary>
        /// Get the default instance of StandardLinksHelper
        /// </summary>
        /// <returns></returns>
        public static StandardLinksHelper GetDefaultInstance()
        {
            if (DefaultInstance == null)
            {
                DefaultInstance = new StandardLinksHelper();
            }

            return DefaultInstance;
        }
#pragma warning restore CA1024 // Use properties where appropriate
        #endregion
    }
}
