using Microsoft.Extensions.Logging;
using System;

namespace demoNuGetPacakage;

public class MyService : IMyService, IDisposable
{
    private bool _disposed;
    private readonly ILogger<MyService> _logger;
    private string _p1;
    private int _p2;

    public MyService(ILoggerFactory? loggerFactory = default)
    {
        _logger = MyLoggerFactory.CreateLogger<MyService>(loggerFactory);
        _logger.LogInformation("Creating Service {service}", nameof(MyService));
        _p1 = "default value";
        _p2 = 1234; // default value
    }

    public void DoWork()
    {
        _logger.LogInformation("Doing work with p1:{p1} and p2:{p2}", _p1, _p2);
    }

    public MyService SetParam1(string p1)
    {
        _p1 = p1;
        return this;
    }

    public MyService SetParam2(int p2)
    {
        _p2 = p2;
        return this;
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
