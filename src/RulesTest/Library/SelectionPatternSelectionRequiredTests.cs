﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class SelectionPatternSelectionRequiredTests
    {
        private static readonly Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.SelectionPatternSelectionRequired();

        [TestMethod]
        public void TabControlWithSelectionPatternButNotEdgeFramework_Applicable()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_TabControlTypeId;
            e.Patterns.Add(new A11yPattern(e, PatternType.UIA_SelectionPatternId));
            e.Framework = FrameworkId.Win32;

            Assert.IsTrue(Rule.Condition.Matches(e));
        }
    } // class
} // namespace
