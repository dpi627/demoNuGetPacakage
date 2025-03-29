using System;

#if NET6_0 || NET8_0
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace demoNuGetPacakage;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// 於 IServiceCollection 註冊 MyService 服務。
    /// </summary>
    /// <param name="collection">IServiceCollection 實例</param>
    public static void UseMyService(
        this IServiceCollection collection,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        collection.AddService<IMyServiceBuilder>(lifetime, serviceProvider =>
        {
            // 從 ServiceProvider 取得 ILoggerFactory 注入
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            return MyServiceBuilder.Init(loggerFactory);
        });

        collection.AddService<IMyService>(lifetime, serviceProvider =>
        {
            // 從 ServiceProvider 取得 IMyServiceBuilder 進行實例化
            var builder = serviceProvider.GetRequiredService<IMyServiceBuilder>();
            return builder.Build();
        });
    }

    private static void AddService<T>(
        this IServiceCollection collection,
        ServiceLifetime lifetime,
        Func<IServiceProvider, T> factory) where T : class
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                collection.AddSingleton(factory);
                break;
            case ServiceLifetime.Scoped:
                collection.AddScoped(factory);
                break;
            case ServiceLifetime.Transient:
                collection.AddTransient(factory);
                break;
        }
    }
}

#else

namespace demoNuGetPacakage;

public static class ServiceCollectionExtension
{
    [Obsolete("UseMyService is only available in .NET 6+")]
    public static void UseMyService(this object collection)
    {
        throw new PlatformNotSupportedException("This method is only supported in .NET 6 or later.");
    }
}

#endif