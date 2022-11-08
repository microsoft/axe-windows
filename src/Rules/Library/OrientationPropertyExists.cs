// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.IntProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.OrientationPropertyExists)]
    class OrientationPropertyExists : Rule
    {
        public OrientationPropertyExists()
        {
            Info.Description = Descriptions.OrientationPropertyExists;
            Info.HowToFix = HowToFix.OrientationPropertyExists;
            Info.Standard = A11yCriteriaId.ObjectInformation;
            Info.PropertyID = PropertyType.UIA_OrientationPropertyId;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return Orientation.Exists.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return ScrollBar | Tab;
        }
    } // class
} // namespace
