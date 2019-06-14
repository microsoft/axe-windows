// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Automation;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
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
        private IScanResultsAssembler ResultsAssemblerStub = new Axe.Windows.Automation.Fakes.StubScanResultsAssembler();
#endif

        private static A11yElement CreateA11yElement(List<A11yElement> children = null, List<ScanStatus> statuses = null)
        {
            A11yElement element = new A11yElement();

            if (children != null)
                element.Children = children;

            if (statuses != null)
            {
                ScanResult scanResult = new ScanResult
                {
                    Items = new List<RuleResult>(),
                };

                foreach (ScanStatus status in statuses)
                {
                    scanResult.Items.Add(new RuleResult {Status = status, Rule = RuleId.NameNotNull});
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
                    SnapshotCommand.Execute(config, OutputFileHelperStub, ResultsAssemblerStub);
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
                    SnapshotCommand.Execute(config, OutputFileHelperStub, ResultsAssemblerStub);
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
                    SnapshotCommand.Execute(config, OutputFileHelperStub, ResultsAssemblerStub);
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
                    SnapshotCommand.Execute(config, OutputFileHelperStub, ResultsAssemblerStub);
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
                    SnapshotCommand.Execute(config, OutputFileHelperStub, ResultsAssemblerStub);
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
                var resultsAssembler = new ScanResultsAssembler();

                    var results = SnapshotCommand.Execute(config, OutputFileHelperStub, resultsAssembler);

                    Assert.AreEqual(0, results.ErrorCount);
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
                var resultsAssembler = new ScanResultsAssembler();

                var results = SnapshotCommand.Execute(config, OutputFileHelperStub, resultsAssembler);

                Assert.AreEqual(0, results.ErrorCount);
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
                    selectAction: selectAction, elementBoundExceeded: false, shimUiFramework: true,
                    setTestModeSucceeds: true, shimScreenCapture: true, shimSnapshot: true, shimSarif: true);

                var config = Config.Builder.ForProcessId(-1).Build();
                var resultsAssembler = new ScanResultsAssembler();
                Axe.Windows.Automation.ScanResults results = null;

                try
                {
                    results = SnapshotCommand.Execute(config, OutputFileHelperStub, resultsAssembler);
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }

                Assert.AreEqual(2, results.ErrorCount);
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
