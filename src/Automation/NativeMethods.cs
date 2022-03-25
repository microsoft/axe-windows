// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Automation
{
    class NativeMethods : INativeMethods
    {
        public bool SetProcessDPIAware()
        {
            return Axe.Windows.Win32.NativeMethods.SetProcessDPIAware();
        }
    } // class
} // namespace
