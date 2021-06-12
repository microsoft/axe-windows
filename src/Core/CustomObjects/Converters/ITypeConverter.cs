// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Core.CustomObjects.Converters
{
    public interface ITypeConverter
    {
        string Render(dynamic value);
    }
}
