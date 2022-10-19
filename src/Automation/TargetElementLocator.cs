// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions.Contexts;
using Axe.Windows.Automation.Resources;
using Axe.Windows.Core.Bases;
using Axe.Windows.Desktop.UIAutomation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Axe.Windows.Automation
{
    class TargetElementLocator : ITargetElementLocator
    {
        public IEnumerable<A11yElement> LocateRootElements(int processId, IActionContext actionContext)
        {
            try
            {
                var desktopElements = actionContext.DesktopDataContext.A11yAutomation.ElementsFromProcessId(processId, actionContext.DesktopDataContext);
                return GetA11YElementsFromDesktopElements(desktopElements);
            }
            catch (Exception ex)
            {
                throw new AxeWindowsAutomationException(string.Format(CultureInfo.InvariantCulture, ErrorMessages.ErrorFailToGetRootElementsOfProcess, processId, ex), ex);
            }
        }

        private static IEnumerable<A11yElement> GetA11YElementsFromDesktopElements(IEnumerable<DesktopElement> desktopElements)
        {
            if (!desktopElements.Any()) throw new ArgumentException(ErrorMessages.NoDesktopElements, nameof(desktopElements));

            foreach (var e in desktopElements)
            {
#pragma warning disable CA2000 // Call IDisposable.Dispose()
                yield return new ElementContext(e).Element;
#pragma warning restore CA2000
            }
        }
    }
}
