// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Automation
{
    internal class DPIAwareness : IDPIAwareness
    {
        public object Enable()
        {
            global::Windows.Win32.PInvoke.SetProcessDPIAware();
            return null;
        }

        public void Restore(object _)
        {
            // Default restore does nothing
        }
    }
}
