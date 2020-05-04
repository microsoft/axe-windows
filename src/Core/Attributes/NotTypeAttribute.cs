// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Axe.Windows.Core.Attributes
{
    /// <summary>
    /// For classes which indicate types (such as <see cref="Axe.Windows.Core.Types.ControlType"/>)
    /// Indicates that a const int should not be included in the dictionary of types
    /// that will be created by <see cref="Axe.Windows.Core.Types.TypeBase"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class NotTypeAttribute : Attribute
    {
    } // class
} // namespace
