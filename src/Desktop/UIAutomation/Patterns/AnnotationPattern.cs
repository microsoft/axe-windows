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
    /// No actiona is available
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/hh448769(v=vs.85).aspx
    /// </summary>
    public class AnnotationPattern : A11yPattern
    {
        IUIAutomationAnnotationPattern Pattern;

        public AnnotationPattern(A11yElement e, IUIAutomationAnnotationPattern p) :base(e, PatternType.UIA_AnnotationPatternId)
        {
            Pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            this.Properties.Add(new A11yPatternProperty() { Name = "AnnotationTypeId", Value = this.Pattern.CurrentAnnotationTypeId });
            this.Properties.Add(new A11yPatternProperty() { Name = "AnnotationTypeName", Value = this.Pattern.CurrentAnnotationTypeName });
            this.Properties.Add(new A11yPatternProperty() { Name = "Author", Value = this.Pattern.CurrentAuthor });
            this.Properties.Add(new A11yPatternProperty() { Name = "DateTime", Value = this.Pattern.CurrentDateTime });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

#pragma warning disable CA1024 // Use properties where appropriate
        [PatternMethod]
        public DesktopElement GetTarget()
        {
            return new DesktopElement(this.Pattern.CurrentTarget);
        }
#pragma warning restore CA1024 // Use properties where appropriate

        protected override void Dispose(bool disposing)
        {
            if(Pattern != null)
            {
                Marshal.ReleaseComObject(Pattern);
                this.Pattern = null;
            }

            base.Dispose(disposing);
        }
    }
}
