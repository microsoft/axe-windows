// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions;
using Axe.Windows.Core.Bases;
using System.Collections.Generic;

namespace Axe.Windows.Automation
{
    internal interface ITargetElementLocator
    {
        IEnumerable<A11yElement> LocateRootElements(int processId, DataManager dataManager);
    } // interface
} // namespace
