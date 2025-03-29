![](https://img.shields.io/badge/SGS-OAD-orange) 
![](https://img.shields.io/badge/proj-demo%20nuget%20package-purple) 
![](https://img.shields.io/badge/-4.7.2-3484D2?logo=dotnet)
![](https://img.shields.io/badge/-4.8.1-3484D2?logo=dotnet)
![](https://img.shields.io/badge/-Standard%202.0-056473?logo=dotnet)
![](https://img.shields.io/badge/-6-512BD4?logo=dotnet)
![](https://img.shields.io/badge/-8-512BD4?logo=dotnet)
![](https://img.shields.io/badge/-NuGet-004880?logo=nuget)

# 📦demoNuGetPackage

> [!NOTE] 
> a quick demo for nuget package development

# 🕹️Features

- contains a interface `IMyService` with `DoWork()` method
- implement `IMyService` with class `MyService` 
- also implement `IDisposable` for `using` keyword
- using `Microsoft.Extensions.Logging.Abstractions` for log
- inject `ILoggerFactory` to create `ILogger<T>`

> [!IMPORTANT]
> - design a class `MyLoggerFactory` to wrap `CreateLogger<T>()`
> - it returns `NullLogger<T>` if `ILoggerFactory` equals to `default`

# 📟Console8

> [!NOTE]
> a simple `.NET8` console project to show some use cases

## 1. Use `using`

### 🪂Direct `using`

```cs
// no logger inject
using MyService service = new();
service.DoWork();
```

### 🪐With `ILoggerFactory` injection

```cs
// create ILoggerFactory from Serilog
using ILoggerFactory loggerFactory = LoggerFactory.Create(config =>
{
    config.AddSerilog();
});

// inject ILoggerFactory
using MyService service = new(loggerFactory);
service.DoWork();
```

launch the console app, you can see the screen output:

```sh
[19:59:24 INF] Starting up
[19:59:24 INF] Creating Service MyService
[19:59:24 INF] Doing work.
```

> [!TIP]
> for old `C#` version, you may need to use `using` like:

```cs
using (var service = new MyService())
{
    service.DoWork();
};
```

## 2. Dependency Injection

### 👽Build with `HostBuilder`

```cs
// create host builder
var builder = Host.CreateApplicationBuilder(args);

// add services
builder.Services.AddSerilog();
builder.Services.AddSingleton<IMyService, MyService>();

// build host
var host = builder.Build();
```

### 🍨One more thing...

> [!IMPORTANT]
> - when using `Microsoft.Extensions.Hosting`
> - you need `host.Run()` for WebApp or Worker Service
> - `await host.RunAsync()` is also available
> - in Worker Service, remember to register service:

```cs
// MyWork.cs is your background service
builder.Services.AddHostedService<MyWorker>();
```

### 🍄Get service from `ServiceProvider`
  
```cs
var myService = host.Services.GetRequiredService<IMyService>();
myService.DoWork();
```

### 🌵Or inject to class, e.g. `constructor-injection`
  
```cs
class MyTest(IMyService service)
{
    void Run()
    {
        service.DoWork();
    }
}
```

### 🏭`Factory` & `Builder` design pattern

```cs
// register service with default build (no logger)
builder.Services.AddScoped<IMyService>(provider =>
{
    var builder = provider.GetRequiredService<IMyServiceBuilder>();
    return builder.SetParam1("default value")
        .SetParam2(123)
        .Build();
});
```

```cs
// register builder with logger provider
builder.Services.AddScoped<IMyServiceBuilder>(provider =>
{
    var factory = provider.GetService<ILoggerFactory>();
    return MyServiceBuilder.Init(factory);
});

class MyTest()
{
    // inject and build in method
    public void RunWithBuilder(IMyServiceBuilder builder)
    {
        var service = builder
            .SetParam1("From Builder")
            .SetParam2(888)
            .Build();
        service.DoWork();
    }
}
```

>[!TIP]
> create an `Extension` for `DI` is a plus ✨

## 3. Dependency Injection Extension

```cs
// only for .NET 6+ with DI (AddScoped)
builder.Services.UseMyService();
```

- 預設 `AddScoped` 適合 WebApp，DesktopApp 可能導致單例行為
- 改用 `AddTransient` 可解決，但在 WebApp 可能造成不必要的實例創建
- 桌面應用可手動管理範圍，例如

```cs
using (var scope = provider.CreateScope())
{
    var service1 = scope.ServiceProvider.GetRequiredService<IMyService>();
    var service2 = scope.ServiceProvider.GetRequiredService<IMyService>();
    // service1 和 service2 是同一個實例，因為在同一個範圍內
}
using (var scope = provider.CreateScope())
{
    var service3 = scope.ServiceProvider.GetRequiredService<IMyService>();
    // service3 是新實例，因為在新範圍中
}
```

- 套件實作了生命週期控制，可依照應用程式類型設定即可

```cs
// support lifetime setting, e.g. AddTransient
builder.Services.UseMyService(ServiceLifetime.Transient);
```