// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

namespace AxeWindowsCLI
{
    public class ProcessHelper : IProcessHelper
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
            catch (Exception e)
            {
                caughtException = e;
            }

            if (caughtException != null ||  processes == null || processes.Length == 0)
            {
                throw new ParameterException("Unable to find process with name " + processName, caughtException);
            }

            if (processes.Length > 1)
            {
                throw new ParameterException("Found multiple processes with name " + processName);
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
            catch (Exception e)
            {
                innerException = e;
            }

            if (string.IsNullOrEmpty(process?.ProcessName))
            {
                throw new ParameterException("Unable to find process with id " + processId, innerException);
            }

            return process.ProcessName;
        }
    }
}
