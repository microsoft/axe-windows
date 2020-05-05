// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Types;
using System.Text;

namespace Axe.Windows.Desktop.Types
{
    /// <summary>
    /// TextAttributeTypes
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ee671662(v=vs.85).aspx
    /// </summary>
    public class TextAttributeType : TypeBase
    {
#pragma warning disable CA1707 // Identifiers should not contain underscores
        public const int UIA_AnimationStyleAttributeId = 40000;
        public const int UIA_BackgroundColorAttributeId = 40001;
        public const int UIA_BulletStyleAttributeId = 40002;
        public const int UIA_CapStyleAttributeId = 40003;
        public const int UIA_CultureAttributeId = 40004;
        public const int UIA_FontNameAttributeId = 40005;
        public const int UIA_FontSizeAttributeId = 40006;
        public const int UIA_FontWeightAttributeId = 40007;
        public const int UIA_ForegroundColorAttributeId = 40008;
        public const int UIA_HorizontalTextAlignmentAttributeId = 40009;
        public const int UIA_IndentationFirstLineAttributeId = 40010;
        public const int UIA_IndentationLeadingAttributeId = 40011;
        public const int UIA_IndentationTrailingAttributeId = 40012;
        public const int UIA_IsHiddenAttributeId = 40013;
        public const int UIA_IsItalicAttributeId = 40014;
        public const int UIA_IsReadOnlyAttributeId = 40015;
        public const int UIA_IsSubscriptAttributeId = 40016;
        public const int UIA_IsSuperscriptAttributeId = 40017;
        public const int UIA_MarginBottomAttributeId = 40018;
        public const int UIA_MarginLeadingAttributeId = 40019;
        public const int UIA_MarginTopAttributeId = 40020;
        public const int UIA_MarginTrailingAttributeId = 40021;
        public const int UIA_OutlineStylesAttributeId = 40022;
        public const int UIA_OverlineColorAttributeId = 40023;
        public const int UIA_OverlineStyleAttributeId = 40024;
        public const int UIA_StrikethroughColorAttributeId = 40025;
        public const int UIA_StrikethroughStyleAttributeId = 40026;
        public const int UIA_TabsAttributeId = 40027;
        public const int UIA_TextFlowDirectionsAttributeId = 40028;
        public const int UIA_UnderlineColorAttributeId = 40029;
        public const int UIA_UnderlineStyleAttributeId = 40030;
        public const int UIA_AnnotationTypesAttributeId = 40031;
        public const int UIA_AnnotationObjectsAttributeId = 40032;
        public const int UIA_StyleNameAttributeId = 40033;
        public const int UIA_StyleIdAttributeId = 40034;
        public const int UIA_LinkAttributeId = 40035;
        public const int UIA_IsActiveAttributeId = 40036;
        public const int UIA_SelectionActiveEndAttributeId = 40037;
        public const int UIA_CaretPositionAttributeId = 40038;
        public const int UIA_CaretBidiModeAttributeId = 40039;
        public const int UIA_LineSpacingAttributeId = 40040;
        public const int UIA_BeforeParagraphSpacingAttributeId = 40041;
        public const int UIA_AfterParagraphSpacingAttributeId = 40042;
        public const int UIA_SayAsInterpretAsAttributeId = 40043;
#pragma warning restore CA1707 // Identifiers should not contain underscores

        private static TextAttributeType sInstance;

        /// <summary>
        /// static method to get an instance of this class
        /// singleton
        /// </summary>
        /// <returns></returns>
        public static TextAttributeType GetInstance()
        {
            if (sInstance == null)
            {
                sInstance = new TextAttributeType();
            }

            return sInstance;
        }

        /// <summary>
        /// private constructor since it would be singleton model
        /// </summary>
        private TextAttributeType() : base() { }

        /// <summary>
        /// change name into right format in dictionary and list.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override string GetNameInProperFormat(string name, int id)
        {
            StringBuilder sb = new StringBuilder(name);

            sb.Replace("UIA_", "");
            sb.Replace("AttributeId", "");

            return sb.ToString();
        }
    }
}
