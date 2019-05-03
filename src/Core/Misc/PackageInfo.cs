// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Axe.Windows.Core.Misc
{
    /// <summary>
    /// Provides information about the Axe.Windows package
    /// </summary>
    public static class PackageInfo
    {
        private static Lazy<Assembly> ThisAssembly = new Lazy<Assembly>(() => Assembly.GetExecutingAssembly(), true);
        private static Lazy<string> LazyInformationalVersion = new Lazy<string>(GetInformationalVersion, true);

        private static String GetInformationalVersion()
        {
            var attribute = ThisAssembly.Value.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            return attribute?.InformationalVersion;
        }

        /// <summary>
        /// Version string with suffix (e.g., "-prerelease") if the suffix exists
        /// </summary>
        public static string InformationalVersion => LazyInformationalVersion.Value;
    } // class
} // namespace
