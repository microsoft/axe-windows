// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Rules.PropertyConditions
{
    static class SystemProperties
    {
        private static string SystemCultureName = System.Globalization.CultureInfo.CurrentCulture.Name;

        internal static string OverriddenCultureName { get; set; }

        public static Condition IsEnglish = CreateEnglishConditionWithTestOverride();

        private static Condition CreateEnglishConditionWithTestOverride()
        {
            StringProperty cultureName = new StringProperty(_ => (OverriddenCultureName ?? SystemCultureName).ToLowerInvariant());

            return new OrCondition(cultureName.IsNoCase("en"), cultureName.MatchesRegEx("^en-.*"));
        }
    } // class
} // namespace
