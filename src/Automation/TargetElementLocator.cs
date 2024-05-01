// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions.Contexts;
using Axe.Windows.Automation.Resources;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Misc;
using Axe.Windows.Desktop.UIAutomation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.Automation
{
    class TargetElementLocator : ITargetElementLocator
    {
        private IntPtr _rootWindowHandle;

        public IEnumerable<A11yElement> LocateRootElements(int processId, IActionContext actionContext)
        {
            try
            {
                var desktopElements = A11yAutomation.ElementsFromProcessId(processId, _rootWindowHandle, actionContext.DesktopDataContext);
                return GetA11yElementsFromDesktopElements(desktopElements);
            }
            catch (Exception ex)
            {
                throw new AxeWindowsAutomationException(ErrorMessages.ErrorFailToGetRootElementsOfProcess.WithParameters(processId), ex);
            }
        }

        private static IEnumerable<A11yElement> GetA11yElementsFromDesktopElements(IEnumerable<DesktopElement> desktopElements)
        {
            if (desktopElements == null || !desktopElements.Any()) throw new ArgumentException(ErrorMessages.NoDesktopElements, nameof(desktopElements));

            foreach (var e in desktopElements)
            {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
                yield return new ElementContext(e).Element;
#pragma warning restore CA2000
            }
        }

        public void SetRootWindowHandle(IntPtr rootWindowHandle) => _rootWindowHandle = rootWindowHandle;
    }
}
