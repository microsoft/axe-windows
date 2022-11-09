// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace MsiFileTests
{
    [TestClass]
    public class WxsValidationTests
    {
#if !DEBUG  // This test needs to run uniquely in release builds
        [TestMethod]
#endif
        public void AllDropFilesAreAccountedFor()
        {
            // Fails if any non-pdb drop files exist without a corresponding entry in either
            // the WXS file or the set of intentional exclusions
            string currentFolder = Path.GetFullPath(Assembly.GetExecutingAssembly().Location);
            string repoRoot = Path.GetFullPath(Path.Combine(currentFolder, @"..\..\..\..\.."));
            string wxsFile = Path.GetFullPath(Path.Combine(repoRoot, @"CLI_Installer\Product.wxs"));
            HashSet<string> productComponentExclusions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Axe.Windows.Automation.xml",
                "AxeWindowsCLI.runtimeconfig.dev.json",
            };

            CompareWxsSectionToDropPath(repoRoot, @"CLI\bin\Release\netcoreapp3.1", wxsFile, "ProductComponent", productComponentExclusions);
            CompareWxsSectionToDropPath(repoRoot, @"CLI\bin\Release\netcoreapp3.1\runtimes\win\lib\netcoreapp2.0", wxsFile, "NetCoreApp20Component");
            CompareWxsSectionToDropPath(repoRoot, @"CLI\bin\Release\netcoreapp3.1\runtimes\win\lib\netcoreapp2.1", wxsFile, "NetCoreApp21Component");
            CompareWxsSectionToDropPath(repoRoot, @"CLI\bin\Release\netcoreapp3.1\runtimes\win\lib\netcoreapp3.1", wxsFile, "NetCoreApp31Component");
            CompareWxsSectionToDropPath(repoRoot, @"CLI\bin\Release\netcoreapp3.1\runtimes\win\lib\netstandard2.0", wxsFile, "NetStandard20Component");
        }

        private static void CompareWxsSectionToDropPath(string repoRoot, string relativeDropPath,
            string wxsFile, string wxsComponentId, HashSet<string> intentionalExclusions = null)
        {
            string dropPath = Path.Combine(repoRoot, relativeDropPath);
            HashSet<string> filesInDropPath = GetNonSymbolFilesInPath(dropPath, intentionalExclusions);
            HashSet<string> filesInWxsComponent = GetFilesIncludedInWxsComponent(repoRoot, wxsFile, wxsComponentId);

            filesInDropPath.ExceptWith(filesInWxsComponent);

            Assert.IsFalse(filesInDropPath.Any(), $"Drop files not in \"{wxsComponentId}\" of WXS: {string.Join(", ", filesInDropPath)}");
        }

        private static HashSet<string> GetNonSymbolFilesInPath(string path, HashSet<string> intentionalExclusions)
        {
            HashSet<string> filesInPath = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string file in Directory.EnumerateFiles(path))
            {
                if (intentionalExclusions != null && !intentionalExclusions.Contains(Path.GetFileName(file)))
                {
                    string extension = Path.GetExtension(file);
                    if (extension != ".pdb")
                    {
                        filesInPath.Add(Path.GetFullPath(file));
                    }
                }
            }

            return filesInPath;
        }

        private static HashSet<string> GetFilesIncludedInWxsComponent(string repoRoot, string wxsFile, string componentId)
        {
            HashSet<string> filesInSection = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            using (XmlReader reader = XmlReader.Create(wxsFile))
            {
                bool thisIsTheCorrectComponent = false;

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "Component")
                        {
                            string id = reader.GetAttribute("Id");
                            thisIsTheCorrectComponent = (id == componentId);
                        }
                        else if (reader.Name == "File" && thisIsTheCorrectComponent)
                        {
                            string relativeFile = reader.GetAttribute("Source");
                            filesInSection.Add(repoRoot + relativeFile.Substring(2));
                        }
                    }
                }
            }

            return filesInSection;
        }
    }
}
