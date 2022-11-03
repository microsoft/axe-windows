// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.Types;
using Axe.Windows.Desktop.Utility;
using System.Collections.Generic;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for SpreadsheetItem Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/hh448774(v=vs.85).aspx
    /// </summary>
    public class SpreadsheetItemPattern : A11yPattern
    {
        IUIAutomationSpreadsheetItemPattern _pattern;

        public SpreadsheetItemPattern(A11yElement e, IUIAutomationSpreadsheetItemPattern p) : base(e, PatternType.UIA_SpreadsheetItemPatternId)
        {
            _pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "Formula", Value = _pattern.CurrentFormula });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod]
        public IList<DesktopElement> GetAnnotationObjects()
        {
            return _pattern.GetCurrentAnnotationObjects()?.ToListOfDesktopElements();
        }

        [PatternMethod]
        public IList<string> GetAnnotationTypes()
        {
            var array = _pattern.GetCurrentAnnotationTypes();
            List<string> list = new List<string>();

            if (array.Length > 0)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    list.Add(AnnotationType.GetInstance().GetNameById((int)array.GetValue(i)));
                }
            }

            return list;
        }

        protected override void Dispose(bool disposing)
        {
            if (_pattern != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_pattern);
                _pattern = null;
            }

            base.Dispose(disposing);
        }
    }
}
