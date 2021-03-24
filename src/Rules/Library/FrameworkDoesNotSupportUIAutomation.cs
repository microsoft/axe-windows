// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.Resources;
using System.Linq;
using static Axe.Windows.Rules.PropertyConditions.Framework;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.FrameworkDoesNotSupportUIAutomation)]
    class FrameworkDoesNotSupportUIAutomation : Rule
    {
        private readonly static string[] KnownBadClasses =
        {
            "SunAwtFrame",
        };

        public FrameworkDoesNotSupportUIAutomation ()
        {
            this.Info.Description = Descriptions.EditSupportsIncorrectRangeValuePattern;
            this.Info.HowToFix = HowToFix.EditSupportsIncorrectRangeValuePattern;
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement element)
        {
            string className = element.ClassName;

            return !KnownBadClasses.Contains(className);
        }

        protected override Condition CreateCondition()
        {
            return Win32Framework;
        }
    }
}
