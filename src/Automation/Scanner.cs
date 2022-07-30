// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Implementation of <see cref="IScanner"/>
    /// </summary>
    class Scanner : IScanner
    {
        private readonly Config _config;
        private readonly IScanTools _scanTools;

        internal Scanner(Config config, IScanTools scanTools)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _scanTools = scanTools ?? throw new ArgumentNullException(nameof(scanTools));

            if (scanTools.OutputFileHelper == null) throw new ArgumentException(ErrorMessages.ScanToolsOutputFileHelperNull, nameof(scanTools));
        }

        /// <summary>
        /// See <see cref="IScanner.Scan()"/>
        /// </summary>
        /// <returns></returns>
        public ScanResults Scan()
        {
            if (_config.AreMultipleScanRootsEnabled)
            {
                throw new InvalidOperationException("Multiple scan roots are not supported when calling Scan(). Use ScanAll() instead.");
            }
            return ExecuteScan(null).First();
        }

        /// <summary>
        /// See <see cref="IScanner.ScanAll()"/>
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<ScanResults> ScanAll()
        {
            return ExecuteScan(null);
        }

        /// <summary>
        /// See <see cref="IScanner.Scan(string)"/>
        /// </summary>
        /// <returns></returns>
        public ScanResults Scan(string scanId)
        {
            if (_config.AreMultipleScanRootsEnabled)
            {
                throw new InvalidOperationException("Multiple scan roots are not supported when calling Scan(). Use ScanAll() instead.");
            }
            return ExecuteScan(scanId).First();
        }

        /// <summary>
        /// See <see cref="IScanner.ScanAll(string)"/>
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<ScanResults> ScanAll(string scanId)
        {
            return ExecuteScan(scanId);
        }

        private IReadOnlyCollection<ScanResults> ExecuteScan(string scanId)
        {
            return ExecutionWrapper.ExecuteCommand(() =>
            {
                _scanTools.OutputFileHelper.SetScanId(scanId);

                return SnapshotCommand.Execute(_config, _scanTools);
            });
        }
    } // class
} // namespace
