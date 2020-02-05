// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using System;
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

        public static int GetReturnValue(ScanResults scanResults, Exception caughtException)
        {
            if (caughtException as ParameterException != null)
                return BadInputParameters;

            if (caughtException != null || scanResults == null)
                return ScanFailedToComplete;

            if (scanResults.Errors.Any())
                return ScanCompletedAndFoundErrors;

            return ScanCompletedAndFoundNoErrors;
        }
    }
}
