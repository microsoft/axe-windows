// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Enums;

namespace Axe.Windows.Rules.PropertyConditions
{
    static class Framework
    {
        public static readonly Condition DirectUI = StringProperties.Framework.Is(FrameworkId.DirectUI);
        public static readonly Condition Edge = StringProperties.Framework.Is(FrameworkId.Edge);
        // The following name includes "Framework" to avoid clashing with the Win32 namespace
        public static readonly Condition Win32Framework = StringProperties.Framework.Is(FrameworkId.Win32);
        public static readonly Condition WinForms = StringProperties.Framework.Is(FrameworkId.WinForm);
        public static readonly Condition WPF = StringProperties.Framework.Is(FrameworkId.WPF);
        public static readonly Condition XAML = StringProperties.Framework.Is(FrameworkId.XAML);
    } // class
} // namespace
