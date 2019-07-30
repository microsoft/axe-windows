// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

namespace OldFileVersionWriteTests
{
    /// <summary>
    /// Simple stubs to mimic the Assert framework, so that the same code can be used
    /// both in unit tests and in this app
    /// </summary>
    internal static class Assert
    {
        internal static void AreNotEqual<T>(T expectedValue, T actualValue)
        {
            if (expectedValue.Equals(actualValue))
            {
                throw new InvalidDataException(string.Format(
                    "Actual Value {0} must not match Value {1}",
                    actualValue, expectedValue));
            }
        }

        internal static void AreEqual<T>(T expectedValue, T actualValue)
        {
            if (!expectedValue.Equals(actualValue))
            {
                throw new InvalidDataException(string.Format(
                    "Actual Value {0} must match Value {1}",
                    actualValue, expectedValue));
            }
        }

        internal static void IsFalse(bool actualValue)
        {
            if (actualValue)
            {
                throw new InvalidDataException("Actual value was true when it should have been false");
            }
        }

        internal static void IsNull(object actualValue)
        {
            if (actualValue != null)
            {
                throw new InvalidDataException(string.Format(
                    "Actual value {0} must be null", actualValue.ToString()));
            }
        }

        internal static void IsNotNull(object actualValue)
        {
            if (actualValue == null)
            {
                throw new InvalidDataException("Actual value was null when it can't be");
            }
        }
    }
}
