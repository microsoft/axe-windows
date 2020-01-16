// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace AxeWindowsScanner
{
    public class ErrorCollector : IErrorCollector
    {
        public IReadOnlyList<string> ParameterErrors => _parameterErrors;

        public IReadOnlyList<string> ScanErrors => _scanErrors;

        public IReadOnlyList<Exception> Exceptions => _exceptions;

        List<string> _parameterErrors = new List<string>();
        List<string> _scanErrors = new List<string>();
        List<Exception> _exceptions = new List<Exception>();

        public void AddParameterError(string error)
        {
            if (string.IsNullOrWhiteSpace(error)) throw new ArgumentException("Parameter must be non-trivial", nameof(error));
            _parameterErrors.Add(error);
        }

        public void AddScanError(string error)
        {
            if (string.IsNullOrWhiteSpace(error)) throw new ArgumentException("Parameter must be non-trivial", nameof(error));
            _scanErrors.Add(error);
        }

        public void AddException(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            _exceptions.Add(exception);
        }
    }
}
