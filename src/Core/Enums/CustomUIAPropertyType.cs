// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Axe.Windows.Core.Enums
{
    public enum CustomUIAPropertyType
    {
        Unset = 0,
#pragma warning disable CA1720 // Identifier contains type name: type names from JSON
        String,
        Int,
        Bool,
        Double,
        Point,
        Element,
        Enum
#pragma warning restore CA1720 // Identifier contains type name: type names from JSON
    }
}
