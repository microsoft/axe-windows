// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.CoreTests.Types
{
    [TestClass]
    public class PatternTypesTests
    {
        [TestMethod]
        public void CheckExists()
        {
            Assert.AreEqual(PatternType.GetInstance().Exists(PatternType.UIA_DockPatternId), true);
            Assert.AreEqual(PatternType.GetInstance().Exists(0), false);
        }
    }
}
