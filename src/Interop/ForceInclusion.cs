// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Interop.UIAutomationCore;
using System;

namespace Axe.Windows.Interop
{
    static class ForceInclusion
    {
        public static Type DummyGetType()
        {
            return new CUIAutomationRegistrar().GetType();
        }

        public static void Main() =>
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            Console.WriteLine("This project exists to help us consume Interop.UIAutomationCore.dll.");
#pragma warning restore CA1303 // Do not pass literals as localized parameters

    }
}
