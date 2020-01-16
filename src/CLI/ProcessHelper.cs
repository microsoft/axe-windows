// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using CommandLine;
using System;
using System.Diagnostics;

namespace AxeWindowsScanner
{
    public class ProcessHelper : IProcessHelper
    {
        private readonly IProcessAbstraction _processAbstraction;
        private readonly IErrorCollector _errorCollector;

        public ProcessHelper(IProcessAbstraction processAbstraction, IErrorCollector errorCollector)
        {
            if (processAbstraction == null) throw new ArgumentNullException(nameof(processAbstraction));
            if (errorCollector == null) throw new ArgumentNullException(nameof(errorCollector));
            _processAbstraction = processAbstraction;
            _errorCollector = errorCollector;
        }

        public int FindProcessByName(string processName)
        {
            var processes = _processAbstraction.GetProcessesByName(processName);

            if (processes == null || processes.Length == 0)
            {
                _errorCollector.AddParameterError("Unable to find process with name " + processName);
                return IProcessHelper.InvalidProcessId;
            }

            if (processes.Length > 1)
            {
                _errorCollector.AddParameterError("Found multiple processes with name " + processName);
                return IProcessHelper.InvalidProcessId;
            }

            return processes[0].Id;
        }

        public string FindProcessById(int processId)
        {
            Process process = null;

            try
            {
                process = _processAbstraction.GetProcessById(processId);
            }
            catch (Exception) { }

            if (string.IsNullOrEmpty(process?.ProcessName))
            {
                _errorCollector.AddParameterError("Unable to find process with id " + processId);
                return IProcessHelper.InvalidProcessName;
            }

            return process.ProcessName;
        }
    }
}
