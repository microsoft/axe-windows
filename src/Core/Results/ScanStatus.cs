// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace Axe.Windows.Core.Results
{
    /// <summary>
    /// Test status enum
    /// </summary>
    public enum ScanStatus
    {
        NoResult = 0,
        Pass = 1,
        Uncertain = 2,
        Fail = 3,
        ScanNotSupported = 4, // for the cases like HTML framework which we don't support.
    }
}
