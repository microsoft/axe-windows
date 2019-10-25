// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Rules;
using Axe.Windows.RuleSelection.Resources;
using System;

namespace RulesMD
{
    static class Helpers
    {
        public static string GetStandardName(A11yCriteriaId criteriaId)
        {
            switch (criteriaId)
            {
                case A11yCriteriaId.AvailableActions:
                    return DefaultGuidelineShortDescriptions.AvailableActions;
                case A11yCriteriaId.InfoAndRelationships:
                    return DefaultGuidelineShortDescriptions.InfoAndRelationships;
                case A11yCriteriaId.Keyboard:
                    return DefaultGuidelineShortDescriptions.Keyboard;
                case A11yCriteriaId.NameRoleValue:
                    return DefaultGuidelineShortDescriptions.NameRoleValue;
                case A11yCriteriaId.ObjectInformation:
                    return DefaultGuidelineShortDescriptions.ObjectInformation;
            }

            return DefaultGuidelineShortDescriptions.None;
        }
    } // class
} // namespace
