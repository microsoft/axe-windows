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
            SystemProperties.OverriddenISOLanguageName = null;
        }

        [TestMethod]
        public void IsEnglish_DefaultInput_MatchesSystemLocale()
        {
            string languageName = CultureInfo.CurrentCulture.ThreeLetterISOLanguageName;
            AssertConditionMatches(languageName, IsSystemSetToEnglish(languageName));
        }

        [TestMethod]
        public void IsEnglish_English_CaseInsensitive_ReturnsTrue()
        {
            SetOverrideAndAssertConditionMatches("EnG", true);
        }

        [TestMethod]
        public void IsEnglish_IsGerman_ReturnsFalse()
        {
            SetOverrideAndAssertConditionMatches("deu", false);
        }

        [TestMethod]
        public void OverriddenCultureName_ResetsToDefault()
        {
            if (!IsSystemSetToEnglish(CultureInfo.CurrentCulture.ThreeLetterISOLanguageName))
            {
                Assert.Inconclusive("This test can only be run with English locale settings");
            }

            AssertConditionMatches("system default", true);
            SetOverrideAndAssertConditionMatches("deu", false);
            SystemProperties.OverriddenISOLanguageName = null;
            AssertConditionMatches("system default", true);
        }

        private static bool IsSystemSetToEnglish(string languageName)
        {
            return string.Equals(languageName, "eng", System.StringComparison.OrdinalIgnoreCase);
        }

        private void SetOverrideAndAssertConditionMatches(string languageName, bool expectsEnglish)
        {
            SystemProperties.OverriddenISOLanguageName = languageName;
            AssertConditionMatches(languageName, expectsEnglish);
        }

        private void AssertConditionMatches(string languageName, bool expectsEnglish)
        {
            if (expectsEnglish)
            {
                Assert.IsTrue(SystemProperties.IsEnglish.Matches(null), $"Expected {languageName} to report true");
            }
            else
            {
                Assert.IsFalse(SystemProperties.IsEnglish.Matches(null), $"Expected {languageName} to report false");
            }
        }
    } // class
} // namespace
