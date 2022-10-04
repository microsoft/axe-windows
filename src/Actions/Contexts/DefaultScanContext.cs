using Axe.Windows.Desktop.UIAutomation.CustomObjects;
using System;

namespace Axe.Windows.Actions.Contexts
{
    internal class DefaultScanContext : IScanContext
    {
        static IScanContext DefaultContext;

        public DataManager DataManager => DataManager.GetDefaultInstance();

        public SelectAction SelectAction => SelectAction.GetDefaultInstance();

        public Registrar Registrar => Registrar.GetDefaultInstance();

        protected virtual void Dispose(bool disposing)
        {
            // This class never disposes, since the objects have application lifetime
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        internal static IScanContext GetDefaultInstance()
        {
            if (DefaultContext == null)
            {
                DefaultContext = new DefaultScanContext();
            }

            return DefaultContext;
        }
    }
}
