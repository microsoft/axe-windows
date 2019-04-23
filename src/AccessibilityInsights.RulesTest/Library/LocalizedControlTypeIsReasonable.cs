// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Axe.Windows.RulesTest.ControlType;
using EvaluationCode = Axe.Windows.Rules.EvaluationCode;

namespace Axe.Windows.RulesTest.Library
{
    [TestClass]
    public class LocalizedControlTypeIsReasonable
    {
        private Axe.Windows.Rules.IRule Rule = new Axe.Windows.Rules.Library.LocalizedControlTypeIsReasonable();

        [TestMethod]
        public void TestControlTypeIdIsAppBarAndLocalizedControlTypeIsAppBar()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_AppBarControlTypeId;
            e.LocalizedControlType = "app bar";

            Assert.AreEqual(EvaluationCode.Pass, this.Rule.Evaluate(e));
        }

        [TestMethod]
        public void TestControlTypeIdIsAppBarAndLocalizedControlTypeIsNotAppBar()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_AppBarControlTypeId;
            e.LocalizedControlType = "custom";

            Assert.AreEqual(EvaluationCode.Warning, this.Rule.Evaluate(e));
        }

        [TestMethod]
        public void TestControlTypeIdIsCustomControl()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = Axe.Windows.Core.Types.ControlType.UIA_CustomControlTypeId;
            Assert.IsFalse(this.Rule.Condition.Matches(e));
        }

        [TestMethod]
        public void LocalizedControlTypeIsReasonable_True_MultipleOptions()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = Hyperlink;
            e.LocalizedControlType = "hyperlink"; 
            Assert.AreEqual(EvaluationCode.Pass, Rule.Evaluate(e));

            e.LocalizedControlType = "link";
            Assert.AreEqual(EvaluationCode.Pass, Rule.Evaluate(e));
        }

        [TestMethod]
        public void LocalizedControlTypeIsReasonable_True_CaseInsensitive()
        {
            var e = new MockA11yElement();
            e.ControlTypeId = Hyperlink;
            e.LocalizedControlType = "Hyperlink";
            Assert.AreEqual(EvaluationCode.Pass, Rule.Evaluate(e));
        }
    } // class
} // LocalizedControlTypespace
