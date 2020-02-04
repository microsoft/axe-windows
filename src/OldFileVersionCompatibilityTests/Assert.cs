// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace OldFileVersionCompatibilityTests
{
    static class Assert
    {
        public static void AreEqual(object o1, object o2)
        {
            if (!o1.Equals(o2))
                throw new TestAssertionException($"Expected {o1} to be equal to {o2}");
        }

        public static void AreNotEqual(object o1, object o2)
        {
            if (o1.Equals(o2))
                throw new TestAssertionException($"Expected {o1} not to be equal to {o2}");
        }

        public static void IsFalse(bool b)
        {
            if (b)
                throw new TestAssertionException($"Expected: false, actual: true");
        }

        public static void IsNotNull(object o)
        {
            if (o == null)
                throw new TestAssertionException($"Expected the given object not to be null");
        }
    } // class
} // namespace
