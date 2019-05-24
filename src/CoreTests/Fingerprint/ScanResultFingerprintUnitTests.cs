// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Fingerprint;
using Axe.Windows.Core.Results;
using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Axe.Windows.CoreTests.Fingerprint
{
    [TestClass]
    public class ScanResultFingerprintUnitTests
    {
#pragma warning disable CS0414
        private readonly string _specialNameValue1;
        private readonly string _specialNameValue2;
#pragma warning restore CS0414
        private const RuleId DefaultRule = RuleId.ItemTypeCorrect;
        private const ScanStatus DefaultScanStatus = ScanStatus.Fail;
        private const string DefaultRuleIdValue = "ItemTypeCorrect";
        private const string DefaultLevelValue = "error";

        public ScanResultFingerprintUnitTests()
        {
#if FIND_SPECIAL_VALUES
            Dictionary<int, string> memory = new Dictionary<int, string>();

            while (true)
            {
                string value = Guid.NewGuid().ToString();

                FingerprintContribution fc = new FingerprintContribution("Name", value);
                int hash = fc.GetHashCode();

                if (memory.TryGetValue(hash, out string oldValue))
                {
                    if (oldValue == value)
                        continue;

                    _specialNameValue1 = oldValue;
                    _specialNameValue2 = value;
                    memory.Clear();
                    return;
                }
                memory.Add(hash, value);
            }
#else
            _specialNameValue1 = "96303c8f-5ff4-423b-a74d-bab12f37efb2";
            _specialNameValue2 = "f6c89020-5898-460d-974c-6227d48ed5e2";
#endif
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [Timeout(2000)]
        public void Ctor_ElementIsNull_ThrowsArgumentNullException()
        {
            try
            {
                new ScanResultFingerprint(null, DefaultRule, DefaultScanStatus);
            }
            catch (ArgumentNullException e)
            {
                Assert.AreEqual("element", e.ParamName);
                throw;
            }
        }

        static void ValidateAndRemoveContribution(Dictionary<string, FingerprintContribution> contributions,
            string keyName, string expectedValue)
        {
            if (string.IsNullOrEmpty(expectedValue))
                return;

            if (!contributions.TryGetValue(keyName, out FingerprintContribution contribution))
                Assert.Fail("Unable to find value for " + keyName);

            Assert.AreEqual(expectedValue, contribution.Value, "Value mismatch for key: " + keyName);

            contributions.Remove(keyName);
        }

        static void ValidateAndRemoveLevelContribution(Dictionary<string, FingerprintContribution> contributions,
            int level, string keyName, List<string> expectedValues)
        {
            if (level >= expectedValues.Count)
                return;

            string fullName = ((level > 0) ?
                string.Format(CultureInfo.InvariantCulture, "Ancestor{0}.", level) :
                string.Empty)
                + keyName;

            ValidateAndRemoveContribution(contributions, fullName, expectedValues[level]);
        }

        static List<string> GetAncestryTreeValues(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new List<string>();

            return input.Split(new char[] { '|' }).ToList();
        }

        static void ValidateFingerprint(IFingerprint fingerprint, string ruleIdValue = DefaultRuleIdValue,
            string nameValue = null, string classNameValue = null, string controlTypeValue = "Unknown(0)",
            string localizedControlTypeValue = null, string frameworkIdValue = null, string acceleratorKeyValue = null,
            string accessKeyValue = null, string cultureValue = null, string automationIdValue = null,
            string isControlElementValue = null, string isContentElementValue = null,
            string isKeyboardFocusableValue = null, string levelValue = DefaultLevelValue)
        {
            Dictionary<string, FingerprintContribution> contributions = new Dictionary<string, FingerprintContribution>();
            foreach (FingerprintContribution contribution in fingerprint.Contributions)
            {
                contributions.Add(contribution.Key, contribution);
            }

            ValidateAndRemoveContribution(contributions, "RuleId", ruleIdValue);
            ValidateAndRemoveContribution(contributions, "Level", levelValue);
            ValidateAndRemoveContribution(contributions, "IsControlElement", isControlElementValue);
            ValidateAndRemoveContribution(contributions, "IsContentElement", isContentElementValue);
            ValidateAndRemoveContribution(contributions, "IsKeyboardFocusable", isKeyboardFocusableValue);

            List<string> acceleratorKeyValues = GetAncestryTreeValues(acceleratorKeyValue);
            List<string> accessKeyValues = GetAncestryTreeValues(accessKeyValue);
            List<string> automationIdValues = GetAncestryTreeValues(automationIdValue);
            List<string> classNameValues = GetAncestryTreeValues(classNameValue);
            List<string> controlTypeValues = GetAncestryTreeValues(controlTypeValue);
            List<string> cultureValues = GetAncestryTreeValues(cultureValue);
            List<string> frameworkIdValues = GetAncestryTreeValues(frameworkIdValue);
            List<string> localizedControlTypeValues = GetAncestryTreeValues(localizedControlTypeValue);
            List<string> nameValues = GetAncestryTreeValues(nameValue);

            int expectedLevels = Math.Max(acceleratorKeyValues.Count,
                Math.Max(accessKeyValues.Count,
                Math.Max(automationIdValues.Count,
                Math.Max(classNameValues.Count,
                Math.Max(controlTypeValues.Count,
                Math.Max(frameworkIdValues.Count,
                Math.Max(localizedControlTypeValues.Count, nameValues.Count)))))));

            int oldCount = contributions.Count;
            int level;
            bool exitLoop = false;

            const int levelLimit = 10;

            Assert.IsTrue(expectedLevels < levelLimit, "Levels exceeds test expectations");
            for (level = 0; level < levelLimit; level++)
            {
                // exit at top of loop so that level is correctly set
                if (exitLoop)
                    break;

                ValidateAndRemoveLevelContribution(contributions, level, "AcceleratorKey", acceleratorKeyValues);
                ValidateAndRemoveLevelContribution(contributions, level, "AccessKey", accessKeyValues);
                ValidateAndRemoveLevelContribution(contributions, level, "AutomationId", automationIdValues);
                ValidateAndRemoveLevelContribution(contributions, level, "ClassName", classNameValues);
                ValidateAndRemoveLevelContribution(contributions, level, "ControlType", controlTypeValues);
                ValidateAndRemoveLevelContribution(contributions, level, "Culture", cultureValues);
                ValidateAndRemoveLevelContribution(contributions, level, "FrameworkId", frameworkIdValues);
                ValidateAndRemoveLevelContribution(contributions, level, "LocalizedControlType", localizedControlTypeValues);
                ValidateAndRemoveLevelContribution(contributions, level, "Name", nameValues);

                // Set exit flag if we have no more contributions or if we removed no properties this iteration
                if (contributions.Count == 0 || (oldCount == contributions.Count))
                {
                    exitLoop = true;
                    continue;
                }

                oldCount = contributions.Count;
            }
            Assert.AreEqual(0, contributions.Count, "Extra contibution keys: " + string.Join(",", contributions.Keys));
            Assert.AreEqual(level, expectedLevels, "Exited before reaching end of expected data");
        }

        [TestMethod]
        [Timeout(2000)]
        public void ValidateTestMethod_GetAncestryTreeValues()
        {
            List<string> values = GetAncestryTreeValues(null);
            Assert.AreEqual(0, values.Count);

            values = GetAncestryTreeValues("abc|defgh|||ijkl");
            Assert.AreEqual(5, values.Count);
            Assert.AreEqual("abc", values[0]);
            Assert.AreEqual("defgh", values[1]);
            Assert.AreEqual(string.Empty, values[2]);
            Assert.AreEqual(string.Empty, values[3]);
            Assert.AreEqual("ijkl", values[4]);
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_ElementIsTrivial_FingerprintIncludesRuleId()
        {
            using (A11yElement element = new A11yElement())
            {
                IFingerprint fingerprint = new ScanResultFingerprint(element, DefaultRule, DefaultScanStatus);
                ValidateFingerprint(fingerprint);
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_ScanStatusIsFail_FingerprintIncludesCorrectLevel()
        {
            using (A11yElement element = new A11yElement())
            {
                IFingerprint fingerprint = new ScanResultFingerprint(element, DefaultRule, ScanStatus.Fail);
                ValidateFingerprint(fingerprint, levelValue: "error");
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_ScanStatusIsNoResult_FingerprintIncludesCorrectLevel()
        {
            using (A11yElement element = new A11yElement())
            {
                IFingerprint fingerprint = new ScanResultFingerprint(element, DefaultRule, ScanStatus.NoResult);
                ValidateFingerprint(fingerprint, levelValue: "open");
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_ScanStatusIsPass_FingerprintIncludesCorrectLevel()
        {
            using (A11yElement element = new A11yElement())
            {
                IFingerprint fingerprint = new ScanResultFingerprint(element, DefaultRule, ScanStatus.Pass);
                ValidateFingerprint(fingerprint, levelValue: "pass");
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_ScanStatusIsScanNotSupported_FingerprintIncludesCorrectLevel()
        {
            using (A11yElement element = new A11yElement())
            {
                IFingerprint fingerprint = new ScanResultFingerprint(element, DefaultRule, ScanStatus.ScanNotSupported);
                ValidateFingerprint(fingerprint, levelValue: "note");
            }
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_ScanStatusIsUncertain_FingerprintIncludesCorrectLevel()
        {
            using (A11yElement element = new A11yElement())
            {
                IFingerprint fingerprint = new ScanResultFingerprint(element, DefaultRule, ScanStatus.Uncertain);
                ValidateFingerprint(fingerprint, levelValue: "open");
            }
        }

        private IA11yElement BuildTestElement(string name = null, string className = null,
            string localizedControlType = null, string framework = null,
            string acceleratorKey = null, string accessKey = null,
            string automationId = null, int controlTypeId = 0,
            IA11yElement parent = null, Rectangle? boundingRect = null,
            bool isKeyboardFocusable = false, bool isControlElement = false,
            bool isContentElement = false, string culture = null)
        {
            Mock<ICoreA11yElement> mockElement = new Mock<ICoreA11yElement>();

            Rectangle boundingRectangle = boundingRect.HasValue ? boundingRect.Value : new Rectangle();

            mockElement.Setup(x => x.AcceleratorKey).Returns(acceleratorKey);
            mockElement.Setup(x => x.AccessKey).Returns(accessKey);
            mockElement.Setup(x => x.AutomationId).Returns(automationId);
            mockElement.Setup(x => x.BoundingRectangle).Returns(boundingRectangle);
            mockElement.Setup(x => x.ClassName).Returns(className);
            mockElement.Setup(x => x.Culture).Returns(culture);
            mockElement.Setup(x => x.ControlTypeId).Returns(controlTypeId);
            mockElement.Setup(x => x.Framework).Returns(framework);
            mockElement.Setup(x => x.IsContentElement).Returns(isContentElement);
            mockElement.Setup(x => x.IsControlElement).Returns(isControlElement);
            mockElement.Setup(x => x.IsKeyboardFocusable).Returns(isKeyboardFocusable);
            mockElement.Setup(x => x.LocalizedControlType).Returns(localizedControlType);
            mockElement.Setup(x => x.Name).Returns(name);
            mockElement.Setup(x => x.Parent).Returns(parent);

            return mockElement.Object;
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_ElementHasName_FingerprintIncludesName()
        {
            const string elementName = "MyElement";

            IA11yElement element = BuildTestElement(name: elementName);

            IFingerprint fingerprint = new ScanResultFingerprint(element, DefaultRule, DefaultScanStatus);
            ValidateFingerprint(fingerprint, nameValue: elementName);
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_ElementHasClassName_FingerprintIncludesClassName()
        {
            const string className = "MyClass";

            IA11yElement element = BuildTestElement(className: className);

            IFingerprint fingerprint = new ScanResultFingerprint(element, DefaultRule, DefaultScanStatus);
            ValidateFingerprint(fingerprint, classNameValue: className);
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_ElementHasLocalizedControlType_FingerprintIncludesLocalizedControlType()
        {
            const string localizedControlType = "MyLocalizedControlType";

            IA11yElement element = BuildTestElement(localizedControlType: localizedControlType);

            IFingerprint fingerprint = new ScanResultFingerprint(element, DefaultRule, DefaultScanStatus);
            ValidateFingerprint(fingerprint, localizedControlTypeValue: localizedControlType);
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_ElementHasUIFramework_FingerprintIncludesFrameworkId()
        {
            const string frameworkId = "MyFrameworkId";

            IA11yElement element = BuildTestElement(framework: frameworkId);

            IFingerprint fingerprint = new ScanResultFingerprint(element, DefaultRule, DefaultScanStatus);
            ValidateFingerprint(fingerprint, frameworkIdValue: frameworkId);
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_ElementHasAcceleratorKey_FingerprintIncludesAcceleratorKey()
        {
            const string acceleratorKey = "MyAcceleratorKey";

            IA11yElement element = BuildTestElement(acceleratorKey: acceleratorKey);

            IFingerprint fingerprint = new ScanResultFingerprint(element, DefaultRule, DefaultScanStatus);
            ValidateFingerprint(fingerprint, acceleratorKeyValue: acceleratorKey);
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_ElementHasAccessKey_FingerprintIncludesAccessKey()
        {
            const string accessKey = "MyAccessKey";

            IA11yElement element = BuildTestElement(accessKey: accessKey);

            IFingerprint fingerprint = new ScanResultFingerprint(element, DefaultRule, DefaultScanStatus);
            ValidateFingerprint(fingerprint, accessKeyValue: accessKey);
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_ElementHasAutomationId_FingerprintIncludesAutomationId()
        {
            const string automationId = "MyAutomationId";

            IA11yElement element = BuildTestElement(automationId: automationId);

            IFingerprint fingerprint = new ScanResultFingerprint(element, DefaultRule, DefaultScanStatus);
            ValidateFingerprint(fingerprint, automationIdValue: automationId);
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_ElementHasOneLevelOfAncestry_FingerprintRemovesTopParentName()
        {
            const string elementName = "NameOfElement";
            const string parentName = "NameOfParent";
            const string desktopName = "NameOfDesktop";

            IA11yElement element =
                BuildTestElement(controlTypeId: 50033, name: elementName, parent:
                BuildTestElement(controlTypeId: 50031, name: parentName, parent:
                BuildTestElement(name: desktopName)));

            IFingerprint fingerprint = new ScanResultFingerprint(element, DefaultRule, DefaultScanStatus);
            ValidateFingerprint(fingerprint, nameValue: elementName, controlTypeValue: "Pane(50033)|SplitButton(50031)");
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_ElementBasedOnWordNavPane_FingerprintIncludesExpectedFields()
        {
            IA11yElement element =
                BuildTestElement(boundingRect: new Rectangle(132, 406, 402 - 132, 1928 - 406), controlTypeId: 50033,
                    isKeyboardFocusable: true, localizedControlType: "pane", name: "Navigation", parent:
                BuildTestElement(boundingRect: new Rectangle(124, 335, 410 - 124, 1935 - 335), controlTypeId: 50021,
                    isKeyboardFocusable: false, localizedControlType: "tool bar", parent:
                BuildTestElement(boundingRect: new Rectangle(124, 335, 410 - 124, 1935 - 335), controlTypeId: 50033,
                    isKeyboardFocusable: true, localizedControlType: "pane", name: "MsoDockLeft", parent:
                BuildTestElement(boundingRect: new Rectangle(111, 13, 3010 - 111, 2013 - 13), controlTypeId: 50032,
                    isKeyboardFocusable: true, localizedControlType: "window", name: "Document 1 - Word", parent:
                BuildTestElement(boundingRect: new Rectangle(0, 0, 3000, 2000), controlTypeId: 50033,
                    isKeyboardFocusable: true, localizedControlType: "pane", name: "Desktop 1")))));

            IFingerprint fingerprint = new ScanResultFingerprint(element, DefaultRule, DefaultScanStatus);
            ValidateFingerprint(fingerprint,
                nameValue: "Navigation||MsoDockLeft",
                controlTypeValue: "Pane(50033)|ToolBar(50021)|Pane(50033)|Window(50032)",
                localizedControlTypeValue: "pane|tool bar|pane|window"
                );
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_RuleIsControlElementPropertyCorrect_FingerprintIncludesIsControlElement()
        {
            IA11yElement element = BuildTestElement(isControlElement: true);

            IFingerprint fingerprint = new ScanResultFingerprint(element, RuleId.IsControlElementPropertyCorrect, DefaultScanStatus);
            ValidateFingerprint(fingerprint, ruleIdValue: "IsControlElementPropertyCorrect", isControlElementValue: "True");
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_RuleIsContentElementPropertyCorrect_FingerprintIncludesIsContentElement()
        {
            IA11yElement element = BuildTestElement(isContentElement: false);

            IFingerprint fingerprint = new ScanResultFingerprint(element, RuleId.IsContentElementPropertyCorrect, DefaultScanStatus);
            ValidateFingerprint(fingerprint, ruleIdValue: "IsContentElementPropertyCorrect", isContentElementValue: "False");
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_RuleIsKeyboardFocusable_FingerprintIncludesIsKeyboardFocusable()
        {
            IA11yElement element = BuildTestElement(isKeyboardFocusable: true);

            IFingerprint fingerprint = new ScanResultFingerprint(element, RuleId.IsKeyboardFocusable, DefaultScanStatus);
            ValidateFingerprint(fingerprint, ruleIdValue: "IsKeyboardFocusable", isKeyboardFocusableValue: "True");
        }

        [TestMethod]
        [Timeout(2000)]
        public void Ctor_RuleIsKeyboardFocusableBasedOnPatterns_FingerprintIncludesIsKeyboardFocusable()
        {
            IA11yElement element = BuildTestElement();

            IFingerprint fingerprint = new ScanResultFingerprint(element, RuleId.IsKeyboardFocusableBasedOnPatterns, DefaultScanStatus);
            ValidateFingerprint(fingerprint, ruleIdValue: "IsKeyboardFocusableBasedOnPatterns", isKeyboardFocusableValue: "False");
        }

        [TestMethod]
        [Timeout(2000)]
        public void GetHashCode_MinimalContributions_ReturnsSameHashCodeMultipleTimes()
        {
            IFingerprint fingerprint = new ScanResultFingerprint(new A11yElement(), DefaultRule, DefaultScanStatus);

            int hash1 = fingerprint.GetHashCode();
            Assert.AreEqual(hash1, fingerprint.GetHashCode());
            Assert.AreEqual(hash1, fingerprint.GetHashCode());
            Assert.AreEqual(hash1, fingerprint.GetHashCode());
        }

        [TestMethod]
        [Timeout(2000)]
        public void GetHashCode_EquivalentMinimalContributions_ReturnsSameHashCode()
        {
            IFingerprint fingerprint1 = new ScanResultFingerprint(new A11yElement(), DefaultRule, DefaultScanStatus);
            IFingerprint fingerprint2 = new ScanResultFingerprint(new A11yElement(), DefaultRule, DefaultScanStatus);

            Assert.AreEqual(fingerprint1.GetHashCode(), fingerprint2.GetHashCode());
        }

        [TestMethod]
        [Timeout(2000)]
        public void GetHashCode_EquivalentComplexContributions_ReturnsSameHashCode()
        {
            const string name = "ElementName";
            const string culture = "ElementCulture";
            const string automationId = "ElementAutomationId";

            IA11yElement element1 = BuildTestElement(name: name, culture: culture, automationId: automationId);
            IA11yElement element2 = BuildTestElement(name: name, culture: culture, automationId: automationId);

            IFingerprint fingerprint1 = new ScanResultFingerprint(element1, DefaultRule, DefaultScanStatus);
            IFingerprint fingerprint2 = new ScanResultFingerprint(element2, DefaultRule, DefaultScanStatus);

            Assert.AreEqual(fingerprint1.GetHashCode(), fingerprint2.GetHashCode());
        }

        [TestMethod]
        [Timeout(2000)]
        public void GetHashCode_DifferentInRuleOnly_ReturnsDifferentHashCode()
        {
            IFingerprint fingerprint1 = new ScanResultFingerprint(new A11yElement(), RuleId.BoundingRectangleExists, DefaultScanStatus);
            IFingerprint fingerprint2 = new ScanResultFingerprint(new A11yElement(), RuleId.BoundingRectangleNotNull, DefaultScanStatus);
            Assert.AreNotEqual(fingerprint1.GetHashCode(), fingerprint2.GetHashCode());
        }

        [TestMethod]
        [Timeout(2000)]
        public void GetHashCode_DifferentInLevelOnly_ReturnsDifferentHashCode()
        {
            IFingerprint fingerprint1 = new ScanResultFingerprint(new A11yElement(), DefaultRule, ScanStatus.Fail);
            IFingerprint fingerprint2 = new ScanResultFingerprint(new A11yElement(), DefaultRule, ScanStatus.Uncertain);
            Assert.AreNotEqual(fingerprint1.GetHashCode(), fingerprint2.GetHashCode());
        }

        [TestMethod]
        [Timeout(2000)]
        public void GetHashCode_DifferentComplexContributions_ReturnsDifferentHashCode()
        {
            const string name = "ElementName";
            const string culture = "ElementCulture";

            IA11yElement element1 = BuildTestElement(name: name, culture: culture, automationId: "id1");
            IA11yElement element2 = BuildTestElement(name: name, culture: culture, automationId: "id2");

            IFingerprint fingerprint1 = new ScanResultFingerprint(element1, DefaultRule, DefaultScanStatus);
            IFingerprint fingerprint2 = new ScanResultFingerprint(element2, DefaultRule, DefaultScanStatus);

            Assert.AreNotEqual(fingerprint1.GetHashCode(), fingerprint2.GetHashCode());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [Timeout (2000)]
        public void CompareTo_OtherIsNull_ThrowsArgumentNullException()
        {
            IFingerprint fingerprint = new ScanResultFingerprint(new A11yElement(), DefaultRule, DefaultScanStatus);

            try
            {
                fingerprint.CompareTo(null);
            }
            catch (ArgumentNullException e)
            {
                Assert.AreEqual("other", e.ParamName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        [Timeout(2000)]
        public void CompareTo_OtherIsDifferentImplemention_ThrowsInvalidOperationException()
        {
            IFingerprint fingerprint = new ScanResultFingerprint(BuildTestElement(), DefaultRule, DefaultScanStatus);

            IFingerprint other = new Mock<IFingerprint>().Object;

            fingerprint.CompareTo(other);
        }

        [TestMethod]
        [Timeout (2000)]
        public void CompareTo_OneElementHasMoreContributions_SortByContributionCount()
        {
            IA11yElement moreContributionsElement = BuildTestElement(name: "ElementWith2Contributions", automationId: "blah");
            IA11yElement fewerContributionsElement = BuildTestElement(name: "ElementWith1Contribution");

            IFingerprint higherFingerprint = new ScanResultFingerprint(moreContributionsElement, DefaultRule, DefaultScanStatus);
            IFingerprint lowerFingerprint = new ScanResultFingerprint(fewerContributionsElement, DefaultRule, DefaultScanStatus);

            Assert.AreEqual(-1, lowerFingerprint.CompareTo(higherFingerprint));
            Assert.AreEqual(1, higherFingerprint.CompareTo(lowerFingerprint));
        }

        [TestMethod]
        [Timeout (2000)]
        public void CompareTo_ElementsWithSameNumberOfContributions_SortByHashValue()
        {
            var list = new SortedList<IFingerprint, string>();
            const int max = 10;

            string elementName = string.Empty;
            for (int loop = 0; loop < max; loop++)
            {
                elementName += "a";
                IA11yElement element = BuildTestElement(name: elementName);
                IFingerprint fingerprint = new ScanResultFingerprint(element, DefaultRule, DefaultScanStatus);
                list.Add(fingerprint, elementName);
            }

            // If the comparison worked correctly, then the hash codes of the list will be 
            // sorted in ascending order
            int oldHashCode = int.MinValue;

            for (int loop = 0; loop < max; loop++)
            {
                int hashCode = list.Keys[loop].GetHashCode();
                string description = string.Format("Index {0}: Hash {1} should be less than Hash {2}",
                    loop, oldHashCode, hashCode);
                Assert.IsTrue(oldHashCode < hashCode);
                oldHashCode = hashCode;
            }
        }

        [TestMethod]
        [Timeout (2000)]
        public void Equals_OtherIsNull_ReturnsFalse()
        {
            IFingerprint fingerprint = new ScanResultFingerprint(new A11yElement(), DefaultRule, DefaultScanStatus);
            Assert.IsFalse(fingerprint.Equals(null));
        }

        [TestMethod]
        [Timeout (2000)]
        public void Equals_OtherIsDifferentImplementation_ReturnsFalse()
        {
            IFingerprint fingerprint = new ScanResultFingerprint(new A11yElement(), DefaultRule, DefaultScanStatus);
            IFingerprint other = new Mock<IFingerprint>().Object;
            Assert.IsFalse(fingerprint.Equals(other));
        }

        [TestMethod]
        [Timeout (2000)]
        public void Equals_OtherHasDifferentNumberOfContributions_ReturnsFalse()
        {
            const string name = "MyTest";
            IA11yElement moreContributionsElement = BuildTestElement(name: name, automationId: "myId");
            IA11yElement fewerContributionsElement = BuildTestElement(name: name);

            IFingerprint fingerprint1 = new ScanResultFingerprint(moreContributionsElement, DefaultRule, DefaultScanStatus);
            IFingerprint fingerprint2 = new ScanResultFingerprint(fewerContributionsElement, DefaultRule, DefaultScanStatus);

            Assert.AreNotEqual(fingerprint1.Contributions.Count(), fingerprint2.Contributions.Count());
            Assert.IsFalse(fingerprint1.Equals(fingerprint2));
            Assert.IsFalse(fingerprint2.Equals(fingerprint1));
        }

        [TestMethod]
        [Timeout(2000)]
        public void Equals_OtherHasSameNumberOfContributionsButDifferentHash_ReturnsFalse()
        {
            IA11yElement element1 = BuildTestElement(name: "a");
            IA11yElement element2 = BuildTestElement(name: "A");

            IFingerprint fingerprint1 = new ScanResultFingerprint(element1, DefaultRule, DefaultScanStatus);
            IFingerprint fingerprint2 = new ScanResultFingerprint(element2, DefaultRule, DefaultScanStatus);

            Assert.AreEqual(fingerprint1.Contributions.Count(), fingerprint2.Contributions.Count());
            Assert.IsFalse(fingerprint1.Equals(fingerprint2));
            Assert.IsFalse(fingerprint2.Equals(fingerprint1));
        }

        [TestMethod]
        [Timeout (2000)]
        public void Equals_OtherIsEquivalent_ReturnsTrue()
        {
            const string automationId = "AutomationId";
            const int controlTypeId = 50021;
            IA11yElement element1 = BuildTestElement(automationId: automationId, controlTypeId: controlTypeId);
            IA11yElement element2 = BuildTestElement(automationId: automationId, controlTypeId: controlTypeId);

            IFingerprint fingerprint1 = new ScanResultFingerprint(element1, DefaultRule, DefaultScanStatus);
            IFingerprint fingerprint2 = new ScanResultFingerprint(element2, DefaultRule, DefaultScanStatus);

            Assert.IsTrue(fingerprint1.Equals(fingerprint2));
            Assert.IsTrue(fingerprint2.Equals(fingerprint1));
        }

        [TestMethod]
        [Timeout (2000)]
        public void Equals_OtherHasSameContributionCountAndHashButDifferentContent_ReturnsFalse()
        {
            FingerprintContribution fc1 = new FingerprintContribution("Name", _specialNameValue1);
            FingerprintContribution fc2 = new FingerprintContribution("Name", _specialNameValue2);

            Assert.AreEqual(fc1.GetHashCode(), fc2.GetHashCode());

            IA11yElement element1 = BuildTestElement(_specialNameValue1);
            IA11yElement element2 = BuildTestElement(_specialNameValue2);
            IFingerprint fingerprint1 = new ScanResultFingerprint(element1, DefaultRule, DefaultScanStatus);
            IFingerprint fingerprint2 = new ScanResultFingerprint(element2, DefaultRule, DefaultScanStatus);

            Assert.AreEqual(fingerprint1.GetHashCode(), fingerprint2.GetHashCode());

            // Ensure that these differ only in specific content
            Assert.AreEqual(0, fingerprint1.CompareTo(fingerprint2));
            Assert.AreEqual(0, fingerprint2.CompareTo(fingerprint1));

            Assert.IsFalse(fingerprint1.Equals(fingerprint2));
            Assert.IsFalse(fingerprint2.Equals(fingerprint1));
        }

        [TestMethod]
        [Timeout (2000)]
        public void DictionaryTest_FingerprintsAreDifferent_TreatsItemsCorrectly()
        {
            Dictionary<IFingerprint, int> store = new Dictionary<IFingerprint, int>();

            IA11yElement element1 = BuildTestElement(name: _specialNameValue1);
            IA11yElement element2 = BuildTestElement(name: _specialNameValue2);
            IFingerprint fingerprint1 = new ScanResultFingerprint(element1, DefaultRule, DefaultScanStatus);
            IFingerprint fingerprint2 = new ScanResultFingerprint(element2, DefaultRule, DefaultScanStatus);

            store.Add(fingerprint1, 1);
            Assert.AreEqual(1, store.Count);
            Assert.IsTrue(store.TryGetValue(fingerprint1, out int value));
            Assert.AreEqual(1, value);
            Assert.IsFalse(store.TryGetValue(fingerprint2, out value));
            store.Add(fingerprint2, 2);
            Assert.AreEqual(2, store.Count);
            Assert.IsTrue(store.TryGetValue(fingerprint2, out value));
            Assert.AreEqual(2, value);
        }

        [TestMethod]
        [Timeout(2000)]
        public void DictionaryTest_FingerprintsAreSame_TreatsItemsCorrectly()
        {
            Dictionary<IFingerprint, int> store = new Dictionary<IFingerprint, int>();

            IFingerprint fingerprint1 = new ScanResultFingerprint(new A11yElement(), DefaultRule, DefaultScanStatus);
            IFingerprint fingerprint2 = new ScanResultFingerprint(new A11yElement(), DefaultRule, DefaultScanStatus);

            store.Add(fingerprint1, 1);
            Assert.AreEqual(1, store.Count);
            Assert.IsTrue(store.TryGetValue(fingerprint1, out int value));
            Assert.AreEqual(1, value);
            value = 0;
            Assert.IsTrue(store.TryGetValue(fingerprint2, out value));
            Assert.AreEqual(1, value);
            store[fingerprint2] = 3;
            Assert.IsTrue(store.TryGetValue(fingerprint1, out value));
            Assert.AreEqual(3, value);
        }
    }
}
