// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Axe.Windows.SystemAbstractions
{
    internal interface IMicrosoftFactory
    {
        IMicrosoftWin32 CreateMicrosoftWin32();
        IMicrosoftWin32Registry CreateMicrosoftWin32Registry();
    } // interface
} // namespace
