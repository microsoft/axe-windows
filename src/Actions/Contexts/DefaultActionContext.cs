// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using Axe.Windows.Desktop.UIAutomation.TreeWalkers;
using System;

namespace Axe.Windows.Actions.Contexts
{
    /// <summary>
    /// An implementation of <see cref="IActionContext"></see> for app-lifetime use. Everything goes to the 
    /// default stores with no lifetime changes.
    /// </summary>
    internal class DefaultActionContext : IActionContext
    {
        static IActionContext DefaultContext;

        public DataManager DataManager => DataManager.GetDefaultInstance();

        public SelectAction SelectAction => SelectAction.GetDefaultInstance();

        public Registrar Registrar => TreeWalkerDataContext.Registrar;
        
        public TreeWalkerDataContext TreeWalkerDataContext => TreeWalkerDataContext.DefaultContext;

        protected virtual void Dispose(bool disposing)
        {
            // This class never disposes, since the objects have application lifetime
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        internal static IActionContext GetDefaultInstance()
        {
            if (DefaultContext == null)
            {
                DefaultContext = new DefaultActionContext();
            }

            return DefaultContext;
        }
    }
}
