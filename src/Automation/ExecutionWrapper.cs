// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation.Resources;
using System;
using System.Globalization;

namespace Axe.Windows.Automation
{
    internal static class ExecutionWrapper
    {
        private static readonly object lockObject = new object();

        /// <summary>
        /// Synchronously (and blocking) execute the passed-in command, handling errors appropriately
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <returns>An object of Type T that describes the command result</returns>
        internal static T ExecuteCommand<T>(Func<T> command)
        {
            lock (lockObject)
            {
                try
                {
                    return command();
                }
                catch (AxeWindowsAutomationException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    string message = string.Format(CultureInfo.InvariantCulture, DisplayStrings.ErrorUnhandledExceptionFormat, ex.ToString());
                    throw new AxeWindowsAutomationException(message, ex);
                }
            }
        }
    }
}
