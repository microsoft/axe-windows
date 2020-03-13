// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EvaluationCode = Axe.Windows.Rules.EvaluationCode;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class IsOffScreenChildTests
    {
        private static Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.IsOffScreenChild();

        [TestMethod]
        public void IsOffScreenChild_Condition_Matches()
        {
            using (var e = new MockA11yElement())
            {
                e.IsOffScreen = false;
                Assert.IsTrue(Rule.Condition.Matches(e));
            } // using
        }

        [TestMethod]
        public void IsOffScreenChild_Condition_NoMatch()
        {
            using (var e = new MockA11yElement())
            {
                e.IsOffScreen = true;
                Assert.IsFalse(Rule.Condition.Matches(e));
            } // using
        }

        [TestMethod]
        public void IsOffScreenChild_Evaluate_Error()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                parent.IsOffScreen = true;
                e.Parent = parent;
                Assert.AreEqual(EvaluationCode.Error, Rule.Evaluate(e));
            } // using
        }

        [TestMethod]
        public void IsOffScreenChild_Evaluate_Pass()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                parent.IsOffScreen = false;
                e.Parent = parent;
                Assert.AreEqual(EvaluationCode.Pass, Rule.Evaluate(e));
            } // using
        }

        [TestMethod]
        public void IsOffScreenChild_Evaluate_ThrowsArgumentNullException()
        {
            var ex = Assert.ThrowsException<ArgumentNullException>(() => Rule.Evaluate(null));
            Assert.AreEqual("e", ex.ParamName);
        }
    } // class
} // namespace
