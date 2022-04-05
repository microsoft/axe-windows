// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// A helper class to create a running stopwatch and ensure that it's always stopped
    /// when it goes out of scope. This pattern lets us avoid burying the call to Stop()
    /// in a finally block
    /// </summary>
    internal class DisposableStopwatch : IDisposable
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        private bool disposedValue;

        internal Stopwatch Stopwatch => _stopwatch;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _stopwatch.Stop();
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
    }
}
