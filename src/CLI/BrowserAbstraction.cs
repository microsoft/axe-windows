// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace AxeWindowsCLI
{
    internal class BrowserAbstraction : IBrowserAbstraction
    {
        public void Open(string pathToFile)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c \"" + pathToFile + "\"")
            {
                CreateNoWindow = true
            });
        }
    }
}
