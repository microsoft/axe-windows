// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using AxeWindowsCLI.Resources;
using System;
using System.Diagnostics;
using System.Globalization;

namespace AxeWindowsCLI
{
    internal class ProcessHelper : IProcessHelper
    {
        private readonly IProcessAbstraction _processAbstraction;

        public ProcessHelper(IProcessAbstraction processAbstraction)
        {
            if (processAbstraction == null) throw new ArgumentNullException(nameof(processAbstraction));
            _processAbstraction = processAbstraction;
        }

        public int ProcessIdFromName(string processName)
        {
            Process[] processes = null;
            Exception caughtException = null;
            try
            {
                processes = _processAbstraction.GetProcessesByName(processName);
            }
#pragma warning disable CA1031
            catch (Exception e)
#pragma warning restore CA1031
            {
                caughtException = e;
            }

            if (caughtException != null ||  processes == null || processes.Length == 0)
            {
                throw new ParameterException(
                    string.Format(CultureInfo.InvariantCulture, DisplayStrings.ErrorNoMatchingProcessNameFormat, processName),
                    caughtException);
            }

            if (processes.Length > 1)
            {
                throw new ParameterException(
                    string.Format(CultureInfo.InvariantCulture, DisplayStrings.ErrorMultipleMatchingProcessNameFormat, processName));
            }

            return processes[0].Id;
        }

        public string ProcessNameFromId(int processId)
        {
            Exception innerException = null;
            Process process = null;

            try
            {
                process = _processAbstraction.GetProcessById(processId);
            }
#pragma warning disable CA1031
            catch (Exception e)
#pragma warning restore CA1031
            {
                innerException = e;
            }

            if (string.IsNullOrEmpty(process?.ProcessName))
            {
                throw new ParameterException(
                    string.Format(CultureInfo.InvariantCulture, DisplayStrings.ErrorNoMatchingProcessIdFormat, processId),
                    innerException);
            }

            return process.ProcessName;
        }
    }
}
