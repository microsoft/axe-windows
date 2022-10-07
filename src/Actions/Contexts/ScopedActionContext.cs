﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using Axe.Windows.Desktop.UIAutomation.TreeWalkers;
using System;
using System.Threading;

namespace Axe.Windows.Actions.Contexts
{
    /// <summary>
    /// An implementation of <see cref="IActionContext"></see> for scoped duration.
    /// Object cleanup occurs when the object is Disposed.
    /// </summary>
    internal class ScopedActionContext : IActionContext
    {
        private bool disposedValue;

        private ScopedActionContext(DataManager dataManager, SelectAction selectAction, TreeWalkerDataContext treeWalkerDataContext)
        {
            DataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
            SelectAction = selectAction ?? throw new ArgumentNullException(nameof(selectAction));
            TreeWalkerDataContext = treeWalkerDataContext ?? throw new ArgumentNullException(nameof(treeWalkerDataContext));
        }

        public DataManager DataManager { get; }

        public SelectAction SelectAction { get; }

        public Registrar Registrar => TreeWalkerDataContext.Registrar;

        public TreeWalkerDataContext TreeWalkerDataContext { get; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DataManager.Dispose();
                    SelectAction.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ScopedActionContext() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        internal static IActionContext CreateInstance(CancellationToken cancellationToken)
        {
            DataManager dataManager = DataManager.CreateInstance();
            return new ScopedActionContext(
                dataManager,
                SelectAction.CreateInstance(dataManager),
                new TreeWalkerDataContext(new Registrar(), cancellationToken));
        }
    }
}
