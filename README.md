![](https://img.shields.io/badge/SGS-OAD-orange) 
![](https://img.shields.io/badge/proj-demo%20nuget%20package-purple) 
![](https://img.shields.io/badge/-4.7.2-3484D2?logo=dotnet)
![](https://img.shields.io/badge/-4.8.1-3484D2?logo=dotnet)
![](https://img.shields.io/badge/-Standard%202.0-056473?logo=dotnet)
![](https://img.shields.io/badge/-6-512BD4?logo=dotnet)
![](https://img.shields.io/badge/-8-512BD4?logo=dotnet)
![](https://img.shields.io/badge/-NuGet-004880?logo=nuget)

# 📦demoNuGetPacakage

> [!NOTE] 
> a quick demo for neget package development

# 🕹️Features

- contains a interface `IMyService` with `DoWork()` method
- implement `IMyService` with class `MyService` 
- also implement `IDisposable` for `using` keyword
- using `Microsoft.Extensions.Logging.Abstractions` for log
- inject `ILoggerFactory` to create `ILogger<T>`

> [!IMPORTANT]
> - design a class `MyLoggerFactory` to warp `CreateLogger<T>()`
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

### 🪅one more things...

> [!TIP]
> - you need `host.Run()` for WebApp or Worker Service
> - `await host.RunAsync()` is also available
> - in Worker Service, remember to register service:

```cs
// MyWork.cs is your background service
builder.Services.AddHostedService<MyWorker>();
```