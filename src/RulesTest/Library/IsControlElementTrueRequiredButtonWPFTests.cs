// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Drawing;

namespace Axe.Windows.RulesTests.Library
{
    [TestClass]
    public class IsControlElementTrueRequiredButtonWPFTests
    {
        private static readonly Rectangle ValidRectangle = new Rectangle(100, 200, 300, 400); // Arbitrary values
        private const int NonRequiredControlType = Axe.Windows.Core.Types.ControlType.UIA_PaneControlTypeId;
        private const int GenericRequiredControlType = Axe.Windows.Core.Types.ControlType.UIA_ComboBoxControlTypeId;
        private const int TextType = Axe.Windows.Core.Types.ControlType.UIA_TextControlTypeId;
        private const int ButtonType = Axe.Windows.Core.Types.ControlType.UIA_ButtonControlTypeId;

        private static Rules.IRule Rule = new Rules.Library.IsControlElementTrueRequiredButtonWPF();
        private Mock<IA11yElement> _elementMock;
        private Mock<IA11yElement> _parentMock;

        [TestInitialize]
        public void BeforeEach()
        {
            _elementMock = new Mock<IA11yElement>(MockBehavior.Strict);
            _parentMock = new Mock<IA11yElement>(MockBehavior.Strict);
        }

        private void VerifyAllMocks()
        {
            _elementMock.VerifyAll();
            _parentMock.VerifyAll();
        }

        [TestMethod]
        public void FrameworkIssueLink_IsNotNull()
        {
            Assert.IsNotNull(Rule.Info.FrameworkIssueLink);
        }

        [TestMethod]
        public void Condition_EmptyBoundingRectangle_ReturnsFalse()
        {
            _elementMock.Setup(m => m.BoundingRectangle).Returns(Rectangle.Empty);

            Assert.IsFalse(Rule.Condition.Matches(_elementMock.Object));

            VerifyAllMocks();
        }

        [TestMethod]
        public void Condition_ControlTypeDoesNotRequireControlElement_ReturnsFalse()
        {
            _elementMock.Setup(m => m.BoundingRectangle).Returns(ValidRectangle);
            _elementMock.Setup(m => m.ControlTypeId).Returns(NonRequiredControlType);

            Assert.IsFalse(Rule.Condition.Matches(_elementMock.Object));

            VerifyAllMocks();
        }

        [TestMethod]
        public void Condition_ControlTypeIsXamlText_ReturnsFalse()
        {
            _elementMock.Setup(m => m.BoundingRectangle).Returns(ValidRectangle);
            _elementMock.Setup(m => m.ControlTypeId).Returns(TextType);
            _elementMock.Setup(m => m.Framework).Returns("XAML");

            Assert.IsFalse(Rule.Condition.Matches(_elementMock.Object));

            VerifyAllMocks();
        }

        [TestMethod]
        public void Condition_ControlTypeIsWpfButton_ReturnsTrue()
        {
            _elementMock.Setup(m => m.BoundingRectangle).Returns(ValidRectangle);
            _elementMock.Setup(m => m.ControlTypeId).Returns(ButtonType);
            _elementMock.Setup(m => m.Framework).Returns("WPF");

            Assert.IsTrue(Rule.Condition.Matches(_elementMock.Object));

            VerifyAllMocks();
        }

        [TestMethod]
        public void Condition_ControlTypeIsGenericRequiredType_ReturnsFalse()
        {
            _elementMock.Setup(m => m.BoundingRectangle).Returns(ValidRectangle);
            _elementMock.Setup(m => m.ControlTypeId).Returns(GenericRequiredControlType);
            _elementMock.Setup(m => m.Framework).Returns("A framework");

            Assert.IsFalse(Rule.Condition.Matches(_elementMock.Object));

            VerifyAllMocks();
        }

        [TestMethod]
        public void PassesTest_ElementIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException e = Assert.ThrowsException<ArgumentNullException>(() => Rule.PassesTest(null));

            VerifyAllMocks();
        }

        [TestMethod]
        public void PassesTest_IsControlElementIsFalse_ReturnsFalse()
        {
            _elementMock.Setup(m => m.IsControlElement).Returns(false);

            Assert.IsFalse(Rule.PassesTest(_elementMock.Object));

            VerifyAllMocks();
        }

        [TestMethod]
        public void PassesTest_IsControlElementIsTrue_ReturnsTrue()
        {
            _elementMock.Setup(m => m.IsControlElement).Returns(true);

            Assert.IsTrue(Rule.PassesTest(_elementMock.Object));

            VerifyAllMocks();
        }
    }
}
