#if NET6_0 || NET8_0
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace demoNuGetPacakage;

/// <summary>
/// 提供 IServiceCollection 的擴展方法。
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// 向 IServiceCollection 添加 MyService 相關的服務。
    /// </summary>
    /// <param name="collection">IServiceCollection 的實例。</param>
    public static void UseMyService(this IServiceCollection collection)
    {
        collection.AddScoped<IMyServiceBuilder>(serviceProvider =>
        {
            // 從 ServiceProvider 取得 ILoggerFactory 注入
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            return MyServiceBuilder.Init(loggerFactory);
        });

        collection.AddScoped<IMyService>(serviceProvider =>
        {
            // 從 ServiceProvider 取得 IMyServiceBuilder 進行實例化
            var builder = serviceProvider.GetRequiredService<IMyServiceBuilder>();
            return builder.Build();
        });
    }
}
#else
using System;

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