// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.Resources;
using System.Text.RegularExpressions;
using static Axe.Windows.Rules.PropertyConditions.Framework;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.FrameworkDoesNotSupportUIAutomation)]
    class FrameworkDoesNotSupportUIAutomation : Rule
    {
        private readonly static string[] KnownProblematiClasses =
        {
            @"^\s*SunAwt.*$",
        };

        public FrameworkDoesNotSupportUIAutomation ()
        {
            this.Info.Description = Descriptions.FrameworkDoesNotSupportUIAutomation;
            this.Info.HowToFix = HowToFix.FrameworkDoesNotSupportUIAutomation;
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            string className = e.ClassName;

            foreach (var knownProblematicClass in KnownProblematiClasses)
            {
                if (Regex.IsMatch(className, knownProblematicClass, RegexOptions.CultureInvariant))
                    return false;
            }

            return true;
        }

        protected override Condition CreateCondition()
        {
            return Win32Framework;
        }
    }
}
