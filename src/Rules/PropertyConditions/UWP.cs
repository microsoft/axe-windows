// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using static Axe.Windows.Rules.PropertyConditions.Framework;
using static Axe.Windows.Rules.PropertyConditions.Relationships;
using static Axe.Windows.Rules.PropertyConditions.StringProperties;

namespace Axe.Windows.Rules.PropertyConditions
{
    static class UWP
    {
        public static Condition TopLevelElement = XAML & NotParent(XAML);
        public static Condition TitleBar = CreateTitleBarCondition();
        public static Condition MenuBar = CreateMenuBarCondition();

        private static Condition CreateTitleBarCondition()
        {
            var automationID = AutomationID.Is("TitleBar") | AutomationID.Is("TitleBarLeftButtons");
            var className = ClassName.Is("ApplicationFrameTitleBarWindow");
            return automationID & className & Win32Framework;
        }

        private static Condition CreateMenuBarCondition()
        {
            var automationID = AutomationID.Is("SystemMenuBar");
            return automationID & Parent(Win32Framework);
        }
    } // class
} // namespace
