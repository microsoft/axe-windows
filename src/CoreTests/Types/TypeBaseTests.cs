// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace CoreTests.Types
{
    [TestClass]
    public class TypeBaseTests
    {
        private class DummyType : TypeBase
        {
            public DummyType() : base("NFL_")
            {}

            public const int NFL_Falcons = 1;
            public const int NFL_Seahawks = 2;
            public const int NFL_Colts = 3;
            public const int MLB_Braves = 4;
        }

        [TestMethod]
        public void TypeBase_CreatesDictionaryAsExpected()
        {
            var dummy = new DummyType();
            Assert.AreEqual(3, dummy.Values.Count());
            Assert.IsTrue(dummy.Values.Contains(DummyType.NFL_Falcons));
            Assert.IsFalse(dummy.Values.Contains(DummyType.MLB_Braves));
        }
    } // class
} // namespace
