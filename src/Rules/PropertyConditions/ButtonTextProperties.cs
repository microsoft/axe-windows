// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;

using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.PropertyConditions
{
    static class ButtonTextProperties
    {
        public static Condition IsTextInButtonWithDifferentName = CreateIsTextInButtonWithDifferentName();

        private static Condition CreateIsTextInButtonWithDifferentName()
        {
            return Condition.Create(e => MatchIsTextInButtonWithDifferentName(e));
        }

        private static bool MatchIsTextInButtonWithDifferentName(IA11yElement e)
        {
            if (!Text.Matches(e))
                return false;

            IA11yElement parentButton = GetNearestButtonAncestor(e);

            if (parentButton == null)
                return false;

            return parentButton.Name != e.Name;
        }

        private static IA11yElement GetNearestButtonAncestor(IA11yElement e)
        {
            IA11yElement parent = e.Parent;
            while(parent != null)
            {
                if (Button.Matches(parent))
                {
                    return parent;
                }
                parent = parent.Parent;
            }

            return null;
        }
    }
}
