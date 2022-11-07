// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.RulesTests.Library
{
    public class LandmarkIsTopLevelTests
    {
        private readonly Axe.Windows.Rules.IRule _rule = null;
        private readonly int _landmarkType = 0;
        private readonly string _localizedLandmarkType = null;

        protected LandmarkIsTopLevelTests(object rule, int landmarkType, string localizedLandmarkType)
        {
            // we must pass in an object because the IRule type is not exposed publicly and it causes a compiler error
            _rule = (Axe.Windows.Rules.IRule)rule;
            _landmarkType = landmarkType;
            _localizedLandmarkType = localizedLandmarkType;
        }

        [TestMethod]
        public void LandmarkIsTopLevel_Pass()
        {
            var e = new MockA11yElement();
            var parent = new MockA11yElement();
            e.LandmarkType = _landmarkType;
            e.LocalizedLandmarkType = _localizedLandmarkType;
            e.Parent = parent;

            Assert.IsTrue(_rule.PassesTest(e));
        }

        [TestMethod]
        public void LandmarkIsTopLevel_Error()
        {
            var e = new MockA11yElement();
            var parent = new MockA11yElement();
            e.LandmarkType = _landmarkType;
            e.LocalizedLandmarkType = _localizedLandmarkType;
            e.Parent = parent;
            parent.LandmarkType = _landmarkType;
            parent.LocalizedLandmarkType = _localizedLandmarkType;

            Assert.IsFalse(_rule.PassesTest(e));
        }
    } // class

    [TestClass]
    public class LandmarkMainIsTopLevelTests : LandmarkIsTopLevelTests
    {
        public LandmarkMainIsTopLevelTests()
            : base(new Axe.Windows.Rules.Library.LandmarkMainIsTopLevel(), LandmarkType.UIA_MainLandmarkTypeId, null)
        { }
    } // class

    [TestClass]
    public class LandmarkBannerIsTopLevelTests : LandmarkIsTopLevelTests
    {
        public LandmarkBannerIsTopLevelTests()
            : base(new Axe.Windows.Rules.Library.LandmarkBannerIsTopLevel(), LandmarkType.UIA_CustomLandmarkTypeId, "banner")
        { }
    } // class

    [TestClass]
    public class LandmarkContentInfoIsTopLevelTests : LandmarkIsTopLevelTests
    {
        public LandmarkContentInfoIsTopLevelTests()
            : base(new Axe.Windows.Rules.Library.LandmarkContentInfoIsTopLevel(), LandmarkType.UIA_CustomLandmarkTypeId, "contentinfo")
        { }
    } // class

    [TestClass]
    public class LandmarkComplementaryIsTopLevelTests : LandmarkIsTopLevelTests
    {
        public LandmarkComplementaryIsTopLevelTests()
            : base(new Axe.Windows.Rules.Library.LandmarkComplementaryIsTopLevel(), LandmarkType.UIA_CustomLandmarkTypeId, "complementary")
        { }
    } // class
} // namespace
