// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace Axe.Windows.Core.Misc
{
    public static class ListHelper
    {
        public static void DisposeAndClear<T>(IList<T> items) where T : IDisposable
        {
            DisposeAllItems(items);
            items?.Clear();
        }

        public static void DisposeAllItems<T>(IList<T> items) where T : IDisposable
        {
            if (items != null)
            {
                foreach (T item in items)
                {
                    item.Dispose();
                }
            }
        }
    }
}
