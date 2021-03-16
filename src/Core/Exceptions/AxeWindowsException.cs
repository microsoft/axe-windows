// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Runtime.Serialization;

namespace Axe.Windows.Core.Exceptions
{
    /// <summary>
    /// AxeWindowsException
    /// Thrown by Axe.Windows libraries
    /// </summary>
    [Serializable]
    public class AxeWindowsException : Exception
    {
        public AxeWindowsException()
        {
        }

        public AxeWindowsException(string message) : base(message)
        {
        }

        public AxeWindowsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AxeWindowsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
