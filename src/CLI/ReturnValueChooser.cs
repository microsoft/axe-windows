// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AxeWindowsCLI
{
    internal static class ReturnValueChooser
    {
        public const int ScanCompletedAndFoundNoErrors = 0;
        public const int ScanCompletedAndFoundErrors = 1;
        public const int ScanFailedToComplete = 2;
        public const int ThirdPartyNoticesDisplayed = 3;  // Used outside this class
        public const int BadInputParameters = 255;

        public static int GetReturnValue(bool parserError, IReadOnlyCollection<ScanResults> scanResults, Exception caughtException)
        {
            if (parserError || caughtException is ParameterException)
                return BadInputParameters;

#pragma warning disable CA1508
            if (caughtException != null || scanResults == null || scanResults.Any(result => result == null))
                return ScanFailedToComplete;
#pragma warning restore CA1508

            if (scanResults.Any(result => result.Errors.Any()))
                return ScanCompletedAndFoundErrors;

            return ScanCompletedAndFoundNoErrors;
        }
    }
}
