// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Globalization;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ControlViewStatusBarStructure)]
    class ControlViewStatusBarStructure: Rule
    {
        public ControlViewStatusBarStructure()
        {
            this.Info.Description = string.Format(CultureInfo.InvariantCulture, Descriptions.Structure, ControlView.StatusBarStructure);
            this.Info.HowToFix = string.Format(CultureInfo.InvariantCulture, HowToFix.Structure, ControlView.StatusBarStructure);
            this.Info.Standard = A11yCriteriaId.InfoAndRelationships;
            this.Info.ErrorCode = EvaluationCode.Note;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return ControlView.StatusBarStructure.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return StatusBar;
        }
    } // class
} // namespace
