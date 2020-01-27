// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace AxeWindowsCLI
{
    public class ErrorCollector : IErrorCollector
    {
        public IReadOnlyList<string> ParameterErrors => _parameterErrors;

        public IReadOnlyList<Exception> Exceptions => _exceptions;

        List<string> _parameterErrors = new List<string>();
        List<Exception> _exceptions = new List<Exception>();

        public bool Any => _parameterErrors.Any() || _exceptions.Any();

        public void AddParameterError(string error)
        {
            if (string.IsNullOrWhiteSpace(error)) throw new ArgumentException("Parameter must be non-trivial", nameof(error));
            _parameterErrors.Add(error);
        }

        public void AddException(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));
            _exceptions.Add(exception);
        }
    }
}
