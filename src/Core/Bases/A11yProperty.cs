// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.CustomObjects.Converters;
using Axe.Windows.Core.Misc;
using Axe.Windows.Core.Resources;
using Axe.Windows.Core.Types;
using Axe.Windows.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;

namespace Axe.Windows.Core.Bases
{
    /// <summary>
    /// Wrapper class for UIAutomationElement Property
    /// </summary>
    public class A11yProperty : IDisposable
    {
        /// <summary>
        /// Property Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Property Name
        /// </summary>
        public string Name { get; set; }

#pragma warning disable CA1051 // Do not declare visible instance fields
        /// <summary>
        /// Property value
        /// because it is used in referenced variable later, it can't be property.
        /// please keep it as field.
        ///
        /// CA1051 because of backward compat issue with loading existing results file, can't change it to field.
        /// </summary>
        public dynamic Value;
#pragma warning restore CA1051 // Do not declare visible instance fields

        [JsonIgnore]
        public string TextValue
        {
            get
            {
                return ToString();
            }
        }

        static readonly ConcurrentDictionary<int, ITypeConverter> TypeConverterMap = new ConcurrentDictionary<int, ITypeConverter>();

        /// <summary>
        /// Constructor with normal case
        /// </summary>
        /// <param name="id"></param>
        /// <param name="element"></param>
        public A11yProperty(int id, dynamic value, string name = null) : this(id, name)
        {
            Value = value;
        }

        /// <summary>
        /// private constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name">if null, it is get name from PropertyTypes by id</param>
        private A11yProperty(int id, string name)
        {
            Id = id;
            Name = name ?? PropertyType.GetInstance().GetNameById(id);
        }

        /// <summary>
        /// Constructor for serialization
        /// </summary>
        public A11yProperty() { }

        public override string ToString()
        {
            string txt = null;

            if (Value != null)
            {
                switch (Id)
                {
                    case PropertyType.UIA_RuntimeIdPropertyId:
                        txt = this.ConvertIntArrayToString();
                        break;
                    case PropertyType.UIA_ControlTypePropertyId:
                        txt = Value != null ? ControlType.GetInstance().GetNameById(Value) : "";
                        break;
                    case PropertyType.UIA_BoundingRectanglePropertyId:
                        // if bounding rectangle is [0,0,0,0], treat it as non-exist. same behavior as Inspect
                        txt = GetBoundingRectangleText();
                        break;
                    case PropertyType.UIA_OrientationPropertyId:
                        switch ((int)Value)
                        {
                            case 0: //OrientationType_None
                                txt = DisplayStrings.NoneOrientation;
                                break;
                            case 1: //OrientationType_Horizontal
                                txt = DisplayStrings.HorizontalOrientation;
                                break;
                            case 2: // OrientationType_Vertical
                                txt = DisplayStrings.VerticalOrientation;
                                break;
                        }
                        break;
                    case PropertyType.UIA_PositionInSetPropertyId:
                    case PropertyType.UIA_LevelPropertyId:
                    case PropertyType.UIA_SizeOfSetPropertyId:
                        /// these properties are 1 based.
                        /// if value is smaller than 1, it should be ignored.
                        if (Value != null && Value > 0)
                        {
                            txt = Value?.ToString();
                        }
                        break;
                    case PropertyType.UIA_HeadingLevelPropertyId:
                        txt = HeadingLevelType.GetInstance().GetNameById(Value);
                        break;
                    case PropertyType.UIA_LandmarkTypePropertyId:
                        txt = Value != 0 ? LandmarkType.GetInstance().GetNameById(Value) : null; // 0 is default value.
                        break;
                    default:
                        if (TypeConverterMap.TryGetValue(Id, out ITypeConverter converter))
                        {
                            txt = converter.Render(Value);
                        }
                        else if (Value is Int32[])
                        {
                            txt = ((Int32[])Value).ConvertInt32ArrayToString();
                        }
                        else if (Value is Double[])
                        {
                            txt = ((Double[])Value).ConvertDoubleArrayToString();
                        }
                        else
                        {
                            txt = Value?.ToString();
                        }
                        break;
                }
            }
            return txt;
        }

        /// <summary>
        /// Get proper bounding rectangle text based on the data
        /// </summary>
        /// <returns></returns>
        private string GetBoundingRectangleText()
        {
            var arr = Value;

            string text;
            if ((double)arr[2] < 0 || (double)arr[3] < 0)
            {
                // the 3rd and 4th values in array are negative value, we need to show value in different format like l,t,w,h
                text = ExtensionMethods.WithParameters(DisplayStrings.BoundingRectangleFormat, arr[0], arr[1], arr[2], arr[3]);
            }
            else
            {
                text = this.ToRectangle().IsEmpty == false ? this.ToRectangle().ToLeftTopRightBottomString() : null;
            }

            return text;
        }

        public static void RegisterCustomProperty(int dynamicId, ITypeConverter converter)
        {
            TypeConverterMap[dynamicId] = converter;
        }

        #region IDisposable Support
        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Name = null;
                    if (Value != null)
                    {
                        if (NativeMethods.VariantClear(ref Value) == Win32Constants.S_OK)
                        {
                            Value = null;
                        }
                    }
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
