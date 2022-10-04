// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using System;

namespace Axe.Windows.Actions.Contexts
{
    internal class TransientActionContext : IActionContext
    {
        private bool disposedValue;

        private TransientActionContext(DataManager dataManager, SelectAction selectAction, Registrar registrar)
        {
            DataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager));
            SelectAction = selectAction ?? throw new ArgumentNullException(nameof(selectAction));
            Registrar = registrar ?? throw new ArgumentNullException(nameof(registrar));
        }

        public DataManager DataManager { get; }

        public SelectAction SelectAction { get; }

        public Registrar Registrar { get; }

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

        internal static IActionContext CreateInstance()
        {
            DataManager dataManager = DataManager.CreateInstance();
            return new TransientActionContext(dataManager, SelectAction.CreateInstance(dataManager), new Registrar());
        }
    }
}
