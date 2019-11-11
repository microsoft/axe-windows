// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.SystemAbstractions;
using System;

namespace Axe.Windows.Win32
{
    /// <summary>
    /// Win32 related helper methods.  
    /// </summary>
    internal class Win32Helper
    {
        private readonly IMicrosoftWin32Registry _registry;

        /// <summary>
        /// Windows 10 version number
        /// the value is based on the value in @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion", "CurrentVersion"
        /// </summary>
        static readonly Version Win10Version = new Version(6, 3);
        const string WindowsVersionRegKey = @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion";

        public Win32Helper(IMicrosoftWin32Registry registry)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            _registry = registry;
        }

        public Win32Helper()
            : this(MicrosoftFactory.CreateMicrosoft().Win32.Registry)
        { }

        /// <summary>
        /// Check whether the current Windows is Windows 7 or not. 
        /// </summary>
        /// <returns></returns>
        internal static bool IsWindows7()
        {
            return Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1;
        }

        /// <summary>
        /// Get the current Windows version info from HKLM
        /// </summary>
        /// <returns></returns>
        private string GetCurrentWindowsVersion()
        {
            var retVal = (string)_registry.GetValue(WindowsVersionRegKey, "CurrentVersion", "");
            return retVal;
        }

        /// <summary>
        /// Get the current Windows build info from HKLM
        /// </summary>
        /// <returns></returns>
        private string GetCurrentWindowsBuild()
        {
            return (string)_registry.GetValue(WindowsVersionRegKey, "CurrentBuild", "");
        }

        private OsComparisonResult CompareWindowsVersionToWin10()
        {
            if (Version.TryParse(GetCurrentWindowsVersion(), out Version currentVersion))
            {
                if (currentVersion > Win10Version)
                    return OsComparisonResult.Newer;

                if (currentVersion == Win10Version)
                    return OsComparisonResult.Equal;
            }

            return OsComparisonResult.Older;
        }

        private OsComparisonResult CompareToWindowsBuildNumber(uint minimalBuild)
        {
            if (uint.TryParse(GetCurrentWindowsBuild(), out uint currentBuild))
            {
                if (currentBuild > minimalBuild)
                    return OsComparisonResult.Newer;

                if (currentBuild == minimalBuild)
                    return OsComparisonResult.Equal;
            }

            return OsComparisonResult.Older;
        }

        /// <summary>
        /// Helper function to evaluate builds
        /// </summary>
        /// <param name="minimalBuild">The minimal build needed to pass</param>
        /// <returns>True iff the OS is at least Win10 at the specified build</returns>
        internal bool IsAtLeastWin10WithSpecificBuild(uint minimalBuild)
        {
            OsComparisonResult win10ComparisonResult = CompareWindowsVersionToWin10();

            return win10ComparisonResult == OsComparisonResult.Newer ||
                (win10ComparisonResult == OsComparisonResult.Equal && CompareToWindowsBuildNumber(minimalBuild) != OsComparisonResult.Older);
        }

        /// <summary>
        /// Check whether current OS is Win10 RS3 or later
        /// </summary>
        /// <returns>True iff the OS is at least Win10 RS3</returns>
        internal bool IsWindowsRS3OrLater()
        {
            return IsAtLeastWin10WithSpecificBuild(16228); // Build 16228 is confirmed in the RS3 range
        }

        /// <summary>
        /// Check whether current OS is Win10 RS5 or later
        /// </summary>
        /// <returns>True iff the OS is at least Win10 RS5</returns>
        internal bool IsWindowsRS5OrLater()
        {
            return IsAtLeastWin10WithSpecificBuild(17713); // Build 17713 is confirmed in the RS5 range
        }
    }
}
