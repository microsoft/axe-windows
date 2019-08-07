// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation.Resources;
using System;
using System.Runtime.Serialization;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// The exception type thrown for all unhandled errors in Axe.Windows.Automation.
    /// If an exception was thrown from code not owned by Axe.Windows.Automation,
    /// that exception will be wrapped in <see cref="AxeWindowsAutomationException"/>
    /// In the <see cref="Exception.InnerException"/> property.
    /// </summary>
    [Serializable]
    public class AxeWindowsAutomationException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AxeWindowsAutomationException()
        { }

        /// <summary>
        /// Constructor taking a message
        /// </summary>
        /// <param name="message"></param>
        public AxeWindowsAutomationException(string message)
            : base(message)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException(ErrorMessages.StringNullOrWhiteSpace, nameof(message));
        }

        /// <summary>
        /// Constructor taking a message string
        /// and an optional inner exception to wrap
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public AxeWindowsAutomationException(string message, Exception innerException)
            : base(message, innerException)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException(ErrorMessages.StringNullOrWhiteSpace, nameof(message));
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected AxeWindowsAutomationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
