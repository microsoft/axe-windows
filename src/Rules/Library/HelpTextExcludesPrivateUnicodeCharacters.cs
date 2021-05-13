// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.Resources;
using System;
using System.Globalization;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.HelpTextExcludesPrivateUnicodeCharacters)]
    class HelpTextExcludesPrivateUnicodeCharacters : Rule
    {
        public HelpTextExcludesPrivateUnicodeCharacters()
        {
            this.Info.Description = string.Format(CultureInfo.CurrentCulture, Descriptions.PropertyExcludesPrivateUnicodeCharacters, HelpText.PropertyDescription);
            this.Info.HowToFix = string.Format(CultureInfo.CurrentCulture, HowToFix.PropertyExcludesPrivateUnicodeCharacters, HelpText.PropertyDescription);
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = PropertyType.UIA_HelpTextPropertyId;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (string.IsNullOrWhiteSpace(e.HelpText)) throw new ArgumentException(ErrorMessages.ElementHelpTextNullOrWhiteSpace, nameof(e));

            return HelpText.ExcludesPrivateUnicodeCharacters.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return HelpText.NotNullOrWhiteSpace;
        }
    } // class
} // namespace
