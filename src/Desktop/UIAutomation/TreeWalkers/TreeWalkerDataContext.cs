// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using System.Threading;

namespace Axe.Windows.Desktop.UIAutomation.TreeWalkers
{
    /// <summary>
    /// Data context for refreshing tree data for test
    /// </summary>
    public class TreeWalkerDataContext
    {
        public static readonly TreeWalkerDataContext DefaultContext = new TreeWalkerDataContext(Registrar.GetDefaultInstance(), CancellationToken.None);

        public Registrar Registrar { get; }

        public CancellationToken CancellationToken { get; }

        public TreeWalkerDataContext(Registrar registrar, CancellationToken cancellationToken)
        {
            Registrar = registrar;
            CancellationToken = cancellationToken;
        }
    }
}
