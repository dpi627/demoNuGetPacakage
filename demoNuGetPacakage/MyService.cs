using Microsoft.Extensions.Logging;
using System;

namespace demoNuGetPacakage;

internal class MyService : IDisposable
{
    private bool _disposed;
    private readonly ILogger<MyService> _logger;

    public MyService(ILoggerFactory? loggerFactory = default)
    {
        _logger = MyLoggerFactory.CreateLogger<MyService>();
        _logger.LogInformation("Creating Service {service}", nameof(MyService));
    }

    public void DoWork()
    {
        _logger.LogInformation("Doing work.");
    }

    #region Implementation of Dispose pattern
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
    #endregion
}
