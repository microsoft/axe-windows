// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Axe.Windows.Win32
{
    /// <summary>
    /// Win32-related constants. Some of these definitions originated from https://pinvoke.net/
    /// </summary>
    internal static class Win32Constants
    {
        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;

#pragma warning disable IDE1006 // Naming Styles
        public const int S_OK = 0;
#pragma warning restore IDE1006 // Naming Styles
    }
}
