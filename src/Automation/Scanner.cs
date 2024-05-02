// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation.Data;
using Axe.Windows.Automation.Resources;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Implementation of the <see cref="IScanner"/> interface
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
            HandleScanOptions(scanOptions);
            return SnapshotCommand.ExecuteAsync(_config, _scanTools, cancellationToken);
        }

        public ScanOutput Scan(ScanOptions scanOptions)
        {
            HandleScanOptions(scanOptions);
            return SnapshotCommand.Execute(_config, _scanTools);
        }

        private void HandleScanOptions(ScanOptions scanOptions)
        {
            scanOptions = scanOptions ?? DefaultScanOptions;
            _scanTools.OutputFileHelper.SetScanId(scanOptions.ScanId);
            _scanTools.TargetElementLocator.SetRootWindowHandle(scanOptions.ScanRootWindowHandle);
        }
    } // class
} // namespace
