// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace AxeWindowsCLI
{
    internal static class OptionsEvaluator
    {
        public static IOptions ProcessInputs(Options rawInputs, IProcessHelper processHelper)
        {
            if (rawInputs == null) throw new ArgumentNullException(nameof(rawInputs));
            if (processHelper == null) throw new ArgumentNullException(nameof(processHelper));

            int processId = rawInputs.ProcessId;
            string processName = rawInputs.ProcessName;
            bool showThirdPartyNotices = rawInputs.ShowThirdPartyNotices;
            bool processIdIsInteresting = processId > 0;
            bool processNameIsInteresting = !string.IsNullOrEmpty(processName);

            if (showThirdPartyNotices)
            {
                if (processIdIsInteresting || processNameIsInteresting)
                {
                    throw new ParameterException("The ShowThirdPartyNotices option can't be specified with the processId or the processName options");
                }
            }
            else
            {
                if (processIdIsInteresting)
                {
                    processName = processHelper.ProcessNameFromId(processId);
                }
                else if (processNameIsInteresting)
                {
                    string p = Path.GetFileNameWithoutExtension(processName);
                    processName = p;
                    processId = processHelper.ProcessIdFromName(p);
                }
                else
                {
                    throw new ParameterException("Please specify either processId or processName on the command line");
                }
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
                    throw new ParameterException("Invalid verbosity level: " + verbosity);
                }
            }

            return new Options
            {
                OutputDirectory = rawInputs.OutputDirectory,
                ProcessId = processId,
                ProcessName = processName,
                ScanId = rawInputs.ScanId,
                VerbosityLevel = verbosityLevel,
                ShowThirdPartyNotices = showThirdPartyNotices,
            };
        }
    }
}
