// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.Types;
using System;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for SelectionItem Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671286(v=vs.85).aspx
    /// </summary>
    [PatternEvent(Id = EventType.UIA_SelectionItem_ElementAddedToSelectionEventId)]
    [PatternEvent(Id = EventType.UIA_SelectionItem_ElementRemovedFromSelectionEventId)]
    [PatternEvent(Id = EventType.UIA_SelectionItem_ElementSelectedEventId)]
    public class SelectionItemPattern : A11yPattern
    {
        IUIAutomationSelectionItemPattern Pattern;

        public SelectionItemPattern(A11yElement e, IUIAutomationSelectionItemPattern p) : base(e, PatternType.UIA_SelectionItemPatternId)
        {
            Pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "IsSelected", Value = Convert.ToBoolean(Pattern.CurrentIsSelected) });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod(IsUIAction = true)]
        public void AddToSelect()
        {
            Pattern.AddToSelection();
        }

        [PatternMethod(IsUIAction = true)]
        public void RemoveFromSelection()
        {
            Pattern.RemoveFromSelection();
        }

        [PatternMethod(IsUIAction = true)]
        public void Select()
        {
            Pattern.Select();
        }

        [PatternMethod]
        public DesktopElement SelectionContainer()
        {
            return new DesktopElement(Pattern.CurrentSelectionContainer);
        }

        protected override void Dispose(bool disposing)
        {
            if (Pattern != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(Pattern);
                Pattern = null;
            }

            base.Dispose(disposing);
        }
    }
}
