// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.Resources;
using System;
using System.Globalization;
using static Axe.Windows.Rules.PropertyConditions.ButtonTextProperties;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.NameExcludesPrivateUnicodeCharacters)]
    class NameExcludesPrivateUnicodeCharacters : Rule
    {
        public NameExcludesPrivateUnicodeCharacters()
        {
            this.Info.Description = string.Format(CultureInfo.CurrentCulture, Descriptions.PropertyExcludesPrivateUnicodeCharacters, Name.PropertyDescription);
            this.Info.HowToFix = string.Format(CultureInfo.CurrentCulture, HowToFix.PropertyExcludesPrivateUnicodeCharacters, Name.PropertyDescription);
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = PropertyType.UIA_NamePropertyId;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (string.IsNullOrWhiteSpace(e.Name)) throw new ArgumentException(ErrorMessages.ElementNameNullOrWhiteSpace, nameof(e));

            return Name.ExcludesPrivateUnicodeCharacters.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return Name.NotNullOrWhiteSpace - IsTextInButtonWithDifferentName;
        }
    } // class
} // namespace
