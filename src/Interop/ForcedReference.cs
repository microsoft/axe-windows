// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Interop.UIAutomationCore;
using System;

namespace Axe.Windows.Interop
{
    /// <summary>
    /// This class exists purely to provide a vehicle to allow the Desktop project to know
    /// exactly where to find Interop.UIAutomationCore.dll for embedding.
    /// </summary>
    static public class ForcedReference
    {
        /// <summary>
        /// This method never actually gets called by our code. It's here to force inclusion of
        /// Interop.UIAutomationCore.dll.
        /// </summary>
        static public Type ForceReference()
        {
            var r = new CUIAutomationRegistrar();
            return r.GetType();
        }
    }
}
