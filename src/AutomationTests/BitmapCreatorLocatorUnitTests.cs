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
        static string GetPathToAssembly(string assemblyName)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), assemblyName);
        }

        [TestMethod]
        public void GetBitmapCreator_NoAssemblyFound_ThrowsFileNotFoundException()
        {
            string path = GetPathToAssembly("NoSuchAssemblyExists.dll");
            Assert.ThrowsException<FileNotFoundException>(() => BitmapCreatorLocator.GetBitmapCreator(path));
        }

        [TestMethod]
        public void GetBitmapCreator_NoRelatedClassesExist_ThrowsArgumentException()
        {
            string path = GetPathToAssembly("System.IO.Packaging.dll");
            ArgumentException e = Assert.ThrowsException<ArgumentException>(() => BitmapCreatorLocator.GetBitmapCreator(path));
            Assert.AreEqual("assemblyName", e.ParamName);
        }

        [TestMethod]
        public void GetBitmapCreator_NoDerivedClassesExist_ThrowsArgumentException()
        {
            string path = GetPathToAssembly("Axe.Windows.SystemAbstractions.dll");
            ArgumentException e = Assert.ThrowsException<ArgumentException>(() => BitmapCreatorLocator.GetBitmapCreator(path));
            Assert.AreEqual("assemblyName", e.ParamName);
        }

        [TestMethod]
        public void GetGitmapCreator_DerivedClassExists_CreatesObjectOfExpectedType()
        {
            string path = GetPathToAssembly("Axe.Windows.Desktop.dll");
            IBitmapCreator creator = BitmapCreatorLocator.GetBitmapCreator(path);
            Assert.IsInstanceOfType(creator, typeof(FrameworkBitmapCreator));
        }
    }
}
