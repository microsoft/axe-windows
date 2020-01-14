using System.Diagnostics;

namespace AxeWindowsScanner
{
    static class ProcessHelper
    {
        internal static int FindProcessByName(string processName)
        {
            var processes = Process.GetProcessesByName(processName);

            if ((processes.Length == 0) || (processes.Length > 1))
            {
                // throw some sort of exception here!
            }

            return processes[0].Id;
        }

        internal static string FindProcessById(int processId)
        {
            var process = Process.GetProcessById(processId);

            if (string.IsNullOrEmpty(process?.ProcessName))
            {
                // throw some sort of exception here!
            }

            return process.ProcessName;
        }
    }
}
