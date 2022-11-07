// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

using static System.FormattableString;

namespace Axe.Windows.Desktop.Utility
{
    public class ProcessItem
    {
        public IntPtr HWnd { get; }
        public int ProcessID { get; }
        public string MainWindowTitle { get; }

        public ProcessItem(Process p)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));

            HWnd = p.MainWindowHandle;
            ProcessID = p.Id;
            MainWindowTitle = p.MainWindowTitle;
        }

        public override string ToString()
        {
            return Invariant($"{ProcessID}:{MainWindowTitle}");
        }
    }
}
