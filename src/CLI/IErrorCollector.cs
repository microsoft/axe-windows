// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace AxeWindowsScanner
{
    public interface IErrorCollector
    {
        public IReadOnlyList<string> ParameterErrors { get; }
        public IReadOnlyList<Exception> Exceptions { get; }

        public void AddParameterError(string error);
        public void AddException(Exception e);
    }
}
