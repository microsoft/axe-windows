// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Rules
{
    /// <summary>
    /// This class contains settings that influence the behavior of the rules engine. It is currently
    /// static for simplicity. This is a potential problem in the scenario of concurrent scans where the
    /// settings change in the middle of a scan. Given the very limited scope of the current settings,
    /// we are choosing to accept this limitation for now.
    /// </summary>
    internal static class RulesSettings
    {
        /// <summary>
        /// This setting controls whether the rules engine should test all Chromium content. This exists
        /// to allow browser teams to debug code that converts from HTML to UIA. It should be set to false
        /// in all other scenarios.
        /// </summary>
        internal static bool ShouldTestAllChromiumContent { get; set; }
    }
}
