// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Types;
using Axe.Windows.Desktop.Styles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Axe.Windows.Desktop.Types
{
    using TemplateData = Tuple<int, string, dynamic, Type>;
    using static Axe.Windows.Desktop.Types.TextAttributeType;

    public static class TextAttributeTemplate
    {
#pragma warning disable CA1002 // Do not expose generic lists
        /// <summary>
        /// Get template of each attribute type
        /// it is used in TextRangeFindDialog.
        /// </summary>
        /// <returns></returns>
        public static List<TemplateData> GetTemplate()
        {
            var boolList = new List<KeyValuePair<bool, string>>() { new KeyValuePair<bool, string>(false, "False"), new KeyValuePair<bool, string>(true, "True") };

            return new List<TemplateData>
            {
                CreateTemplateData<int>(UIA_AnimationStyleAttributeId, AnimationStyle.GetInstance()),
                CreateTemplateData<int>(UIA_BackgroundColorAttributeId),
                CreateTemplateData<int>(UIA_BulletStyleAttributeId, BulletStyle.GetInstance()),
                CreateTemplateData<int>(UIA_CapStyleAttributeId, CapStyle.GetInstance()),
                CreateTemplateData<int>(UIA_CultureAttributeId, CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures).Select(c => new KeyValuePair<int, string>(c.LCID, c.EnglishName)).ToList()),
                CreateTemplateData<string>(UIA_FontNameAttributeId),
                CreateTemplateData<double>(UIA_FontSizeAttributeId),
                CreateTemplateData<int>(UIA_FontWeightAttributeId, FontWeight.GetInstance()),
                CreateTemplateData<int>(UIA_ForegroundColorAttributeId),
                CreateTemplateData<int>(UIA_HorizontalTextAlignmentAttributeId, HorizontalTextAlignment.GetInstance()),
                CreateTemplateData<int>(UIA_IndentationFirstLineAttributeId),
                CreateTemplateData<int>(UIA_IndentationLeadingAttributeId),
                CreateTemplateData<int>(UIA_IndentationTrailingAttributeId),
                CreateTemplateData<bool>(UIA_IsHiddenAttributeId, boolList),
                CreateTemplateData<bool>(UIA_IsItalicAttributeId, boolList),
                CreateTemplateData<bool>(UIA_IsReadOnlyAttributeId, boolList),
                CreateTemplateData<bool>(UIA_IsSubscriptAttributeId, boolList),
                CreateTemplateData<bool>(UIA_IsSuperscriptAttributeId, boolList),
                CreateTemplateData<int>(UIA_MarginBottomAttributeId),
                CreateTemplateData<int>(UIA_MarginLeadingAttributeId),
                CreateTemplateData<int>(UIA_MarginTopAttributeId),
                CreateTemplateData<int>(UIA_MarginTrailingAttributeId),
                CreateTemplateData<int>(UIA_OutlineStylesAttributeId, OutlineStyle.GetInstance()),
                CreateTemplateData<int>(UIA_OverlineColorAttributeId),
                CreateTemplateData<int>(UIA_OverlineStyleAttributeId, TextDecorationLineStyle.GetInstance()),
                CreateTemplateData<int>(UIA_StrikethroughColorAttributeId),
                CreateTemplateData<int>(UIA_StrikethroughStyleAttributeId, TextDecorationLineStyle.GetInstance()),
                CreateTemplateData<int>(UIA_TabsAttributeId),
                CreateTemplateData<int>(UIA_TextFlowDirectionsAttributeId, FlowDirection.GetInstance()),
                CreateTemplateData<int>(UIA_UnderlineColorAttributeId),
                CreateTemplateData<int>(UIA_UnderlineStyleAttributeId, TextDecorationLineStyle.GetInstance()),
                CreateTemplateData<int>(UIA_AnnotationTypesAttributeId, AnnotationType.GetInstance()),
                //CreateTemplateData<int>(UIA_AnnotationObjectsAttributeId, AnimationStyles.GetInstance()),
                CreateTemplateData<string>(UIA_StyleNameAttributeId),
                CreateTemplateData<int>(UIA_StyleIdAttributeId, StyleId.GetInstance()),
                //CreateTemplateData<int>(UIA_LinkAttributeId, AnimationStyles.GetInstance()),
                CreateTemplateData<bool>(UIA_IsActiveAttributeId, boolList),
                CreateTemplateData<int>(UIA_SelectionActiveEndAttributeId, ActiveEnd.GetInstance()),
                CreateTemplateData<int>(UIA_CaretPositionAttributeId, CaretPosition.GetInstance()),
                CreateTemplateData<int>(UIA_CaretBidiModeAttributeId, CaretBidiMode.GetInstance()),
                CreateTemplateData<string>(UIA_LineSpacingAttributeId),
                CreateTemplateData<double>(UIA_BeforeParagraphSpacingAttributeId),
                CreateTemplateData<double>(UIA_AfterParagraphSpacingAttributeId),
                CreateTemplateData<int>(UIA_SayAsInterpretAsAttributeId, SayAsInterpretAs.GetInstance()),
            };
        }
#pragma warning restore CA1002 // Do not expose generic lists

        private static TemplateData CreateTemplateData<T>(int id, dynamic value = null)
        {
            return new TemplateData(
                id,
                TextAttributeType.GetInstance().GetNameById(id),
                value,
                typeof(T));
        }

        private static TemplateData CreateTemplateData<T>(int id, TypeBase typeBase)
        {
            return CreateTemplateData<T>(id, typeBase.GetKeyValuePairList());
        }
    } // class
} // namespace
