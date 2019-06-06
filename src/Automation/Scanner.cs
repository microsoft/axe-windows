// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Implementation of <see cref="IScanner"/>
    /// </summary>
    class Scanner : IScanner
    {
        private readonly Config _config;

        internal Scanner(Config config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            _config = config;
        }

        /// <summary>
        /// See <see cref="IScanner.Scan"/>
        /// </summary>
        /// <returns></returns>
        public ScanResults Scan()
        {
            throw new NotImplementedException();
        }
    } // class
} // namespace
