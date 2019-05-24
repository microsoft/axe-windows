// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Automation2
{
    /// <summary>
    /// Used to create <see cref="IAutomationSession"/> objects
    /// </summary>
    public interface IAutomationSessionFactory
    {
        /// <summary>
        /// Creates an <see cref="IAutomationConfig"/> 
        /// object which can be used to create an <see cref="IAutomationSession"/> object
        /// by calling <see cref="CreateAutomationSession(IAutomationConfig)"/>
        /// </summary>
        IAutomationConfig CreateAutomationConfig();

        /// <summary>
        /// Creates an <see cref="IAutomationSession"/>
        /// object which can be used to run AxeWindows automated tests
        /// on a process specified in the given configuration.
        /// </summary>
        /// <param name="config">
        /// An <see cref="IAutomationConfig"/> object with details about
        /// the automation session to be created
        /// </param>
        IAutomationSession CreateAutomationSession(IAutomationConfig config);
    } // interface
} // namespace
