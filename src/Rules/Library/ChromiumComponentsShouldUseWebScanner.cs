// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.Framework;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ChromiumComponentsShouldUseWebScanner)]
    class ChromiumComponentsShouldUseWebScanner : Rule
    {
        public override bool IsExclusionRule => true;

        public ChromiumComponentsShouldUseWebScanner()
        {
            Info.Description = Descriptions.ChromiumComponentsShouldUseWebScanner;
            Info.HowToFix = HowToFix.ChromiumComponentsShouldUseWebScanner;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            return false;
        }

        protected override Condition CreateCondition()
        {
            return Chrome;
        }

        public override bool IncludeInResults(IA11yElement element)
        {
            // Only include a failure for the top level element
            return element.Parent == null || !element.Parent.Framework.Equals(FrameworkId.Chrome);
        }
    } // class
} // namespace
