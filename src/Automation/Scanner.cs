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
        private static Object LockObject = new Object();
        private readonly Config _config;
        private readonly IOutputFileHelper _outputFileHelper;
        private readonly IScanResultsAssembler _scanResultsAssembler;

        internal Scanner(Config config, IOutputFileHelper outputFileHelper, ScanResultsAssembler scanResultsAssembler)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (outputFileHelper == null) throw new ArgumentNullException(nameof(outputFileHelper));
            if (scanResultsAssembler == null) throw new ArgumentNullException(nameof(scanResultsAssembler));

            _config = config;
            _outputFileHelper = outputFileHelper;
            _scanResultsAssembler = scanResultsAssembler;
        }

        /// <summary>
        /// See <see cref="IScanner.Scan"/>
        /// </summary>
        /// <returns></returns>
        public ScanResults Scan()
        {
            lock (LockObject)
            {
                return SnapshotCommand.Execute(_config, _outputFileHelper, _scanResultsAssembler);
            } // lock
        }
    } // class
} // namespace
