// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.SystemAbstractions;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Axe.Windows.Automation
{
    internal static class BitmapCreatorLocator
    {
        /// <summary>
        /// Given an assembly attempt to find the unique public implementation of IBitmapCreator
        /// </summary>
        /// <param name="assemblyName">Full path to the assembly to check</param>
        /// <param name="creatorTypeCount">Returns the number of eligible implementations of IBitmapCreator. Provided for unit tests</param>
        /// <returns>An instance of the located IBitmapCreator implementation</returns>
        internal static IBitmapCreator GetBitmapCreator(string assemblyName, out int creatorTypeCount)
        {
            creatorTypeCount = 0; // Set this value in case we throw while getting the types

            Assembly assembly = Assembly.LoadFrom(assemblyName);
            Type[] types = assembly.GetTypes();
            Type bitmapCreatorType = typeof(IBitmapCreator);
            Type[] creatorTypes = (from t in types
                                   where t.IsPublic && bitmapCreatorType != t && bitmapCreatorType.IsAssignableFrom(t)
                                   select t).ToArray();

            creatorTypeCount = creatorTypes.Length;
            switch (creatorTypeCount)
            {
                case 1: // This is the only successful path
                    return Activator.CreateInstance(creatorTypes[0]) as IBitmapCreator;
                case 0:
                    throw new ArgumentException("Unable to locate any candidate IBitmapCreator implementations in the specified assembly", nameof(assemblyName));
                default:
                    string message = string.Format(CultureInfo.InvariantCulture, "Unable to choose between {0} candidate IBitmapCreator implementations in the specified assembly", creatorTypeCount);
                    throw new ArgumentException(message, nameof(assemblyName));
            }
        }
    }
}
