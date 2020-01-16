// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AxeWindowsScanner
{
    public interface IProcessHelper
    {
        public const int InvalidProcessId = -1;
        public const string InvalidProcessName = null;

        int FindProcessByName(string processName);
        string FindProcessById(int processId);
    }
}
