// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation.Resources;
using System;
using System.Collections.Generic;

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
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (scanTools == null) throw new ArgumentNullException(nameof(scanTools));
            if (scanTools.OutputFileHelper == null) throw new ArgumentException(ErrorMessages.ScanToolsOutputFileHelperNull, nameof(scanTools));

            _config = config;
            _scanTools = scanTools;
        }

        /// <summary>
        /// See <see cref="IScanner.Scan()"/>
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<ScanResults> Scan()
        {
            return ExecuteScan(null);
        }

        /// <summary>
        /// See <see cref="IScanner.Scan(string)"/>
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<ScanResults> Scan(string scanId)
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
