// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;

namespace Axe.Windows.Automation
{
    internal interface ITargetElementLocator
    {
        A11yElement LocateRootElement(int processId);
    } // interface
} // namespace
