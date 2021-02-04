// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.Resources;
using System;
using System.Globalization;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.LocalizedLandmarkTypeExcludesPrivateUnicodeCharacters)]
    class LocalizedLandmarkTypeExcludesPrivateUnicodeCharacters : Rule
    {
        public LocalizedLandmarkTypeExcludesPrivateUnicodeCharacters()
        {
            this.Info.Description = string.Format(CultureInfo.CurrentCulture, Descriptions.PropertyExcludesPrivateUnicodeCharacters, LocalizedLandmarkType.PropertyDescription);
            this.Info.HowToFix = string.Format(CultureInfo.CurrentCulture, HowToFix.PropertyExcludesPrivateUnicodeCharacters, LocalizedLandmarkType.PropertyDescription);
            this.Info.Standard = A11yCriteriaId.InfoAndRelationships;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return LocalizedLandmarkType.ExcludesPrivateUnicodeCharacters.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return LocalizedLandmarkType.NotNullOrEmpty;
        }
    } // class
} // namespace
