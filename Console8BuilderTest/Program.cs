using demoNuGetPacakage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Console8BuilderTest;

internal class Program
{
    static void Main(string[] args)
    {
        // create serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        Log.Information("Starting up");

        var builder = Host.CreateApplicationBuilder(args);

        // add services
        builder.Services.AddSerilog();

        //// 註冊 IMyServiceBuilder
        //// 進行實例化，並從 ServiceProvider 取得 ILoggerFactory 注入
        //builder.Services.AddScoped<IMyServiceBuilder>(serviceProvider =>
        //{
        //    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        //    return MyServiceBuilder.Init(loggerFactory);
        //});

        //// 註冊 IMyService
        //// 從 ServiceProvider 取得 IMyServiceBuilder 進行實例化
        //// 如果不需要設定參數，可以直接註冊 MyService 或在其他類別取用 IMyServiceBuilder 建立
        //builder.Services.AddScoped<IMyService>(serviceProvider =>
        //{
        //    var builderService = serviceProvider.GetRequiredService<IMyServiceBuilder>();
        //    return builderService.SetParam1("default value")
        //                         .SetParam2(123)
        //                         .Build();
        //});

        // 使用擴充方法簡化註冊服務
        builder.Services.UseMyService();

        builder.Services.AddTransient<MyTest>();

        // 如果是一次性執行，只要 build() 就可以
        var host = builder.Build();

        // 一次性執行程式，build() 之後可從 host.Services 取得服務執行
        var myService = host.Services.GetRequiredService<IMyService>();
        myService.DoWork();

        var myTest = host.Services.GetRequiredService<MyTest>();
        myTest.Run();

        // 使用 IMyServiceBuilder 建立 IMyService
        var serviceBuilder = host.Services.GetRequiredService<IMyServiceBuilder>();
        myTest.RunWithBuilder(serviceBuilder);
    }

    class MyTest(IMyService service)
    {
        public void Run()
        {
            // 使用 constructor 注入的 IMyService
            service.SetParam1("Woo").SetParam2(500).DoWork();
        }

        public void RunWithBuilder(IMyServiceBuilder builder)
        {
            // 使用 IMyServiceBuilder 建立 IMyService
            var service = builder.SetParam1("From Builder").SetParam2(888).Build();
            service.DoWork();
        }
    }
}
