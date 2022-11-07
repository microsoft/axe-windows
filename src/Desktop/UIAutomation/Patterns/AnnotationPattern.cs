// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using System.Runtime.InteropServices;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for Annotation Control Pattern
    /// No action is available
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/hh448769(v=vs.85).aspx
    /// </summary>
    public class AnnotationPattern : A11yPattern
    {
        IUIAutomationAnnotationPattern _pattern;

        public AnnotationPattern(A11yElement e, IUIAutomationAnnotationPattern p) : base(e, PatternType.UIA_AnnotationPatternId)
        {
            _pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "AnnotationTypeId", Value = _pattern.CurrentAnnotationTypeId });
            Properties.Add(new A11yPatternProperty() { Name = "AnnotationTypeName", Value = _pattern.CurrentAnnotationTypeName });
            Properties.Add(new A11yPatternProperty() { Name = "Author", Value = _pattern.CurrentAuthor });
            Properties.Add(new A11yPatternProperty() { Name = "DateTime", Value = _pattern.CurrentDateTime });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

#pragma warning disable CA1024 // Use properties where appropriate
        [PatternMethod]
        public DesktopElement GetTarget()
        {
            return new DesktopElement(_pattern.CurrentTarget);
        }
#pragma warning restore CA1024 // Use properties where appropriate

        protected override void Dispose(bool disposing)
        {
            if (_pattern != null)
            {
                Marshal.ReleaseComObject(_pattern);
                _pattern = null;
            }

            base.Dispose(disposing);
        }
    }
}
