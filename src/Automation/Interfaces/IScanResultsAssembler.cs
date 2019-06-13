using Axe.Windows.Core.Bases;

namespace Axe.Windows.Automation.Interfaces
{
    /// <summary>
    /// Provides methods used to assemble a <see cref="ScanResults"/> object
    /// </summary>
    public interface IScanResultsAssembler
    {
        /// <summary>
        /// Assembles failed scans from the provided element
        /// </summary>
        /// <param name="element">Root element from which scan results will be assembled</param>
        /// <returns>A ScanResults object containing the relevant errors and error count</returns>
        ScanResults AssembleScanResultsFromElement(A11yElement element);
    }
}
