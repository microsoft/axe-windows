// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Rules.PropertyConditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Axe.Windows.RulesTests.ControlType;

namespace Axe.Windows.RulesTests.PropertyConditions
{
    [TestClass]
    public class ButtonTextPropertiesTests
    {
        private Mock<IA11yElement> _elementMock;

        [TestInitialize]
        public void BeforeEach()
        {
            _elementMock = new Mock<IA11yElement>(MockBehavior.Strict);
        }

        [TestMethod]
        public void Matches_ElementIsNotTextBlock_ReturnsFalse()
        {
            _elementMock.Setup(m => m.ControlTypeId).Returns(Button);
            Assert.IsFalse(ButtonTextProperties.IsTextInButtonWithDifferentName.Matches(_elementMock.Object));
        }

        [TestMethod]
        public void Matches_NoButtonAncestorExists_ReturnsFalse()
        {
            Mock<IA11yElement> parentMock = new Mock<IA11yElement>(MockBehavior.Strict);

            parentMock.Setup(m => m.ControlTypeId).Returns(Table);
            parentMock.Setup(m => m.Parent).Returns((IA11yElement)null);
            
            _elementMock.Setup(m => m.ControlTypeId).Returns(Text);
            _elementMock.Setup(m => m.Parent).Returns(parentMock.Object);

            Assert.IsFalse(ButtonTextProperties.IsTextInButtonWithDifferentName.Matches(_elementMock.Object));
        }

        [TestMethod]
        public void Matches_ButtonAncestorExists_NamesMatch_ReturnsFalse()
        {
            const string matchingName = "This is a matching name";
            CreateTestHierarchyWithNames(matchingName, matchingName);

            Assert.IsFalse(ButtonTextProperties.IsTextInButtonWithDifferentName.Matches(_elementMock.Object));
        }

        [TestMethod]
        public void Matches_ButtonAncestorExists_NamesMatch_ReturnsTrue()
        {
            CreateTestHierarchyWithNames("TextBlock name", "Button name");

            Assert.IsTrue(ButtonTextProperties.IsTextInButtonWithDifferentName.Matches(_elementMock.Object));
        }

        private void CreateTestHierarchyWithNames(string textBlockName, string ancestorName)
        {
            Mock<IA11yElement> parentMock = new Mock<IA11yElement>(MockBehavior.Strict);
            Mock<IA11yElement> grandparentMock = new Mock<IA11yElement>(MockBehavior.Strict);

            _elementMock.Setup(m => m.ControlTypeId).Returns(Text);
            _elementMock.Setup(m => m.Name).Returns(textBlockName);
            _elementMock.Setup(m => m.Parent).Returns(parentMock.Object);

            parentMock.Setup(m => m.ControlTypeId).Returns(Table);
            parentMock.Setup(m => m.Parent).Returns(grandparentMock.Object);

            grandparentMock.Setup(m => m.ControlTypeId).Returns(Button);
            grandparentMock.Setup(m => m.Name).Returns(ancestorName);
        }
    } // class
} // namespace
