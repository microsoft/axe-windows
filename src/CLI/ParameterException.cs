// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace AxeWindowsCLI
{
    internal class ParameterException : ArgumentException
    {
        public ParameterException(string message, Exception innerException) :
            base(message, innerException)
        { }

        public ParameterException(string message)
            : this (message, null)
        { }

        public ParameterException()
        { }
    }
}
