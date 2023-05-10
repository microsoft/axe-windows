// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Rules.PropertyConditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace Axe.Windows.RulesTests.PropertyConditions
{
    [TestClass]
    public class SystemProperitesTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            SystemProperties.OverriddenCultureName = null;
        }

        [TestMethod]
        public void IsEnglish_DefaultInput_MatchesSystemLocale()
        {
            string cultureName = CultureInfo.CurrentCulture.Name;
            AssertConditionMatches(cultureName, IsSystemSetToEnglish(cultureName));
        }

        [TestMethod]
        public void IsEnglish_IsSimpleEnglish_CaseInsensitive_ReturnsTrue()
        {
            SetOverrideAndAssertConditionMatches("En", true);
        }

        [TestMethod]
        public void IsEnglish_IsUKEnglish_CaseInsensitive_ReturnsTrue()
        {
            SetOverrideAndAssertConditionMatches("eN-Uk", true);
        }

        [TestMethod]
        public void IsEnglish_IsGerman_ReturnsFalse()
        {
            SetOverrideAndAssertConditionMatches("de-DE", false);
        }

        [TestMethod]
        public void OverriddenCultureName_ResetsToDefault()
        {
            if (!IsSystemSetToEnglish(CultureInfo.CurrentCulture.Name))
            {
                Assert.Inconclusive("This test can only be run with English locale settings");
            }

            AssertConditionMatches("system default", true);
            SetOverrideAndAssertConditionMatches("de-DE", false);
            SystemProperties.OverriddenCultureName = null;
            AssertConditionMatches("system default", true);
        }

        private static bool IsSystemSetToEnglish(string cultureName)
        {
            return (cultureName.ToLowerInvariant() == "en" || cultureName.ToLowerInvariant().StartsWith("en-"));
        }

        private void SetOverrideAndAssertConditionMatches(string cultureName, bool expectsEnglish)
        {
            SystemProperties.OverriddenCultureName = cultureName;
            AssertConditionMatches(cultureName, expectsEnglish);
        }

        private void AssertConditionMatches(string cultureName, bool expectsEnglish)
        {
            if (expectsEnglish)
            {
                Assert.IsTrue(SystemProperties.IsEnglish.Matches(null), $"Expected {cultureName} to report true");
            }
            else
            {
                Assert.IsFalse(SystemProperties.IsEnglish.Matches(null), $"Expected {cultureName} to report false");
            }
        }
    } // class
} // namespace
