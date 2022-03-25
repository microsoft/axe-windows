// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class NameIsInformative
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.NameIsInformative();

        private static MockA11yElement CreateMatchingElement()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = ControlType.ListItem;
            e.Name = "Mando";
            e.BoundingRectangle = new System.Drawing.Rectangle(0, 0, 25, 25);

            return e;
        }

        [TestMethod]
        public void NameMatchesDotNetType()
        {
            using (var e = new MockA11yElement())
            {
                string[] stringsToTry =
                    {
                    "Microsoft.xxx.xxx",
                    "Microsoft.x",
                    "Windows.xxx.xxx",
                    "Windows.abc"
                };

                foreach (var s in stringsToTry)
                {
                    e.Name = s;
                    Assert.IsFalse(Rule.PassesTest(e));
                }
            } // using
        }

        [TestMethod]
        public void NameDoesNotMatchDotNetType()
        {
            using (var e = new MockA11yElement())
            {
                string[] stringsToTry =
                    {
                    " Microsoft",
                    "Microsoft. ",
                    "Microsoft.*",
                    " Windows",
                    "Windows. ",
                    "Windows.*",
                };

                foreach (var s in stringsToTry)
                {
                    e.Name = s;
                    Assert.IsTrue(Rule.PassesTest(e));
                }
            } // using
        }

        [TestMethod]
        public void MatchesExpectedElement()
        {
            var e = CreateMatchingElement();
            Assert.IsTrue(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void NameNull_DoesNotMatch()
        {
            var e = CreateMatchingElement();
            e.Name = null;
            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void NameEmpty_DoesNotMatch()
        {
            var e = CreateMatchingElement();
            e.Name = string.Empty;
            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void NameWhiteSpace_DoesNotMatch()
        {
            var e = CreateMatchingElement();
            e.Name = "   ";
            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void BoundingRectInvalid_DoesNotMatch()
        {
            var e = CreateMatchingElement();
            e.BoundingRectangle = new System.Drawing.Rectangle();
            Assert.IsFalse(Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void FrameworkWind32_DoesNotMatch()
        {
            var e = CreateMatchingElement();
            e.Framework = Core.Enums.FrameworkId.Win32;
            Assert.IsFalse(Rule.Condition.Matches(e));
        }
    } // class
} // namespace
