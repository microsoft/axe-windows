// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Interop.UIAutomationCore;
using System;

namespace InteropDummy
{
    class Program
    {
        static int Main(string[] _)
        {
            var fakeType = new CUIAutomationRegistrar().GetType();

#pragma warning disable CA1303 // Do not pass literals as localized parameters
            Console.WriteLine("This program exists to import Interop.UIAutomationCore.Signed.dll");
            Console.WriteLine("in a way that makes it easy to handle the licensing.");
#pragma warning restore CA1303 // Do not pass literals as localized parameters

            return (fakeType == typeof(int)) ? 1 : 0;
        }
    }
}
