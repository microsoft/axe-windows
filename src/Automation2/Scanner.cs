// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Automation2
{
    /// <summary>
    /// Implementation of IScanner
    /// </summary>
    public class Scanner : IScanner
    {
        private readonly Config _config;

        private Scanner(Config config)
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

        /// <summary>
        /// Build a Scanner object
        /// </summary>
        public class Builder
        {
            private readonly Scanner _scanner;

            private Builder(Config config)
            {
                _scanner = new Scanner(config);
            }

            /// <summary>
            /// Specify configuration options for the scan
            /// </summary>
            /// <param name="config"></param>
            /// <returns></returns>
            public static Builder WithConfig(Config config)
            {
                return new Builder(config);
            }

            /// <summary>
            /// Create an instance of the <see cref="Scanner"/> class
            /// </summary>
            /// <returns></returns>
            public IScanner Build()
            {
                throw new NotImplementedException();
            }
        } // Builder
    } // class
} // namespace
