// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AxeWindowsScanner
{
    public interface IOutputGenerator
    {
        void ShowBanner(IOptions options);
        void ShowOutput(IOptions options, IErrorCollector errorCollector, ScanResults scanResults);
    }
}
