// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation.Data;
using Axe.Windows.Automation.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Implementation of <see cref="IScanner"/> (recommended for new projects) and <see cref="IScanner"/>
    /// </summary>
    class Scanner : IScanner
    {
        private readonly Config _config;
        private readonly IScanTools _scanTools;
        private static readonly ScanOptions DefaultScanOptions = new ScanOptions(null);

        internal Scanner(Config config, IScanTools scanTools)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _scanTools = scanTools ?? throw new ArgumentNullException(nameof(scanTools));

            if (scanTools.OutputFileHelper == null) throw new ArgumentException(ErrorMessages.ScanToolsOutputFileHelperNull, nameof(scanTools));
        }

        public Task<ScanOutput> ScanAsync(ScanOptions scanOptions, CancellationToken cancellationToken)
        {
            scanOptions = scanOptions ?? DefaultScanOptions;
            _scanTools.OutputFileHelper.SetScanId(scanOptions.ScanId);
            return SnapshotCommand.ExecuteAsync(_config, _scanTools, cancellationToken);
        }

        public ScanOutput Scan(ScanOptions scanOptions)
        {
            return ScanAsync(scanOptions, CancellationToken.None).GetAwaiter().GetResult();
        }
    } // class
} // namespace
