// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.IO;

namespace Axe.Windows.SystemAbstractions
{
    internal interface ISystemIODirectory
    {
        DirectoryInfo CreateDirectory(string path);
        bool Exists(string path);
    } // interface
} // namespace
