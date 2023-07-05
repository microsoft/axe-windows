// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using AxeWindowsCLI.Resources;
using System;
using System.Globalization;
using System.IO;

namespace AxeWindowsCLI
{
    internal static class OptionsEvaluator
    {
        public static IOptions ProcessInputs(Options rawInputs, IProcessHelper processHelper)
        {
            if (rawInputs == null) throw new ArgumentNullException(nameof(rawInputs));
            if (processHelper == null) throw new ArgumentNullException(nameof(processHelper));

            int delayInSeconds = rawInputs.DelayInSeconds;

            if (delayInSeconds < 0 || delayInSeconds > 60)
            {
                throw new ParameterException(string.Format(CultureInfo.CurrentCulture, DisplayStrings.ErrorInvalidDelayFormat, delayInSeconds));
            }

            int processId = rawInputs.ProcessId;
            string processName = rawInputs.ProcessName;

            if (processId != 0)
            {
                processName = processHelper.ProcessNameFromId(processId);
            }
            else if (!string.IsNullOrEmpty(processName))
            {
                processName = TrimProcessName(processName);
                processId = processHelper.ProcessIdFromName(processName);
            }
            else
            {
                throw new ParameterException(DisplayStrings.ErrorNoTarget);
            }

            string verbosity = rawInputs.Verbosity;

            VerbosityLevel verbosityLevel = VerbosityLevel.Default;  // Until proven otherwise

            if (!string.IsNullOrEmpty(verbosity))
            {
                if (Enum.TryParse<VerbosityLevel>(verbosity, true, out VerbosityLevel level))
                {
                    verbosityLevel = level;
                }
                else
                {
                    throw new ParameterException(string.Format(CultureInfo.CurrentCulture, DisplayStrings.ErrorInvalidVerbosityFormat, verbosity));
                }
            }

            return new Options
            {
                OutputDirectory = rawInputs.OutputDirectory,
                ProcessId = processId,
                ProcessName = processName,
                ScanId = rawInputs.ScanId,
                VerbosityLevel = verbosityLevel,
                DelayInSeconds = delayInSeconds,
                CustomUia = rawInputs.CustomUia,
                AlwaysSaveTestFile = rawInputs.AlwaysSaveTestFile,
            };
        }

        private static string TrimProcessName(string processName)
        {
            string reducedProcessName = Path.GetFileName(processName);
            if (reducedProcessName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                reducedProcessName = Path.GetFileNameWithoutExtension(reducedProcessName);
            }
            return reducedProcessName;
        }
    }
}
