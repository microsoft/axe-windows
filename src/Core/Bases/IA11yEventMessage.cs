// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;

namespace Axe.Windows.Core.Bases
{
    public interface IA11yEventMessage
    {
        int EventId { get; set; }

        /// <summary>
        /// Time stamp with millisecond accuracy
        /// </summary>
        string TimeStamp { get; set; }

#pragma warning disable CA1002 // Do not expose generic lists
        List<KeyValuePair<string, dynamic>> Properties { get; }
#pragma warning restore CA1002 // Do not expose generic lists

        A11yElement Element { get; set; }
    }
}
