using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace demoNuGetPacakage;

/// <summary>
/// Factory class for creating loggers.
/// </summary>
internal class MyLoggerFactory
{
    /// <summary>
    /// Creates a logger for the specified type.
    /// </summary>
    /// <typeparam name="T">The type for which to create a logger.</typeparam>
    /// <param name="loggerFactory">The logger factory to use. If null, a <see cref="NullLogger{T}"/> instance is returned.</param>
    /// <returns>An <see cref="ILogger{T}"/> instance.</returns>
    public static ILogger<T> CreateLogger<T>(ILoggerFactory? loggerFactory = default) =>
        loggerFactory?.CreateLogger<T>() ?? NullLogger<T>.Instance;
}
