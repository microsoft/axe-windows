// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class NameIsReasonableLengthTests
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.NameIsReasonableLength();

        [TestMethod]
        public void TestNameIsReasonableLengthTrue()
        {
            using (var e = new MockA11yElement())
            {
                e.Name = "Hello";
                Assert.IsTrue(Rule.PassesTest(e));
            } // using
        }

        [TestMethod]
        public void TestNameIsReasonableLengthFalse()
        {
            using (var e = new MockA11yElement())
            {
                StringBuilder s = new StringBuilder("*");
                for (var i = 0; i < 10; ++i)
                    s.Append(s.ToString() + s.ToString());

                e.Name = s.ToString();
                Assert.IsFalse(Rule.PassesTest(e));
            } // using
        }

        [TestMethod]
        public void TestNullElement()
        {
            Action action = () => { Rule.PassesTest(null); };
            Assert.ThrowsException<ArgumentNullException>(action);
        }

        [TestMethod]
        public void TestNullName()
        {
            using (var e = new MockA11yElement())
            {
                e.LocalizedControlType = "Button";
                Action action = () => { Rule.PassesTest(e); };
                Assert.ThrowsException<ArgumentException>(action);
            } // using
        }

        [TestMethod]
        public void NameIsReasonableLength_TextElement_NotApplicable()
        {
            using (var e = new MockA11yElement())
            {
                e.ControlTypeId = ControlType.Text;
                Assert.IsFalse(Rule.Condition.Matches(e));
            } // using
        }
    } // class
} // namespace
