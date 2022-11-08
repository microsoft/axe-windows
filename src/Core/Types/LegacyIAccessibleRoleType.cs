// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

using static System.FormattableString;

namespace Axe.Windows.Core.Types
{
    /// <summary>
    /// Class for LegacyIAccessible.Role Types
    /// </summary>
    public class LegacyIAccessibleRoleType : TypeBase
    {
#pragma warning disable CA1707 // Identifiers should not contain underscores
        public const int ROLE_SYSTEM_TITLEBAR = 1;
        public const int ROLE_SYSTEM_MENUBAR = 2;
        public const int ROLE_SYSTEM_SCROLLBAR = 3;
        public const int ROLE_SYSTEM_GRIP = 4;
        public const int ROLE_SYSTEM_SOUND = 5;
        public const int ROLE_SYSTEM_CURSOR = 6;
        public const int ROLE_SYSTEM_CARET = 7;
        public const int ROLE_SYSTEM_ALERT = 8;
        public const int ROLE_SYSTEM_WINDOW = 9;
        public const int ROLE_SYSTEM_CLIENT = 10;
        public const int ROLE_SYSTEM_MENUPOPUP = 11;
        public const int ROLE_SYSTEM_MENUITEM = 12;
        public const int ROLE_SYSTEM_TOOLTIP = 13;
        public const int ROLE_SYSTEM_APPLICATION = 14;
        public const int ROLE_SYSTEM_DOCUMENT = 15;
        public const int ROLE_SYSTEM_PANE = 16;
        public const int ROLE_SYSTEM_CHART = 17;
        public const int ROLE_SYSTEM_DIALOG = 18;
        public const int ROLE_SYSTEM_BORDER = 19;
        public const int ROLE_SYSTEM_GROUPING = 20;
        public const int ROLE_SYSTEM_SEPARATOR = 21;
        public const int ROLE_SYSTEM_TOOLBAR = 22;
        public const int ROLE_SYSTEM_STATUSBAR = 23;
        public const int ROLE_SYSTEM_TABLE = 24;
        public const int ROLE_SYSTEM_COLUMNHEADER = 25;
        public const int ROLE_SYSTEM_ROWHEADER = 26;
        public const int ROLE_SYSTEM_COLUMN = 27;
        public const int ROLE_SYSTEM_ROW = 28;
        public const int ROLE_SYSTEM_CELL = 29;
        public const int ROLE_SYSTEM_LINK = 30;
        public const int ROLE_SYSTEM_HELPBALLOON = 31;
        public const int ROLE_SYSTEM_CHARACTER = 32;
        public const int ROLE_SYSTEM_LIST = 33;
        public const int ROLE_SYSTEM_LISTITEM = 34;
        public const int ROLE_SYSTEM_OUTLINE = 35;
        public const int ROLE_SYSTEM_OUTLINEITEM = 36;
        public const int ROLE_SYSTEM_PAGETAB = 37;
        public const int ROLE_SYSTEM_PROPERTYPAGE = 38;
        public const int ROLE_SYSTEM_INDICATOR = 39;
        public const int ROLE_SYSTEM_GRAPHIC = 40;
        public const int ROLE_SYSTEM_STATICTEXT = 41;
        public const int ROLE_SYSTEM_TEXT = 42;
        public const int ROLE_SYSTEM_PUSHBUTTON = 43;
        public const int ROLE_SYSTEM_CHECKBUTTON = 44;
        public const int ROLE_SYSTEM_RADIOBUTTON = 45;
        public const int ROLE_SYSTEM_COMBOBOX = 46;
        public const int ROLE_SYSTEM_DROPLIST = 47;
        public const int ROLE_SYSTEM_PROGRESSBAR = 48;
        public const int ROLE_SYSTEM_DIAL = 49;
        public const int ROLE_SYSTEM_HOTKEYFIELD = 50;
        public const int ROLE_SYSTEM_SLIDER = 51;
        public const int ROLE_SYSTEM_SPINBUTTON = 52;
        public const int ROLE_SYSTEM_DIAGRAM = 53;
        public const int ROLE_SYSTEM_ANIMATION = 54;
        public const int ROLE_SYSTEM_EQUATION = 55;
        public const int ROLE_SYSTEM_BUTTONDROPDOWN = 56;
        public const int ROLE_SYSTEM_BUTTONMENU = 57;
        public const int ROLE_SYSTEM_BUTTONDROPDOWNGRID = 58;
        public const int ROLE_SYSTEM_WHITESPACE = 59;
        public const int ROLE_SYSTEM_PAGETABLIST = 60;
        public const int ROLE_SYSTEM_CLOCK = 61;
        public const int ROLE_SYSTEM_SPLITBUTTON = 62;
        public const int ROLE_SYSTEM_IPADDRESS = 63;
        public const int ROLE_SYSTEM_OUTLINEBUTTON = 64;
#pragma warning restore CA1707 // Identifiers should not contain underscores

        private static LegacyIAccessibleRoleType TheInstance;

#pragma warning disable CA1024 // Use properties where appropriate
        /// <summary>
        /// static method to get an instance of this class
        /// singleton
        /// </summary>
        /// <returns></returns>
        public static LegacyIAccessibleRoleType GetInstance()
        {
            if (TheInstance == null)
            {
                TheInstance = new LegacyIAccessibleRoleType();
            }

            return TheInstance;
        }
#pragma warning restore CA1024 // Use properties where appropriate

        /// <summary>
        /// private constructor since this uses a singleton model
        /// </summary>
        private LegacyIAccessibleRoleType() : base("ROLE_SYSTEM_") { }

        /// <summary>
        /// change name into right format in dictionary and list.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override string GetNameInProperFormat(string name, int id)
        {
            StringBuilder sb = new StringBuilder(name);

            sb.Append(Invariant($"({id})"));

            return sb.ToString();
        }
    }
}
