// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Automation
{
#pragma warning disable CA1815 // Should override Equals
    /// <summary>
    /// Represents the output file(s), if any, associated with a ScanResults object
    /// </summary>
    public struct OutputFile
    {
        /// <summary>
        /// The A11yTest file that was generated (or null if no file was generated)
        /// </summary>
        public string A11yTest { get; }

        /// <summary>
        /// The Sarif file that was generated (or null if no file was generated)
        /// </summary>
        public string Sarif { get; }

        /// <summary>
        /// Private ctor. Called by the BuildFrom* methods
        /// </summary>
        /// <param name="a11yTest">The A11yTest file that was created (if any)</param>
        /// <param name="sarif">The Sarif file that was created (if any)</param>
        private OutputFile(string a11yTest = null, string sarif = null)
        {
            A11yTest = a11yTest;
            Sarif = sarif;
        }

        /// <summary>
        /// Build an OutputFile containing just an A11yTest file
        /// </summary>
        /// <param name="a11yTestFile"></param>
        /// <returns></returns>
        internal static OutputFile BuildFromA11yTestFile(string a11yTestFile)
        {
            return new OutputFile(a11yTest: a11yTestFile);
        }
    }
#pragma warning restore CA1815
}
