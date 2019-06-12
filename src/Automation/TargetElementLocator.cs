// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Core.Bases;
using Axe.Windows.Desktop.UIAutomation;
using System;
using System.Globalization;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Wraps up the logic of selecting a target element basd on the command parameters
    /// </summary>
    internal static class TargetElementLocator
    {
        /// <summary>
        /// Locate the target element in the UIA tree
        /// </summary>
        /// <param name="processId">The process id of the application to scan</param>
        /// <returns>The ElementContext that matches the targeting parameters</returns>
        internal static ElementContext LocateRootElement(int processId)
        {
            try
            {
                var element = A11yAutomation.ElementFromProcessId(processId);

                return new ElementContext(element);
            }
            catch (Exception ex)
            {
                throw new AxeWindowsAutomationException(string.Format(CultureInfo.InvariantCulture, DisplayStrings.ErrorFailToGetRootElementOfProcess, processId, ex), ex);
            }
        }
    }
}
