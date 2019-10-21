// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Rules;
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
                    return "Section 508 502.3.10";
                case A11yCriteriaId.InfoAndRelationships:
                    return "WCAG 1.3.1";
                case A11yCriteriaId.Keyboard:
                    return "WCAG 2.1.1";
                case A11yCriteriaId.NameRoleValue:
                    return "WCAG 4.1.2";
                case A11yCriteriaId.ObjectInformation:
                    return "Section 508 502.3.1";
            }

            return "none";
        }
    } // class
} // namespace
