// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading;

namespace Axe.Windows.AutomationTests
{
    [TestClass]
    public class DisposableStopwatchTests
    {
        private readonly TimeSpan OneMillisecond = TimeSpan.FromMilliseconds(1);

        [TestMethod]
        [Timeout(1000)]
        public void Ctor_StartsStopwatch()
        {
            using (var sw = new DisposableStopwatch())
            {
                Thread.Sleep(OneMillisecond);
                Assert.AreNotEqual(0, sw.Stopwatch.ElapsedTicks);
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void Dispose_StopsStopwatch()
        {
            Stopwatch savedStopwatch = null;

            using (var sw = new DisposableStopwatch())
            {
                // Production code should never save a reference inside a using block!
                savedStopwatch = sw.Stopwatch;
                Thread.Sleep(OneMillisecond);
            }

            long ticksAfterDispose = savedStopwatch.ElapsedTicks;
            Thread.Sleep(OneMillisecond);

            Assert.AreNotEqual(0, ticksAfterDispose);
            Assert.AreEqual(ticksAfterDispose, savedStopwatch.ElapsedTicks);
        }
    }
}
