﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Security.Cryptography.X509Certificates;

namespace OldFileVersionCompatibilityTests
{
    class ProgramException : Exception
    {
        public ProgramException(string message)
            : base(message)
        { }
    } // class
} // namespace
