// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Class to start AxeWindows (via StartCommand.Execute)
    /// </summary>
    public static class StartCommand
    {
        /// <summary>
        /// Execute the Start command. Used by both .NET and by PowerShell entry points
        /// </summary>
        /// <param name="primaryConfig">The primary calling parameters</param>
        /// <param name="configFile">Path to a config file containing the backup parameters</param>
        /// <returns>A StartCommandResult that describes the result of the command</returns>
        public static StartCommandResult Execute(Dictionary<string, string> primaryConfig, string configFile)
        {
            return ExecutionWrapper.ExecuteCommand(() =>
            {
                // Custom assembly resolver needs to be created before anything else, but only for PowerShell
                CommandParameters parameters = new CommandParameters(primaryConfig, configFile);
                AutomationSession.NewInstance(parameters);
                return new StartCommandResult
                {
                    Completed = true,
                    SummaryMessage = DisplayStrings.SuccessStart,
                    Succeeded = true,
                };
            });
        }
    }
}
