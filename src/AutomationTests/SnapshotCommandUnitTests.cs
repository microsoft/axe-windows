// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Automation;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Misc;
using Axe.Windows.Core.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
#if FAKES_SUPPORTED
using Axe.Windows.Actions.Contexts.Fakes;
using Axe.Windows.Actions.Fakes;
using Axe.Windows.Automation.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
#endif

namespace Axe.Windows.AutomationTests
{
    using ScanResult = Axe.Windows.Core.Results.ScanResult;
    using ScanResults = Axe.Windows.Core.Results.ScanResults;

    [TestClass]
    public class SnapshotCommandUnitTests
    {
        const string TestMessage = "I find his reaction most illogical";

#if FAKES_SUPPORTED
        private IOutputFileHelper OutputFileHelperStub = new Axe.Windows.Automation.Fakes.StubIOutputFileHelper();
#endif

        private static void AssertCompleteResult(SnapshotCommandResult result, int expectedPass, int expectedFail, int expectedInconclusive, int expectedUnsupported)
        {
            Assert.AreEqual(true, result.Completed);
            Assert.AreEqual(expectedFail, result.ScanResultsFailedCount, "Mismatch in count of failures");
            Assert.AreEqual(expectedInconclusive, result.ScanResultsInconclusiveCount, "Mismatch in count of inconclusives");
            Assert.AreEqual(expectedPass, result.ScanResultsPassedCount, "Mismatch in count of passes");
            Assert.AreEqual(expectedUnsupported, result.ScanResultsUnsupportedCount, "Mismatch in count of unsupporteds");
            Assert.AreEqual(expectedPass + expectedFail + expectedInconclusive + expectedUnsupported, result.ScanResultsTotalCount);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.SummaryMessage), "SummaryMessage can't be trivial");
        }

        private static A11yElement CreateA11yElement(List<A11yElement> children = null, List<ScanStatus> statuses = null)
        {
            A11yElement element = new A11yElement();

            if (children != null)
                element.Children = children;

            if (statuses != null)
            {
                ScanResult scanResult = new ScanResult
                {
                    Items = new List<RuleResult>()
                };

                foreach (ScanStatus status in statuses)
                {
                    scanResult.Items.Add(new RuleResult {Status = status});
                }

                ScanResults elementScanResults = new ScanResults();
                elementScanResults.AddScanResult(scanResult);

                element.ScanResults = elementScanResults;
            }

            return element;
        }

#if FAKES_SUPPORTED
        [TestMethod]
        [Timeout(2000)]
        public void Execute_TargetElementLocatorReceivesExpectedParameters()
        {
            using (ShimsContext.Create())
            {
                int expectedProcessId = 42;
                int actualProcessId = 0;

                Config config = Config.Builder.ForProcessId(expectedProcessId).Build();

                ShimTargetElementLocator.LocateRootElementInt32 = (processId) =>
                {
                    actualProcessId = processId;
                    throw new Exception(TestMessage);
                };

                InitializeShims();

                try
                {
                    SnapshotCommandResult result = SnapshotCommand.Execute(config, OutputFileHelperStub);
                }
                catch (AxeWindowsAutomationException ex)
                {
                    Assert.AreEqual(TestMessage, ex.InnerException.Message);
                }

                Assert.AreEqual(expectedProcessId, actualProcessId);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Execute_UnableToSelectCandidateElement_ThrowsAutomationException_ErrorAutomation008()
        {
            using (ShimsContext.Create())
            {
                ShimSelectAction selectAction = new ShimSelectAction()
                {
                    SetCandidateElementA11yElement = (element) => { },
                    Select = () => false,
                };

                InitializeShims(selectAction:selectAction, shimTargetElementLocator: true);

                var config = Config.Builder.ForProcessId(-1).Build();

                try
                {
                    SnapshotCommandResult result = SnapshotCommand.Execute(config, OutputFileHelperStub);
                }
                catch (AxeWindowsAutomationException ex)
                {
                    Assert.IsTrue(ex.Message.Contains(" Automation008:"));
                }
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Execute_UnableToSetTestModeDataContext_ThrowsAutomationException_ErrorAutomation008()
        {
            using (ShimsContext.Create())
            {
                ShimSelectAction selectAction = new ShimSelectAction()
                {
                    SetCandidateElementA11yElement = (element) => { },
                    Select = () => true,
                    POIElementContextGet = () => new ElementContext(CreateA11yElement()),
                };

                InitializeShims(shimTargetElementLocator: true,
                    shimUiFramework: true, setTestModeSucceeds: false);

                var config = Config.Builder.ForProcessId(-1).Build();

                try
                {
                    SnapshotCommandResult result = SnapshotCommand.Execute(config, OutputFileHelperStub);
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(ex.Message.Contains(" Automation008:"));
                }
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Execute_NoScanResultsInPOI_ThrowsAutomationException_ErrorAutomation012()
        {
            using (ShimsContext.Create())
            {
                ShimSelectAction selectAction = new ShimSelectAction()
                {
                    SetCandidateElementA11yElement = (element) => { },
                    Select = () => true,
                    POIElementContextGet = () => new ElementContext(CreateA11yElement()),
                };

                InitializeShims(shimTargetElementLocator: true,
                    selectAction: selectAction, elementBoundExceeded: false, shimUiFramework: true,
                    setTestModeSucceeds: true);

                var config = Config.Builder.ForProcessId(-1).Build();

                try
                {
                    SnapshotCommandResult result = SnapshotCommand.Execute(config, OutputFileHelperStub);
                }
                catch (AxeWindowsAutomationException ex)
                {
                    Assert.IsTrue(ex.Message.Contains(" Automation012:"));
                }
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Execute_UpperBoundIsReached_ThrowsAutomationException_ErrorAutomation017()
        {
            using (ShimsContext.Create())
            {
                ShimSelectAction selectAction = new ShimSelectAction()
                {
                    SetCandidateElementA11yElement= (element) => { },
                    Select = () => true,
                    POIElementContextGet = () => new ElementContext(
                        new A11yElement()),
                };

                InitializeShims(shimTargetElementLocator: true,
                    selectAction: selectAction, elementBoundExceeded: true, shimUiFramework: true,
                    setTestModeSucceeds: true);

                var config = Config.Builder.ForProcessId(-1).Build();

                try
                {
                    SnapshotCommandResult result = SnapshotCommand.Execute(config, OutputFileHelperStub);
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(ex.Message.Contains(" Automation017:"));
                }
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Execute_NoScanResultsInPOI_ReturnsCompleteWithNoIssues()
        {
            using (ShimsContext.Create())
            {
                ShimSelectAction selectAction = new ShimSelectAction()
                {
                    SetCandidateElementA11yElement = (element) => { },
                    Select = () => true,
                    POIElementContextGet = () => new ElementContext(
                        new A11yElement
                        {
                            ScanResults = new ScanResults(),
                            Children = new List<A11yElement>
                            {
                                new A11yElement
                                {
                                    ScanResults = new ScanResults(),
                                    Children = new List<A11yElement>(),
                                }
                            }
                        }),
                };

                InitializeShims(shimTargetElementLocator: true,
                    selectAction: selectAction, elementBoundExceeded: false, shimUiFramework: true,
                    setTestModeSucceeds: true);

                var config = Config.Builder.ForProcessId(-1).Build();

                SnapshotCommandResult result = SnapshotCommand.Execute(config, OutputFileHelperStub);

                AssertCompleteResult(result, 0, 0, 0, 0);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Execute_NoScanResultsInPOI_SavesFile_ReturnsCompleteWithNoIssues()
        {
            using (ShimsContext.Create())
            {
                ShimSelectAction selectAction = new ShimSelectAction()
                {
                    SetCandidateElementA11yElement = (element) => { },
                    Select = () => true,
                    POIElementContextGet = () => new ElementContext(
                        new A11yElement
                        {
                            ScanResults = new ScanResults(),
                            Children = new List<A11yElement>
                            {
                                new A11yElement
                                {
                                    ScanResults = new ScanResults(),
                                    Children = new List<A11yElement>(),
                                }
                            }
                        }),
                };

                InitializeShims(shimTargetElementLocator: true,
                    selectAction: selectAction, elementBoundExceeded: false, shimUiFramework: true,
                    setTestModeSucceeds: true, shimScreenCapture: true, shimSnapshot: true, shimSarif: true);

                var config = Config.Builder.ForProcessId(-1).Build();

                SnapshotCommandResult result = SnapshotCommand.Execute(config, OutputFileHelperStub);

                AssertCompleteResult(result, 0, 0, 0, 0);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Execute_OnlyPassesInPOI_SavesFile_ReturnsComplete_PassesOnly()
        {
            using (ShimsContext.Create())
            {
                List<ScanStatus> scanStatusPass = new List<ScanStatus> { ScanStatus.Pass };

                ShimSelectAction selectAction = new ShimSelectAction()
                {
                    SetCandidateElementA11yElement = (element) => { },
                    Select = () => true,
                    POIElementContextGet = () => new ElementContext(
                        CreateA11yElement(
                            new List<A11yElement>
                            {
                                CreateA11yElement(new List<A11yElement>(), scanStatusPass)
                            },
                            scanStatusPass))
                };

                InitializeShims(shimTargetElementLocator: true,
                    selectAction: selectAction, elementBoundExceeded: false, shimUiFramework: true,
                    setTestModeSucceeds: true, shimScreenCapture: true, shimSnapshot: true, shimSarif: true);

                var config = Config.Builder.ForProcessId(-1).Build();

                SnapshotCommandResult result = SnapshotCommand.Execute(config, OutputFileHelperStub);

                AssertCompleteResult(result, 2, 0, 0, 0);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Execute_OnlyInconclusiveInPOI_SavesFile_ReturnsComplete_InconclusivesOnly()
        {
            using (ShimsContext.Create())
            {
                List<ScanStatus> scanScatusUncertain = new List<ScanStatus> { ScanStatus.Uncertain };

                ShimSelectAction selectAction = new ShimSelectAction()
                {
                    SetCandidateElementA11yElement = (element) => { },
                    Select = () => true,
                    POIElementContextGet = () => new ElementContext(
                        CreateA11yElement(
                            new List<A11yElement>
                            {
                                CreateA11yElement(new List<A11yElement>(), scanScatusUncertain)
                            },
                            scanScatusUncertain))
                };

                InitializeShims(shimTargetElementLocator: true,
                    selectAction: selectAction, elementBoundExceeded: false, shimUiFramework: true,
                    setTestModeSucceeds: true, shimScreenCapture: true, shimSnapshot: true, shimSarif: true);

                var config = Config.Builder.ForProcessId(-1).Build();

                SnapshotCommandResult result = SnapshotCommand.Execute(config, OutputFileHelperStub);

                AssertCompleteResult(result, 0, 0, 2, 0);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Execute_OnlyFailuresInPOI_SavesFile_ReturnsComplete_FailuresOnly()
        {
            using (ShimsContext.Create())
            {
                List<ScanStatus> scanStatusFail = new List<ScanStatus> { ScanStatus.Fail };

                ShimSelectAction selectAction = new ShimSelectAction()
                {
                    SetCandidateElementA11yElement = (element) => { },
                    Select = () => true,
                    POIElementContextGet = () => new ElementContext(
                        CreateA11yElement(
                            new List<A11yElement>
                            {
                                CreateA11yElement(new List<A11yElement>(), scanStatusFail)
                            },
                            scanStatusFail))
                };

                InitializeShims(shimTargetElementLocator: true,
                    selectAction:selectAction, elementBoundExceeded: false, shimUiFramework: true,
                    setTestModeSucceeds: true, shimScreenCapture: true, shimSnapshot: true, shimSarif: true);

                var config = Config.Builder.ForProcessId(-1).Build();

                SnapshotCommandResult result = SnapshotCommand.Execute(config, OutputFileHelperStub);

                AssertCompleteResult(result, 0, 2, 0, 0);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Execute_OnlyUnsupportedsInPOI_SavesFile_ReturnsComplete_UnsupportedsOnly()
        {
            using (ShimsContext.Create())
            {
                List<ScanStatus> scanStatusUnsupported = new List<ScanStatus> { ScanStatus.ScanNotSupported };

                ShimSelectAction selectAction = new ShimSelectAction()
                {
                    SetCandidateElementA11yElement = (element) => { },
                    Select = () => true,
                    POIElementContextGet = () => new ElementContext(
                        CreateA11yElement(
                            new List<A11yElement>
                            {
                                CreateA11yElement(new List<A11yElement>(), scanStatusUnsupported)
                            },
                            scanStatusUnsupported))
                };

                InitializeShims(shimTargetElementLocator: true,
                    selectAction: selectAction, elementBoundExceeded: false, shimUiFramework: true,
                    setTestModeSucceeds: true, shimScreenCapture: true, shimSnapshot: true, shimSarif: true);

                var config = Config.Builder.ForProcessId(-1).Build();

                SnapshotCommandResult result = SnapshotCommand.Execute(config, OutputFileHelperStub);

                AssertCompleteResult(result, 0, 0, 0, 2);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Execute_MixedResultsInPOI_SavesFile_ReturnsComplete_CorrectMixedResults()
        {
            using (ShimsContext.Create())
            {
                ShimSelectAction selectAction = new ShimSelectAction()
                {
                    SetCandidateElementA11yElement = (element) => { },
                    Select = () => true,
                    POIElementContextGet = () => new ElementContext(
                        CreateA11yElement(
                            new List<A11yElement>
                            {
                                CreateA11yElement(new List<A11yElement>(), new List<ScanStatus>
                                {
                                    ScanStatus.Fail, ScanStatus.Pass   // Will count as failure
                                }),
                                CreateA11yElement(new List<A11yElement>(), new List<ScanStatus>
                                {
                                    ScanStatus.Pass  // Will count as pass
                                }),
                                CreateA11yElement(new List<A11yElement>(), new List<ScanStatus>
                                {
                                    ScanStatus.Uncertain, ScanStatus.Uncertain, ScanStatus.ScanNotSupported  // Will count as unsupported
                                }),
                                CreateA11yElement(new List<A11yElement>(), new List<ScanStatus>
                                {
                                    ScanStatus.Pass, ScanStatus.Uncertain, ScanStatus.NoResult  // Will count as uncertain
                                }),
                                CreateA11yElement(new List<A11yElement>(), new List<ScanStatus>
                                {
                                    ScanStatus.Uncertain, ScanStatus.NoResult  // Will count as uncertain
                                }),
                            },
                            new List<ScanStatus>
                            {
                                ScanStatus.Pass  // Will count as pass
                            }))
                };

                InitializeShims(shimTargetElementLocator: true,
                    selectAction: selectAction, elementBoundExceeded: false, shimUiFramework: true,
                    setTestModeSucceeds: true, shimScreenCapture: true, shimSnapshot: true, shimSarif: true);

                var config = Config.Builder.ForProcessId(-1).Build();

                SnapshotCommandResult result = SnapshotCommand.Execute(config, OutputFileHelperStub);

                // Note: Results are for each A11yElement, not for each ScanStatus!
                AssertCompleteResult(result, 2, 1, 2, 1);
            }
        }

        private static void InitializeShims(
            bool shimTargetElementLocator = false,
            bool? elementBoundExceeded = null,
            bool shimUiFramework = false,
            bool? setTestModeSucceeds = null,
            bool shimScreenCapture = false,
            bool shimSnapshot = false,
            bool shimSarif = false,
            ShimSelectAction selectAction = null)
        {
            if (shimTargetElementLocator)
            {
                ShimTargetElementLocator.LocateRootElementInt32 = (processId) =>
                {
                    return new ElementContext(CreateA11yElement());
                };
            }

            if (selectAction != null)
            {
                ShimSelectAction.GetDefaultInstance = () => selectAction;
            }

            if (elementBoundExceeded.HasValue)
            {
                SetElementDataContext(elementBoundExceeded.Value);
            }

            if (shimUiFramework)
            {
                ShimGetDataAction.GetProcessAndUIFrameworkOfElementContextGuid = (_) => new Tuple<string, string>("one", "two");
            }

            if (setTestModeSucceeds.HasValue)
            {
                ShimCaptureAction.SetTestModeDataContextGuidDataContextModeTreeViewModeBoolean = (_, __, ___, _____) 
                    => setTestModeSucceeds.Value;
            }

            if (shimScreenCapture)
            {
                ShimScreenShotAction.CaptureScreenShotGuid = (_) => { };
            }

            if (shimSnapshot)
            {
                ShimSaveAction.SaveSnapshotZipStringGuidNullableOfInt32A11yFileModeDictionaryOfSnapshotMetaPropertyNameObject = (_, __, ___, ____, _____) => { };
            }

            if (shimSarif)
            {
                ShimSaveAction.SaveSarifFileStringGuidBoolean = (_, __, ___) => { };
            }
        }

        private static void SetElementDataContext(bool upperBoundExceeded)
        {
            BoundedCounter counter = new BoundedCounter(1);
            counter.TryIncrement();

            if (upperBoundExceeded)  // if true, we're 1 over capacity. if false, we're exactly at capacity
            {
                counter.TryIncrement();
            }

            ShimElementDataContext dc = new ShimElementDataContext
            {
                ElementCounterGet = () => counter
            };

            ShimGetDataAction.GetElementDataContextGuid = (_) => dc;
        }
#endif
    }
}
