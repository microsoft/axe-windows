﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Abstractions;
using System;
using System.IO;

namespace Axe.Windows.Concretions
{
    internal class SystemIODirectory : ISystemIODirectory
    {
        public DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }

        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }
    } // class
} // namespace
