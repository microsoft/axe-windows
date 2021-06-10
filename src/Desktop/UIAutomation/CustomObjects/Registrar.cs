// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Interop.UIAutomationCore;
using System;
using System.Runtime.InteropServices;

namespace Axe.Windows.Desktop.UIAutomation.CustomObjects
{
    public class Registrar : IDisposable
    {
        CUIAutomationRegistrarClass _uiaRegistrar = new CUIAutomationRegistrarClass();
        private bool disposedValue;

        public int Test()
        {
            UIAutomationPropertyInfo info = new UIAutomationPropertyInfo
            {
                guid = Guid.Empty,
                pProgrammaticName = "Some property",
                type = UIAutomationType.UIAutomationType_Bool,
            };

            _uiaRegistrar.RegisterProperty(ref info, out int dynamicProperty);

            return dynamicProperty;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // Free unmanaged resources (unmanaged objects) and set large fields to null
                Marshal.ReleaseComObject(_uiaRegistrar);
                _uiaRegistrar = null;

                disposedValue = true;
            }
        }

        ~Registrar()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
