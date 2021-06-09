// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Interop.UIAutomationCore;

namespace Axe.Windows.Desktop.UIAutomation.CustomObjects.Converters
{
    public interface ITypeConverter
    {
        UIAutomationType UnderlyingUiaType { get; }

        string Render(dynamic value);
    }
}
