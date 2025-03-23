using System;
using System.Collections.Generic;
using System.Text;

namespace demoNuGetPacakage
{
    internal class MyService : IDisposable
    {
        private bool _disposed;

        // Public implementation of Dispose pattern
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any managed objects here, e.g.:
                // _managedResource?.Dispose();
            }

            // Free any unmanaged objects here, e.g.:
            // if (_unmanagedHandle != IntPtr.Zero)
            // {
            //     NativeMethods.ReleaseHandle(_unmanagedHandle);
            //     _unmanagedHandle = IntPtr.Zero;
            // }

            _disposed = true;
        }

        // Finalizer
        ~MyService()
        {
            Dispose(false);
        }
    }
}
