// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Rules.PropertyConditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using static Axe.Windows.RulesTests.ControlType;

namespace Axe.Windows.RulesTests.PropertyConditions
{
    [TestClass]
    public class ElementGroupsTests
    {
        private readonly int[] _allowSameNameAndControlTypeTypes = null;
        private readonly IEnumerable<int> _disallowSameNameAndControlTypeTypes = null;

        public ElementGroupsTests()
        {
            _allowSameNameAndControlTypeTypes = new int[] { AppBar, Custom, Header, MenuBar, SemanticZoom, StatusBar, TitleBar, Text };
            _disallowSameNameAndControlTypeTypes = ControlType.All.Difference(_allowSameNameAndControlTypeTypes);
        }

        [TestMethod]
        public void AllowSameNameAndControlType_True_BasedOnType()
        {
            var e = new MockA11yElement();

            foreach (var t in _allowSameNameAndControlTypeTypes)
            {
                e.ControlTypeId = t;
                Assert.IsTrue(ElementGroups.AllowSameNameAndControlType.Matches(e));
            } // for each type
        }

        [TestMethod]
        public void AllowSameNameAndControlType_False_ContainsDisallowedType()
        {
            var e = new MockA11yElement();

            foreach (var t in _disallowSameNameAndControlTypeTypes)
            {
                e.ControlTypeId = t;
                Assert.IsFalse(ElementGroups.AllowSameNameAndControlType.Matches(e));
            } // for each type
        }

        [TestMethod]
        public void AllowSameNameAndControlType_True_ExceedsMaxLength()
        {
            var e = new MockA11yElement();

            e.ControlTypeId = _disallowSameNameAndControlTypeTypes.First();

            var tenChars = "1234567890";
            for (int i = 0; i < 5; ++i)
                e.Name += tenChars;

            Assert.IsFalse(ElementGroups.AllowSameNameAndControlType.Matches(e));

            e.Name += "1";

            Assert.IsTrue(ElementGroups.AllowSameNameAndControlType.Matches(e));
        }

        [TestMethod]
        public void WPFScrollBarPageUpButton_True()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                parent.ControlTypeId = ControlType.ScrollBar;
                e.IsOffScreen = false;
                e.ControlTypeId = ControlType.Button;
                e.Framework = "WPF";
                e.AutomationId = "PageUp";
                parent.Children.Add(e);
                e.Parent = parent;

                Assert.IsTrue(ElementGroups.WPFScrollBarPageButtons.Matches(e));
            } // using
        }

        [TestMethod]
        public void WPFScrollBarPageDownButton_True()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                parent.ControlTypeId = ControlType.ScrollBar;
                e.ControlTypeId = ControlType.Button;
                e.Framework = "WPF";
                e.AutomationId = "PageDown";
                parent.Children.Add(e);
                e.Parent = parent;

                Assert.IsTrue(ElementGroups.WPFScrollBarPageButtons.Matches(e));
            } // using
        }

        [TestMethod]
        public void WPFScrollBarPageLeftButton_True()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                parent.ControlTypeId = ControlType.ScrollBar;
                e.ControlTypeId = ControlType.Button;
                e.Framework = "WPF";
                e.AutomationId = "PageLeft";
                parent.Children.Add(e);
                e.Parent = parent;

                Assert.IsTrue(ElementGroups.WPFScrollBarPageButtons.Matches(e));
            } // using
        }

        [TestMethod]
        public void WPFScrollBarPageRightButton_True()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                parent.ControlTypeId = ControlType.ScrollBar;
                e.ControlTypeId = ControlType.Button;
                e.Framework = "WPF";
                e.AutomationId = "PageRight";
                parent.Children.Add(e);
                e.Parent = parent;

                Assert.IsTrue(ElementGroups.WPFScrollBarPageButtons.Matches(e));
            } // using
        }

        [TestMethod]
        public void WPFScrollBarPageButtons_NotWPF_False()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                parent.ControlTypeId = ControlType.ScrollBar;
                e.ControlTypeId = ControlType.Button;
                // e.Framework = "WPF";
                e.AutomationId = "PageUp";
                parent.Children.Add(e);
                e.Parent = parent;

                Assert.IsFalse(ElementGroups.WPFScrollBarPageButtons.Matches(e));
            } // using
        }

        [TestMethod]
        public void WPFScrollBarPageButtons_NotButton_False()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                parent.ControlTypeId = ControlType.ScrollBar;
                // e.ControlTypeId = ControlType.Button;
                e.Framework = "WPF";
                e.AutomationId = "PageUp";
                parent.Children.Add(e);
                e.Parent = parent;

                Assert.IsFalse(ElementGroups.WPFScrollBarPageButtons.Matches(e));
            } // using
        }

        [TestMethod]
        public void WPFScrollBarPageButtons_ParentNotScrollBar_False()
        {
            // This test essentially also covers the possibility of no parent.

            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                // parent.ControlTypeId = ControlType.ScrollBar;
                e.ControlTypeId = ControlType.Button;
                e.Framework = "WPF";
                e.AutomationId = "PageUp";
                parent.Children.Add(e);
                e.Parent = parent;

                Assert.IsFalse(ElementGroups.WPFScrollBarPageButtons.Matches(e));
            } // using
        }

        [TestMethod]
        public void WPFScrollBarPageButtons_NotPageUpOrPageDown_False()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                parent.ControlTypeId = ControlType.ScrollBar;
                e.ControlTypeId = ControlType.Button;
                e.Framework = "WPF";
                // e.AutomationId = "PageUp";
                parent.Children.Add(e);
                e.Parent = parent;

                Assert.IsFalse(ElementGroups.WPFScrollBarPageButtons.Matches(e));
            } // using
        }

        [TestMethod]
        public void WPFDataGridCell_MatchExpected()
        {
            using (var e = new MockA11yElement())
            {
                Assert.IsFalse(ElementGroups.WPFDataGridCell.Matches(e));

                e.Framework = "WPF";
                Assert.IsFalse(ElementGroups.WPFDataGridCell.Matches(e));

                e.ClassName = "DataGridCell";
                Assert.IsTrue(ElementGroups.WPFDataGridCell.Matches(e));

                e.Framework = string.Empty;
                Assert.IsFalse(ElementGroups.WPFDataGridCell.Matches(e));
            } // using
        }

        [TestMethod]
        public void IsButtonText_MatchExpected()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                Assert.IsFalse(ElementGroups.IsButtonText.Matches(e));

                e.ControlTypeId = Window;
                Assert.IsFalse(ElementGroups.IsButtonText.Matches(e));

                e.ControlTypeId = Text;
                Assert.IsFalse(ElementGroups.IsButtonText.Matches(e));

                e.Parent = parent;
                parent.ControlTypeId = Window;
                Assert.IsFalse(ElementGroups.IsButtonText.Matches(e));

                parent.ControlTypeId = Button;
                Assert.IsTrue(ElementGroups.IsButtonText.Matches(e));

                e.ControlTypeId = Window;
                Assert.IsFalse(ElementGroups.IsButtonText.Matches(e));
            } // using
        }

        [TestMethod]
        public void IsCheckBoxText_MatchExpected()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                e.ControlTypeId = Button;
                Assert.IsFalse(ElementGroups.IsCheckBoxText.Matches(e));

                e.ControlTypeId = Text;
                Assert.IsFalse(ElementGroups.IsCheckBoxText.Matches(e));

                e.Parent = parent;
                parent.ControlTypeId = CheckBox;
                Assert.IsTrue(ElementGroups.IsCheckBoxText.Matches(e));

                e.ControlTypeId = Button;
                Assert.IsFalse(ElementGroups.IsCheckBoxText.Matches(e));
            } // using
        }

        [TestMethod]
        public void IsWPFCheckBoxText_MatchExpected()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                Assert.IsFalse(ElementGroups.IsWPFCheckBoxText.Matches(e));

                // Exhaustive checks for IsCheckBoxText is handled in
                // IsCheckBoxText_MatchExpected, so just need 1 true case
                parent.ControlTypeId = CheckBox;
                e.Parent = parent;
                e.ControlTypeId = Text;
                Assert.IsFalse(ElementGroups.IsWPFCheckBoxText.Matches(e));

                e.Framework = "WPF";
                Assert.IsTrue(ElementGroups.IsWPFCheckBoxText.Matches(e));

                e.Parent = null;
                Assert.IsFalse(ElementGroups.IsWPFCheckBoxText.Matches(e));
            } // using
        }

        [TestMethod]
        public void ListXAML_MatchExpected()
        {
            using (var e = new MockA11yElement())
            {
                Assert.IsFalse(ElementGroups.ListXAML.Matches(e));

                e.ControlTypeId = List;
                Assert.IsFalse(ElementGroups.ListXAML.Matches(e));

                e.Framework = "XAML";
                Assert.IsTrue(ElementGroups.ListXAML.Matches(e));

                e.ControlTypeId = ListItem;
                Assert.IsFalse(ElementGroups.ListXAML.Matches(e));
            } // using

        }

        [TestMethod]
        public void HyperlinkInTextXAML_MatchExpected()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                Assert.IsFalse(ElementGroups.HyperlinkInTextXAML.Matches(e));

                e.ControlTypeId = Hyperlink;
                Assert.IsFalse(ElementGroups.HyperlinkInTextXAML.Matches(e));

                e.Framework = "XAML";
                Assert.IsFalse(ElementGroups.HyperlinkInTextXAML.Matches(e));

                e.Parent = parent;
                Assert.IsFalse(ElementGroups.HyperlinkInTextXAML.Matches(e));

                parent.ControlTypeId = Text;
                Assert.IsTrue(ElementGroups.HyperlinkInTextXAML.Matches(e));

                e.ControlTypeId = Text;
                Assert.IsFalse(ElementGroups.HyperlinkInTextXAML.Matches(e));

                e.ControlTypeId = Hyperlink;
                e.Framework = "WPF";
                Assert.IsFalse(ElementGroups.HyperlinkInTextXAML.Matches(e));
            } // using
        }

        [TestMethod]
        public void WPFButton_MatchExpected()
        {
            using (var e = new MockA11yElement())
            {
                Assert.IsFalse(ElementGroups.WPFButton.Matches(e));

                e.ControlTypeId = Button;
                Assert.IsFalse(ElementGroups.WPFButton.Matches(e));

                e.Framework = "WPF";
                Assert.IsTrue(ElementGroups.WPFButton.Matches(e));

                e.Framework = "AnythingElse";
                Assert.IsFalse(ElementGroups.WPFButton.Matches(e));
            } // using
        }

        [TestMethod]
        public void XAMLTextInEdit_MatchExpected()
        {
            using (var e = new MockA11yElement())
            using (var parent = new MockA11yElement())
            {
                Assert.IsFalse(ElementGroups.XAMLTextInEdit.Matches(e));

                e.ControlTypeId = Text;
                Assert.IsFalse(ElementGroups.XAMLTextInEdit.Matches(e));

                parent.ControlTypeId = Edit;
                e.Parent = parent;
                Assert.IsFalse(ElementGroups.XAMLTextInEdit.Matches(e));

                e.Framework = "XAML";
                Assert.IsTrue(ElementGroups.XAMLTextInEdit.Matches(e));

                e.ControlTypeId = ComboBox;
                Assert.IsFalse(ElementGroups.XAMLTextInEdit.Matches(e));

                parent.ControlTypeId = ComboBox;
                Assert.IsFalse(ElementGroups.XAMLTextInEdit.Matches(e));

                e.Parent = null;
                Assert.IsFalse(ElementGroups.XAMLTextInEdit.Matches(e));
            } // using
        }

        [TestMethod]
        public void WinFormsEdit_MatchExpected()
        {
            using (var e = new MockA11yElement())
            {
                Assert.IsFalse(ElementGroups.WinFormsEdit.Matches(e));

                e.ControlTypeId = Edit;
                Assert.IsFalse(ElementGroups.WinFormsEdit.Matches(e));

                e.Framework = "WinForm";
                Assert.IsTrue(ElementGroups.WinFormsEdit.Matches(e));
            } // using
        }
    } // class
} // namespace
