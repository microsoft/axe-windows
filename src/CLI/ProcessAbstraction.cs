// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace AxeWindowsCLI
{
    internal class ProcessAbstraction : IProcessAbstraction
    {
        public Process GetProcessById(int id)
        {
            return Process.GetProcessById(id);
        }

        public Process[] GetProcessesByName(string name)
        {
            return Process.GetProcessesByName(name);
        }
    }
}
