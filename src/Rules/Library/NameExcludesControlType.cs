// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.Misc;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using System.Text.RegularExpressions;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.NameExcludesControlType)]
    class NameExcludesControlType : Rule
    {
        public NameExcludesControlType()
        {
            this.Info.Description = Descriptions.NameExcludesControlType;
            this.Info.HowToFix = HowToFix.NameExcludesControlType;
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.PropertyID = PropertyType.UIA_NamePropertyId;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (string.IsNullOrWhiteSpace(e.Name)) throw new ArgumentException(ErrorMessages.ElementNameNullOrWhiteSpace, nameof(e));

            if (!ControlTypeStrings.Dictionary.TryGetValue(e.ControlTypeId, out string controlTypeString)) throw new InvalidOperationException(ErrorMessages.NoControlTypeEntryFound);

            var r = new Regex($@"\b{controlTypeString}\b", RegexOptions.IgnoreCase);

            return !r.IsMatch(e.Name);
        }

        protected override Condition CreateCondition()
        {
            return ~ElementGroups.AllowSameNameAndControlType
                & Name.NotNullOrEmpty
                & Name.NotWhiteSpace;
        }
    } // class
} // namespace
