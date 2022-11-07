// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Attributes;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Types;
using System;
using System.Linq;
using UIAutomationClient;

namespace Axe.Windows.Desktop.UIAutomation.Patterns
{
    /// <summary>
    /// Control pattern wrapper for Transform Control Pattern
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671291(v=vs.85).aspx
    /// </summary>
    public class TransformPattern : A11yPattern
    {
        IUIAutomationTransformPattern _pattern;

        public TransformPattern(A11yElement e, IUIAutomationTransformPattern p) : base(e, PatternType.UIA_TransformPatternId)
        {
            _pattern = p;

            PopulateProperties();

            // Get UI Actionability based on Properties than methods.
            IsUIActionable = Properties.Any(pp => pp.Value == true);
        }

        private void PopulateProperties()
        {
#pragma warning disable CA2000 // Properties are disposed in A11yPattern.Dispose()
            Properties.Add(new A11yPatternProperty() { Name = "CanMove", Value = Convert.ToBoolean(_pattern.CurrentCanMove) });
            Properties.Add(new A11yPatternProperty() { Name = "CanResize", Value = Convert.ToBoolean(_pattern.CurrentCanResize) });
            Properties.Add(new A11yPatternProperty() { Name = "CanRotate", Value = Convert.ToBoolean(_pattern.CurrentCanRotate) });
#pragma warning restore CA2000 // Properties are disposed in A11yPattern.Dispose()
        }

        [PatternMethod]
        public void Move(double x, double y)
        {
            _pattern.Move(x, y);
        }

        [PatternMethod]
        public void Resize(double width, double height)
        {
            _pattern.Resize(width, height);
        }

        [PatternMethod]
        public void Rotate(double degree)
        {
            _pattern.Rotate(degree);
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
