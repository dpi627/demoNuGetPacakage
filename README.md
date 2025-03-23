# demoNuGetPacakage

> [!NOTE] 
> a quick demo for neget package development

# Freatures

- contains a simple class `MyService`
- contains a simple interface `IMyService`
- `MyService` contains a method `DoWork()`
- using `Microsoft.Extensions.Logging`
- `MyService` implement `IMyService` and `IDisposable`
- inject `ILoggerFactory` for create `ILogger<T>`

> [!IMPORTANT]
> - design a class `MyLoggerFactory` to warp `CreateLogger<T>()`
> - it returns `NullLogger<T>` if `ILoggerFactory` equals to `default`

# Console8

> [!NOTE]
> a simple console project to show some use cases

## 1. Use `using`

### direct `using`

```cs
// no logger inject
using MyService service = new();
service.DoWork();
```

### with `ILoggerFactory` injection

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

> [!TIP]
> for old `C#` version, you may need to use `using` like below

```cs
using (var service = new MyService())
{
    service.DoWork();
};
```

## 2. Dependency Injection

### build with `HostBuilder`

```cs
// create host builder
var builder = Host.CreateApplicationBuilder(args);

// add services
builder.Services.AddSerilog();
builder.Services.AddSingleton<IMyService, MyService>();

// build host
var host = builder.Build();
```

### get service from `ServiceProvider`
  
```cs
var myService = host.Services.GetRequiredService<IMyService>();
myService.DoWork();
```

### or inject to class, e.g. `constructor-injection`
  
```cs
class MyTest(IMyService service)
{
    void Run()
    {
        service.DoWork();
    }
}
```

> [!TIP]
> - you need `host.Run()` for WebApp or Worker Service
> - `await host.RunAsync()` is also available
> - in Worker Service, you need to register service like below

```cs
// MyWork.cs is your background service
builder.Services.AddHostedService<MyWorker>();
```