// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using System.Linq;

namespace AxeWindowsScanner
{
    public static class ReturnValueChooser
    {
        public const int ScanCompletedAndFoundNoErrors = 0;
        public const int ScanCompletedAndFoundErrors = 1;
        public const int ScanFailedToComplete = 2;
        public const int BadInputParameters = 255;

        public static int GetReturnValue(IErrorCollector errorCollector, ScanResults scanResults)
        {
            if (errorCollector.ParameterErrors.Any())
                return BadInputParameters;

            if (errorCollector.Exceptions.Any() || scanResults == null)
                return ScanFailedToComplete;

            if (scanResults.Errors.Any())
                return ScanCompletedAndFoundErrors;

            return ScanCompletedAndFoundNoErrors;
        }
    }
}
