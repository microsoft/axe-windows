// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions;
using Axe.Windows.Desktop.Settings;
using Axe.Windows.RuleSelection;
using System;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// This class holds various items that persist for the entire lifetime of an
    /// automation session. Cleanup occurs only when the stop command is executed
    /// </summary>
    internal class AutomationSession
    {
        private readonly DataManager dataManager;
        private readonly SelectAction selectAction;

        /// <summary>
        /// ctor - Initializes the app in automation mode
        /// </summary>
        /// <param name="config">A set of configuration options</param>
        private AutomationSession(Config config)
        {
            try
            {
                this.dataManager = DataManager.GetDefaultInstance();
                this.selectAction = SelectAction.GetDefaultInstance();
                this.SessionParameters = config;
            }
            catch (Exception)
            {
                Cleanup();
                throw;
            }
        }

        private void Cleanup()
        {
            this.selectAction?.Dispose();
            this.dataManager?.Dispose();
        }

        private static AutomationSession instance;
        private static readonly object lockObject = new object();

        /// <summary>
        /// Returns the CommandParameters for the session in question
        /// </summary>
        internal Config SessionParameters { get; private set; }

        /// <summary>
        /// Obtain a new instance of the AutomationSession object. Only one can exist at a time
        /// </summary>
        /// <param name="config">A set of configuration options</param>
        /// <exception cref="AxeWindowsAutomationException">Thrown if session already exists</exception>
        /// <returns>The current AutomationSession object</returns>
        internal static AutomationSession NewInstance(Config config)
        {
            lock (lockObject)
            {
                if (instance != null)
                    throw new AxeWindowsAutomationException(DisplayStrings.ErrorAlreadyStarted);

                instance = new AutomationSession(config);
                instance.SessionParameters = config;

                return instance;
            }
        }

        /// <summary>
        /// Obtain the current instance of the AutomationSession object. Only one can exist at a time.
        /// Will also track the action if an appropriate Action is provided
        /// </summary>
        /// <exception cref="AxeWindowsAutomationException">Thrown if session object does not exist</exception>
        /// <returns>The current AutomationSession object</returns>
        internal static AutomationSession Instance()
        {
            lock (lockObject)
            {
                if (instance == null)
                    throw new AxeWindowsAutomationException(DisplayStrings.ErrorNotStarted_Instance);
                return instance;
            }
        }

        /// <summary>
        /// Clear the current instance of the AutomationSession object
        /// </summary>
        /// <exception cref="AxeWindowsAutomationException">Thrown if session object does not exist</exception>
        internal static void ClearInstance()
        {
            lock (lockObject)
            {
                if (instance == null)
                    throw new AxeWindowsAutomationException(DisplayStrings.ErrorNotStarted_Clear);
                instance.Cleanup();
                instance = null;
            }
        }
    }
}
