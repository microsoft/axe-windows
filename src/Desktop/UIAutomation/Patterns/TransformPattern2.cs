// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using System;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for Transform2 Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671291(v=vs.85).aspx
    /// </summary>
    public class TransformPattern2 : A11yPattern
    {
        IUIAutomationTransformPattern2 Pattern;

        public TransformPattern2(A11yElement e, IUIAutomationTransformPattern2 p) : base(e, PatternType.UIA_TransformPattern2Id)
        {
            Pattern = p;

            PopulateProperties();
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "CanMove", Value = Convert.ToBoolean(Pattern.CurrentCanMove) });
            Properties.Add(new A11yPatternProperty() { Name = "CanResize", Value = Convert.ToBoolean(Pattern.CurrentCanResize) });
            Properties.Add(new A11yPatternProperty() { Name = "CanRotate", Value = Convert.ToBoolean(Pattern.CurrentCanRotate) });
            Properties.Add(new A11yPatternProperty() { Name = "CanZoom", Value = Convert.ToBoolean(Pattern.CurrentCanZoom) });
            Properties.Add(new A11yPatternProperty() { Name = "CanZoomLevel", Value = Convert.ToBoolean(Pattern.CurrentZoomLevel) });
            Properties.Add(new A11yPatternProperty() { Name = "CanZoomMaximum", Value = Convert.ToBoolean(Pattern.CurrentZoomMaximum) });
            Properties.Add(new A11yPatternProperty() { Name = "CanZoomMinimum", Value = Convert.ToBoolean(Pattern.CurrentZoomMinimum) });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod(IsUIAction = true)]
        public void Zoom(double zoomValue)
        {
            Pattern.Zoom(zoomValue);
        }

        [PatternMethod(IsUIAction = true)]
        public void ZoomByUnit(ZoomUnit zu)
        {
            Pattern.ZoomByUnit(zu);
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
