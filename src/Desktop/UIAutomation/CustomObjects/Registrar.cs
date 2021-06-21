// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.CustomObjects;
using Axe.Windows.Core.CustomObjects.Converters;
using Axe.Windows.Core.Enums;
using Interop.UIAutomationCore;
using System;
using System.Collections.Generic;

namespace Axe.Windows.Desktop.UIAutomation.CustomObjects
{
    public class Registrar
    {
        private IUIAutomationRegistrar _uiaRegistrar;
        private Action<int, ITypeConverter> _converterRegistrationAction;
        public Dictionary<int, CustomProperty> IdsToCustomProperties { get; private set; }

        public Registrar()
            : this(new CUIAutomationRegistrar(), new Action<int, ITypeConverter>(A11yProperty.RegisterCustomProperty)) { }

        internal Registrar(IUIAutomationRegistrar uiaRegistrar, Action<int, ITypeConverter> converterRegistrationAction)
        {
            _uiaRegistrar = uiaRegistrar;
            _converterRegistrationAction = converterRegistrationAction;
            IdsToCustomProperties = new Dictionary<int, CustomProperty>();
        }

        static Registrar sDefaultInstance;

#pragma warning disable CA1024 // Use properties where appropriate: backing field
        public static Registrar GetDefaultInstance()
#pragma warning restore CA1024 // Use properties where appropriate: backing field
        {
            // This code is currently not thread safe.
            if (sDefaultInstance == null)
                sDefaultInstance = new Registrar();
            return sDefaultInstance;
        }

        public void RegisterCustomProperty(CustomProperty prop)
        {
            if (prop == null) throw new ArgumentNullException(nameof(prop));
            UIAutomationPropertyInfo info = new UIAutomationPropertyInfo
            {
                guid = prop.Guid,
                pProgrammaticName = prop.ProgrammaticName,
                type = GetUnderlyingUIAType(prop.Type)
            };

            _uiaRegistrar.RegisterProperty(ref info, out int dynamicId);
            _converterRegistrationAction(dynamicId, CreateTypeConverter(prop.Type));
            IdsToCustomProperties[dynamicId] = prop;
        }

        private static UIAutomationType GetUnderlyingUIAType(CustomUIAPropertyType type)
        {
            switch (type)
            {
                case CustomUIAPropertyType.String: return UIAutomationType.UIAutomationType_OutString;
                case CustomUIAPropertyType.Int: return UIAutomationType.UIAutomationType_Int;
                case CustomUIAPropertyType.Bool: return UIAutomationType.UIAutomationType_Bool;
                case CustomUIAPropertyType.Double: return UIAutomationType.UIAutomationType_Double;
                case CustomUIAPropertyType.Point: return UIAutomationType.UIAutomationType_OutPoint;
                case CustomUIAPropertyType.Element: return UIAutomationType.UIAutomationType_Element;
                default: throw new ArgumentException("Unset or unknown type", nameof(type));
            }
        }

        internal static ITypeConverter CreateTypeConverter(CustomUIAPropertyType type)
        {
            switch (type)
            {
                case CustomUIAPropertyType.String: return new StringTypeConverter();
                case CustomUIAPropertyType.Int: return new IntTypeConverter();
                case CustomUIAPropertyType.Bool: return new BoolTypeConverter();
                case CustomUIAPropertyType.Double: return new DoubleTypeConverter();
                case CustomUIAPropertyType.Point: return new PointTypeConverter();
                case CustomUIAPropertyType.Element: return new ElementTypeConverter();
                default: throw new ArgumentException("Unset or unknown type", nameof(type));
            }
        }
    }
}
