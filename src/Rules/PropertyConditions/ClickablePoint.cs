// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.Extensions;
using Axe.Windows.Rules.Resources;
using System;
using System.Drawing;
using static Axe.Windows.Rules.Misc.Helpers;

namespace Axe.Windows.Rules.PropertyConditions
{
    /// <summary>
    /// Provides Conditions related to the ClickablePoint property of an IA11yElement
    /// </summary>
    static class ClickablePoint
    {
        public static Condition OnScreen = Condition.Create(IsClickablePointOnScreen, ConditionDescriptions.ClickablePointOnScreen);

        private static bool IsClickablePointOnScreen(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            if (!e.TryGetPropertyValue<Point>(PropertyType.UIA_ClickablePointPropertyId, out var clickablePoint)) return false;

            return Desktop.CachedBoundingRectangle.Contains(clickablePoint);
        }
    } // class
} // namespace
