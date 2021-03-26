// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ControlShouldNotSupportTablePattern)]
    class ControlShouldNotSupportTablePattern : Rule
    {
        public ControlShouldNotSupportTablePattern()
        {
            this.Info.Description = Descriptions.ControlShouldNotSupportTablePattern;
            this.Info.HowToFix = HowToFix.ControlShouldNotSupportTablePattern;
            this.Info.Standard = A11yCriteriaId.AvailableActions;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return !Patterns.Table.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            // This rule is based on documentation at https://docs.microsoft.com/en-us/windows/win32/winauto/uiauto-supportlistcontroltype
            // But we don't check Win32 lists because we know they will fail and the framework is no longer supported

            return List
                & ~ClassName.Is("SysListView32");
        }
    } // class
} // namespace
