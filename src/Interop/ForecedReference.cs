// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Interop.UIAutomationCore;
using System;

namespace Axe.Windows.Interop
{
    static public class ForecedReference
    {
        static public Type ForceReference()
        {
            var r = new CUIAutomationRegistrar();
            return r.GetType();
        }
    }
}
