// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Threading;

namespace Axe.Windows.AutomationTests
{
    internal class TimedExecutionWrapper
    {
        private static readonly TimeSpan SleepTime = TimeSpan.FromMilliseconds(250);

        private readonly TimeSpan _allowedTime;
        private Stopwatch _stopwatch;
        private Thread _thread;

        internal bool Completed { get; private set; }
        internal Exception CaughtException { get; private set; }

        internal TimedExecutionWrapper(TimeSpan allowedTime)
        {
            _allowedTime = allowedTime;
        }

        internal void RunAction(Action testAction)
        {
            CaughtException = null;
            _stopwatch = Stopwatch.StartNew();
            CreateThreadForTest(testAction);
            WaitForThreadToStart();
            WaitForThreadToComplete();
            Completed = !_thread.IsAlive;
        }

        private void CreateThreadForTest(Action testAction)
        {
            _thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    testAction();
                }
                catch (Exception e)
                {
                    CaughtException = e;
                }
            }));
            _thread.Start();
        }

        private void WaitForThreadToStart()
        {
            while (_stopwatch.Elapsed < _allowedTime && !_thread.IsAlive)
            {
                Thread.Sleep(SleepTime);
            }
        }

        private void WaitForThreadToComplete()
        {
            while (_stopwatch.Elapsed < _allowedTime && _thread.IsAlive)
            {
                Thread.Sleep(SleepTime);
            }
        }
    }
}
