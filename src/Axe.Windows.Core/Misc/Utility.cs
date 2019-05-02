// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Telemetry;
using System;
using System.Diagnostics;

namespace Axe.Windows.Core.Misc
{
    public static class Utility
    {
        public static string GetProcessName(int processId)
        {
            if (!TryGetProcessById(processId, out Process process)) return null;

            return process?.ProcessName;
        }

        private static bool TryGetProcessById(int processId, out Process process)
        {
            process = null;

            if (processId == 0) return false;

            try
            {
                process = Process.GetProcessById(processId);
            }
            catch (ArgumentException e)
            {
                e.ReportException();
                // occurs when an invalid process id is passed to GetProcessById
                return false;
            }

            return true;
        }
    }
}
