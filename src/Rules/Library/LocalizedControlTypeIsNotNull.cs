// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.LocalizedControlTypeNotNull)]
    class LocalizedControlTypeIsNotNull : Rule
    {
        public LocalizedControlTypeIsNotNull()
        {
            Info.Description = Descriptions.LocalizedControlTypeNotNull;
            Info.HowToFix = HowToFix.LocalizedControlTypeNotNull;
            Info.Standard = A11yCriteriaId.ObjectInformation;
            Info.PropertyID = PropertyType.UIA_LocalizedControlTypePropertyId;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return e.LocalizedControlType != null;
        }

        protected override Condition CreateCondition()
        {
            return IsKeyboardFocusable & ~UWP.TitleBar;
        }
    } // class
} // namespace
