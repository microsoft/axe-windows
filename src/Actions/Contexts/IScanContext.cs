// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using System;

namespace Axe.Windows.Actions.Contexts
{
    public interface IScanContext : IDisposable
    {
        DataManager DataManager { get; }
        SelectAction SelectAction { get; }

        Registrar Registrar { get; }
    }
}
