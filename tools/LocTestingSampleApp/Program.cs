using Axe.Windows.Automation;
using Axe.Windows.Automation.Data;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;

namespace AxeWinLocTesting
{
    internal class Program
    {
        // Change this code to experiment with differnet languages, for example "es", "it", "ja", "ko".
        private static string langCode = "fr";

        // Change this path to experiment with different test apps.
        private static string exePath = "../WildlifeManager/WildlifeManager.exe";

        // True will show full JSON scan results, false will just show a short example. The long output can
        // be very long and confusing, since it serializes both content that should be localized (ex: rule
        // descriptions/ text) and content that should not be localized (ex: property/field names).
        private static bool showFullResults = false;

        // Wait time to allow process to start up before beginning scan.
        private static int processStartupWaitTime = 1000;

        static void Main(string[] _)
        {
            var process = StartTestExe();
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(langCode);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langCode);
            try
            {
                var scanner = CreateScanner(process.Id);

                var scanResults = GetScanResults(scanner);

                PrintScanResults(scanResults, showFullResults);
            }
            finally
            {
                process.Kill();
            }
        }

        private static Process StartTestExe()
        {
            var process = Process.Start(exePath);
            Thread.Sleep(processStartupWaitTime);
            return process;
        }

        private static IScanner CreateScanner(int processId)
        {
            Console.WriteLine("Creating scanner...");
            var configBuilder = Config.Builder.ForProcessId(processId);
            configBuilder.WithOutputFileFormat(OutputFileFormat.None);
            var config = configBuilder.Build();
            var scanner = ScannerFactory.CreateScanner(config);
            return scanner;
        }

        private static ScanOutput GetScanResults(IScanner scanner)
        {
            Console.WriteLine("Scanning...");
            var scanResults = scanner.Scan(null);
            return scanResults;
        }

        private static void PrintScanResults(ScanOutput scanResults, bool fullContent)
        {
            Console.WriteLine("Scan results:");
            Console.WriteLine();
            Console.WriteLine("===========================================");
            if (fullContent)
            {
                Console.WriteLine(JsonConvert.SerializeObject(scanResults, Formatting.Indented));
            }
            else
            {
                foreach (var scanResult in scanResults.WindowScanOutputs)
                {
                    foreach (var error in scanResult.Errors)
                    {
                        Console.WriteLine(error.Rule.Description);
                    }
                }
            }
            Console.WriteLine("===========================================");
            Console.WriteLine();
        }
    }
}