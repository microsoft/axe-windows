// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Core.Bases;
using Axe.Windows.Desktop.UIAutomation;
using System;
using System.Globalization;

namespace Axe.Windows.Automation
{
    class TargetElementLocator : ITargetElementLocator
    {
        public A11yElement LocateRootElement(int processId)
        {
            try
            {
                var element = A11yAutomation.ElementFromProcessId(processId);

#pragma warning disable CA2000 // Call IDisposable.Dispose()
                return new ElementContext(element).Element;
#pragma warning restore CA2000
            }
            catch (Exception ex)
            {
                throw new AxeWindowsAutomationException(string.Format(CultureInfo.InvariantCulture, DisplayStrings.ErrorFailToGetRootElementOfProcess, processId, ex), ex);
            }
        }
    }
}
