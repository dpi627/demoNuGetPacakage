using demoNuGetPacakage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Console8;

internal class Program
{
    static void Main(string[] args)
    {
        // create serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        Log.Information("Starting up");

        #region 不使用 Hosting Builder，直接 using
        //using MyService service = new();
        //service.DoWork();
        #endregion

        #region 不使用 Hosting Builder，直接 using (並注入 ILoggerFactory)
        //using ILoggerFactory loggerFactory = LoggerFactory.Create(config =>
        //{
        //    config.AddSerilog();
        //});
        //using MyService service = new(loggerFactory);
        //service.DoWork();
        #endregion

        #region 使用 Hosting Builder
        // create host
        var builder = Host.CreateApplicationBuilder(args);

        // add services
        builder.Services.AddSerilog();
        builder.Services.AddSingleton<MyService>();

        // 如果是 Worker Service，服務會註冊為 Singleton，並且自動啟動
        // builder.Services.AddHostedService<MyWorker>(); // MyWorker 為 BackgroundService 實作

        // 如果是一次性執行，只要 build() 就可以
        var host = builder.Build();

        // WebApp 或 Worker Service 還需 Run() 讓服務持續運行
        //host.Run(); // 或 await host.RunAsync();
        #endregion

        // 一次性執行程式，build() 之後可從 host.Services 取得服務執行
        var myService = host.Services.GetRequiredService <MyService>();
        myService.DoWork();
    }
}
