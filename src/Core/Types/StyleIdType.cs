// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Text;

using static System.FormattableString;

namespace Axe.Windows.Core.Types
{
    /// <summary>
    /// StyleIdType class
    /// contain StyleId values
    /// </summary>
    public class StyleIdType : TypeBase
    {
        const string Prefix = "StyleId_";

        /// <summary>
        /// this list is from below source code
        /// https://docs.microsoft.com/en-us/windows/win32/winauto/uiauto-style-identifiers
        /// </summary>
        public const int StyleId_Custom       = 70000;
        public const int StyleId_Heading1     = 70001;
        public const int StyleId_Heading2     = 70002;
        public const int StyleId_Heading3     = 70003;
        public const int StyleId_Heading4     = 70004;
        public const int StyleId_Heading5     = 70005;
        public const int StyleId_Heading6     = 70006;
        public const int StyleId_Heading7     = 70007;
        public const int StyleId_Heading8     = 70008;
        public const int StyleId_Heading9     = 70009;
        public const int StyleId_Title        = 70010;
        public const int StyleId_Subtitle     = 70011;
        public const int StyleId_Normal       = 70012;
        public const int StyleId_Emphasis     = 70013;
        public const int StyleId_Quote        = 70014;
        public const int StyleId_BulletedList = 70015;
        public const int StyleId_NumberedList = 70016;

        private static StyleIdType sInstance;

        /// <summary>
        /// static method to get an instance of this class
        /// singleton
        /// </summary>
        /// <returns></returns>
        public static StyleIdType GetInstance()
        {
            if (sInstance == null)
            {
                sInstance = new StyleIdType();
            }

            return sInstance;
        }

        /// <summary>
        /// private constructor since it would be singleton model
        /// </summary>
        private StyleIdType() : base(Prefix) { }

        /// <summary>
        /// change name into right format in dictionary and list.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override string GetNameInProperFormat(string name, int id)
        {
            StringBuilder sb = new StringBuilder(name);

            sb.Replace(Prefix, "");
            sb.Append(Invariant($"({id})"));

            return sb.ToString();
        }
    }
}
