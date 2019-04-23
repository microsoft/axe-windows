// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Text.RegularExpressions;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.Misc;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
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
        }

        public override EvaluationCode Evaluate(IA11yElement e)
        {
            if (e == null) throw new ArgumentException(nameof(e));
            if (String.IsNullOrWhiteSpace(e.Name)) throw new ArgumentException($"{nameof(e.Name)} was null or white space");

            if (!ControlTypeStrings.Dictionary.TryGetValue(e.ControlTypeId, out string controlTypeString)) throw new InvalidOperationException($"No control type entry was found in {nameof(ControlTypeStrings.Dictionary)}");

            var r = new Regex($@"\b{controlTypeString}\b", RegexOptions.IgnoreCase);

            return r.IsMatch(e.Name)
                ? EvaluationCode.Error : EvaluationCode.Pass;
        }

        protected override Condition CreateCondition()
        {
            return ~ElementGroups.AllowSameNameAndControlType
                & Name.NotNullOrEmpty
                & Name.NotWhiteSpace;
        }
    } // class
} // namespace
