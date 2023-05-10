// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Rules.PropertyConditions
{
    static class SystemProperties
    {
        private static readonly string SystemISOLanguageName = System.Globalization.CultureInfo.CurrentCulture.ThreeLetterISOLanguageName;

        internal static string OverriddenISOLanguageName { get; set; }

        public static Condition IsEnglish = CreateEnglishConditionWithTestOverride();

        private static Condition CreateEnglishConditionWithTestOverride()
        {
            StringProperty cultureName = new StringProperty(_ => (OverriddenISOLanguageName ?? SystemISOLanguageName));

            return cultureName.IsNoCase("eng");
        }
    } // class
} // namespace
