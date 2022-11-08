// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using System;

namespace Axe.Windows.CI
{
    class Program
    {
        /// <summary>
        /// Entry point for dummy app
        /// </summary>
        static void Main()
        {
            if (Config.Builder.ForProcessId(0) == null)
            {
                Console.WriteLine("This will never be written");
            }
            Console.WriteLine("This is just a placeholder app to gather the Axe.Windows assemblies.");
        }
    }
}
