// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace Axe.Windows.Core.Types
{
    /// <summary>
    /// Class for Platform Property  Type IDs
    /// this is platform specific Ids
    /// </summary>
    public class PlatformPropertyType : TypeBase
    {
#pragma warning disable CA1707 // Identifiers should not contain underscores
        public const int Platform_WindowsStylePropertyId = 1;
        public const int Platform_WindowsExtendedStylePropertyId = 2;
        public const int Platform_ProcessNamePropertyId = 3;
#pragma warning restore CA1707 // Identifiers should not contain underscores

        private static PlatformPropertyType TheInstance;

#pragma warning disable CA1024 // Use properties where appropriate
        /// <summary>
        /// static method to get an instance of this class
        /// singleton
        /// </summary>
        /// <returns></returns>
        public static PlatformPropertyType GetInstance()
        {
            if (TheInstance == null)
            {
                TheInstance = new PlatformPropertyType();
            }

            return TheInstance;
        }
#pragma warning restore CA1024 // Use properties where appropriate

        /// <summary>
        /// private constructor since this uses a singleton model
        /// </summary>
        private PlatformPropertyType() : base("Platform_") { }

        /// <summary>
        /// change name into right format in dictionary and list.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override string GetNameInProperFormat(string name, int id)
        {
            StringBuilder sb = new StringBuilder(name);

            sb.Replace("Platform_", "");
            sb.Replace("PropertyId", "");
            // sb.Append(Invariant($" ({id})"));

            return sb.ToString();
        }
    }
}
