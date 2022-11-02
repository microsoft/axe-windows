// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.HelpTextNotEqualToName)]
    class HelpTextNotEqualToName : Rule
    {
        public HelpTextNotEqualToName()
        {
            Info.Description = Descriptions.HelpTextNotEqualToName;
            Info.HowToFix = HowToFix.HelpTextNotEqualToName;
            Info.Standard = A11yCriteriaId.ObjectInformation;
            Info.ErrorCode = EvaluationCode.Warning;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var condition = ~(Name.IsEqualTo(HelpText));

            return condition.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return Name.NotNullOrEmpty & HelpText.NotNullOrEmpty;
        }
    } // class
} // namespace
