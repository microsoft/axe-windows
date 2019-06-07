// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#if FAKES_SUPPORTED
using Axe.Windows.Automation.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
#endif

namespace Axe.Windows.AutomationTests
{
    [TestClass]
    public class StopCommandUnitTests
    {
#if FAKES_SUPPORTED
        [TestMethod]
        [Timeout (1000)]
        public void Execute_ClearInstanceSucceeds_ReturnsSuccessfulResult()
        {
            using (ShimsContext.Create())
            {
                int callsToClearInstance = 0;

                ShimAutomationSession.ClearInstance = () =>
                {
                    callsToClearInstance++;
                };

                StopCommandResult result = StopCommand.Execute();

                Assert.AreEqual(1, callsToClearInstance);
                Assert.AreEqual(true, result.Completed);
                Assert.AreEqual(true, result.Succeeded);
                Assert.IsFalse(string.IsNullOrWhiteSpace(result.SummaryMessage));
            }
        }

        [TestMethod]
        [Timeout (1000)]
        public void Execute_ClearInstanceThrowsAutomationExceptionWithExpectedMessage()
        {
            const string exceptionMessage = "Hello from your local exception!";

            using (ShimsContext.Create())
            {
                int callsToClearInstance = 0;

                ShimAutomationSession.ClearInstance = () =>
                {
                    callsToClearInstance++;
                    throw new AxeWindowsAutomationException(exceptionMessage);
                };

                try
                {
                    StopCommand.Execute();
                }
                catch (AxeWindowsAutomationException ex)
                {
                    Assert.AreEqual(exceptionMessage, ex.Message);
                }

                Assert.AreEqual(1, callsToClearInstance);
            }
        }
#endif
    }
}
