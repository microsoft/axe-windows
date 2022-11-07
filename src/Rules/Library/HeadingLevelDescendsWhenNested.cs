// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.IntProperties;
using static Axe.Windows.Rules.PropertyConditions.Relationships;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.HeadingLevelDescendsWhenNested)]
    class HeadingLevelDescendsWhenNested : Rule
    {
        public HeadingLevelDescendsWhenNested()
        {
            Info.Description = Descriptions.HeadingLevelDescendsWhenNested;
            Info.HowToFix = HowToFix.HeadingLevelDescendsWhenNested;
            Info.Standard = A11yCriteriaId.InfoAndRelationships;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var condition = HeadingLevel > e.HeadingLevel;
            return !AnyAncestor(condition).Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return (HeadingLevel >= HeadingLevelType.HeadingLevel1) & (HeadingLevel <= HeadingLevelType.HeadingLevel9);
        }
    } // class
} // namespace
