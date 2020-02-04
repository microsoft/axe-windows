// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics;

namespace OldFileVersionCompatibilityTests
{
    class TestAssertionException : Exception
    {
        public string FileName { get; }
        public int LineNumber { get; }

        public TestAssertionException(string message)
            : base(message)
        {
            var trace = new StackTrace(true);
            var frame = trace.GetFrame(2);
            if (frame == null) throw new ProgramException("Unable to get stack frame for assertion");

            FileName = frame.GetFileName();
            LineNumber = frame.GetFileLineNumber();
        }
    } // class
} // namespace
