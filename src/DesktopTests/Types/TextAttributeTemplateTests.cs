// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Desktop.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Axe.Windows.DesktopTests.Types
{
    [TestClass]
    public class TextAttributeTemplateTests
    {
        private static int[] _ignoreIds = new int[] {
            TextAttributeType.UIA_AnnotationObjectsAttributeId,
            TextAttributeType.UIA_LinkAttributeId,
        };

        [TestMethod]
        public void TextAttributeTemplate_HasExpectedEntries()
        {
            var templateList = TextAttributeTemplate.GetTemplate();
            var ids = TextAttributeType.GetInstance().Values;

            foreach (var id in ids)
            {
                if (_ignoreIds.Contains(id)) continue;

                Assert.IsTrue(templateList.Any(data => data.Item1 == id), $"{TextAttributeType.GetInstance().GetNameById(id)} was not found in the TextAttribute template");
            }
        }
    } // class
} // namespace
