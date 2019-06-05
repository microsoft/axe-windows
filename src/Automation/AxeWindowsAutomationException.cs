// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// The exception type thrown for all unhandled errors in Axe.Windows.Automation.
    /// If an exception was thrown from code not owned by Axe.Windows.Automation,
    /// that exception will be wrapped in <see cref="AxeWindowsAutomationException"/>
    /// In the <see cref="Exception.InnerException"/> property.
    /// </summary>
    public class AxeWindowsAutomationException : Exception
    {
        /// <summary>
        /// Constructor taking a text description of the exception
        /// </summary>
        /// <param name="message">A text description of the exception</param>
        public AxeWindowsAutomationException(string message)
            : base(message)
        { }
    } // class
} // namespace
