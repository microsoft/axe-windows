// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using Axe.Windows.Desktop.Drawing;
using Axe.Windows.SystemAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace Axe.Windows.AutomationTests
{
    [TestClass]
    public class BitmapCreatorLocatorUnitTests
    {
        const int Undefined = -1; // Any value that will never be returned as the count

        static string GetPathToAssembly(string assemblyName)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), assemblyName);
        }

        [TestMethod]
        public void GetBitmapCreator_NoAssemblyFound_ThrowsFileNotFoundException()
        {
            int creatorTypeCount = Undefined;
            string path = GetPathToAssembly("NoSuchAssemblyExists.dll");

            Assert.ThrowsException<FileNotFoundException>(() => BitmapCreatorLocator.GetBitmapCreator(path, out creatorTypeCount));

            Assert.AreEqual(0, creatorTypeCount);
        }

        [TestMethod]
        public void GetBitmapCreator_NoRelatedClassesExist_ThrowsArgumentException()
        {
            int creatorTypeCount = Undefined;
            string path = GetPathToAssembly("System.IO.Packaging.dll");

            ArgumentException e = Assert.ThrowsException<ArgumentException>(() => BitmapCreatorLocator.GetBitmapCreator(path, out creatorTypeCount));

            Assert.AreEqual("assemblyName", e.ParamName);
            Assert.AreEqual(0, creatorTypeCount);
        }

        [TestMethod]
        public void GetBitmapCreator_ZeroDerivedClassesExist_ThrowsArgumentException()
        {
            int creatorTypeCount = Undefined;
            string path = GetPathToAssembly("Axe.Windows.SystemAbstractions.dll");

            ArgumentException e = Assert.ThrowsException<ArgumentException>(() => BitmapCreatorLocator.GetBitmapCreator(path, out creatorTypeCount));

            Assert.AreEqual("assemblyName", e.ParamName);
            Assert.AreEqual(0, creatorTypeCount);
        }

        [TestMethod]
        public void GetBitmapCreator_OneDerivedClassExists_CreatesObjectOfExpectedType()
        {
            string path = GetPathToAssembly("Axe.Windows.Desktop.dll");

            IBitmapCreator creator = BitmapCreatorLocator.GetBitmapCreator(path, out int creatorTypeCount);

            Assert.IsInstanceOfType(creator, typeof(FrameworkBitmapCreator));
            Assert.AreEqual(1, creatorTypeCount);
        }

        [TestMethod]
        public void GetBitmapCreator_TwoDerivedClassesExist_ThrowsArgumentException()
        {
            int creatorTypeCount = Undefined;
            string path = Assembly.GetExecutingAssembly().Location;

            ArgumentException e = Assert.ThrowsException<ArgumentException>(() => BitmapCreatorLocator.GetBitmapCreator(path, out creatorTypeCount));

            Assert.AreEqual("assemblyName", e.ParamName);
            Assert.AreEqual(2, creatorTypeCount);
        }
    }
}
