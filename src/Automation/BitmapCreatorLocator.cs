// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.SystemAbstractions;
using System;
using System.Linq;
using System.Reflection;

namespace Axe.Windows.Automation
{
    internal static class BitmapCreatorLocator
    {
        public static IBitmapCreator GetBitmapCreator(string assemblyName)
        {
            var assembly = Assembly.LoadFrom(assemblyName);
            var types = assembly.GetTypes();
            Type bitmapCreatorType = typeof(IBitmapCreator);
            var creatorTypes = (from t in types
                                where bitmapCreatorType != t && bitmapCreatorType.IsAssignableFrom(t)
                                select t).ToArray();

            if (creatorTypes.Any())
            {
                return Activator.CreateInstance(creatorTypes[0]) as IBitmapCreator;
            }

            throw new ArgumentException("Unable to locate IBitmapCreator implementation in specified assembly", nameof(assemblyName));
        }
    }
}
