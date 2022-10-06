// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;

namespace Axe.Windows.Desktop.UIAutomation.TreeWalkers
{
    /// <summary>
    /// Data context for refreshing tree data for test
    /// </summary>
    internal class TreeWalkerDataContext
    {
        public CancellationToken CancellationToken { get; set; }

        public TreeWalkerDataContext(CancellationToken? cancellationToken = null)
        {
            CancellationToken = cancellationToken ?? CancellationToken.None;
        }
    }
}
