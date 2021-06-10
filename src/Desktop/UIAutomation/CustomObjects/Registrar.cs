// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Interop.UIAutomationCore;
using System;
using System.Runtime.InteropServices;

namespace Axe.Windows.Desktop.UIAutomation.CustomObjects
{
    public class Registrar : IDisposable
    {
        CUIAutomationRegistrar _uiaRegistrar = new CUIAutomationRegistrar();
        private bool disposedValue;

        public int RegisterCustomProperty(Guid id, string name, UIAutomationType type)
        {
            UIAutomationPropertyInfo info = new UIAutomationPropertyInfo
            {
                guid = id,
                pProgrammaticName = name,
                type = type,
            };

            _uiaRegistrar.RegisterProperty(ref info, out int dynamicId);

            return dynamicId;
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
