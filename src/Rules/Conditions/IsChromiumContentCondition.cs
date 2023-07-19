// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Rules.Resources;

using static Axe.Windows.Rules.PropertyConditions.ElementGroups;
using static Axe.Windows.Rules.PropertyConditions.Relationships;

namespace Axe.Windows.Rules
{
    /// <summary>
    /// Determines is the element is an element within the context of a Chromium document
    /// </summary>
    class IsChromiumContentCondition : Condition
    {
        private readonly Condition _isChromiumElement = IsChromiumDocument | AnyAncestor(IsChromiumDocument);

        public IsChromiumContentCondition()
        {
        }

        public override bool Matches(IA11yElement element)
        {
            return _isChromiumElement.Matches(element);
        }

        public override string ToString()
        {
            return ConditionDescriptions.IsChromiumContent;
        }
    } // class
} // namespace
