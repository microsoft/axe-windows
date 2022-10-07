// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using Axe.Windows.Desktop.UIAutomation.TreeWalkers;
using System;

namespace Axe.Windows.Actions.Contexts
{
    /// <summary>
    /// Actions need backing data. An IActionContext groups together related items
    /// into a single package to ensure that the data remains consistent and has
    /// a manageable lifetime.
    /// </summary>
    public interface IActionContext : IDisposable
    {
        /// <summary>
        /// The <see cref="DataManager"/> object that provides access to the element data
        /// </summary>
        DataManager DataManager { get; }

        /// <summary>
        /// The <see cref="SelectAction"/> object that manages object selection
        /// </summary>
        SelectAction SelectAction { get; }

        /// <summary>
        /// The <see cref="Registrar"/> object that provides access to CustomUIA data
        /// </summary>
        Registrar Registrar { get; }

        /// <summary>
        /// The <see cref="TreeWalkerDataContext"/> object to keep TreeWalker in the proper context
        /// </summary>
        TreeWalkerDataContext TreeWalkerDataContext { get; }
    }
}
