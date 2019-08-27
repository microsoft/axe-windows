// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.Misc;
using Axe.Windows.Rules.Resources;
using System;
using System.Globalization;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.LocalizedControlTypeExcludesPrivateUnicodeCharacters)]
    class LocalizedControlTypeExcludesPrivateUnicodeCharacters : Rule
    {
        public LocalizedControlTypeExcludesPrivateUnicodeCharacters()
        {
            this.Info.Description = string.Format(CultureInfo.CurrentCulture, Descriptions.PropertyExcludesPrivateUnicodeCharacters, LocalizedControlType.PropertyDescription);
            this.Info.HowToFix = string.Format(CultureInfo.CurrentCulture, HowToFix.PropertyExcludesPrivateUnicodeCharacters, LocalizedControlType.PropertyDescription);
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = PropertyType.UIA_LocalizedControlTypePropertyId;
        }

        public override EvaluationCode Evaluate(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (String.IsNullOrWhiteSpace(e.LocalizedControlType)) throw new ArgumentException(ErrorMessages.ElementLocalizedControlTypeNullOrWhiteSpace, nameof(e));

            return LocalizedControlType.ExcludesPrivateUnicodeCharacters.Matches(e) ? EvaluationCode.Pass : EvaluationCode.Error;
        }

        protected override Condition CreateCondition()
        {
            return LocalizedControlType.NotNullOrWhiteSpace;
        }
    } // class
} // namespace
