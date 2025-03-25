using demoNuGetPacakage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        using MyService service = new();
        service.SetParam1("Hello").SetParam2(9527).DoWork();
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
        //builder.Services.AddSingleton<IMyService, MyService>(x =>
        //    new MyService().SetParam1("Hey").SetParam2(666)
        //);
        builder.Services.AddSingleton<IMyService, MyService>(x => {
            var myService = new MyService().SetParam1("Hi").SetParam2(87);
            // do something with myService...
            return myService;
        });
        builder.Services.AddTransient<MyTest>();

        // 如果是 Worker Service，服務會註冊為 Singleton，並且自動啟動
        // builder.Services.AddHostedService<MyWorker>(); // MyWorker 為 BackgroundService 實作

        // 如果是一次性執行，只要 build() 就可以
        var host = builder.Build();

        // WebApp 或 Worker Service 還需 Run() 讓服務持續運行
        //host.Run(); // 或 await host.RunAsync();
        #endregion

        // 一次性執行程式，build() 之後可從 host.Services 取得服務執行
        var myService = host.Services.GetRequiredService<IMyService>();
        myService.DoWork();

        var myTest = host.Services.GetRequiredService<MyTest>();
        myTest.Run(); // 如果將前面 MyService 改為 AddTransient，執行後會看到服務生成兩次
    }

    /// 高階模組(MyTest)依賴低階模組(MyService)的介面(Interface)而非實體
    /// 之後抽換實作只要改 DI Container 的註冊即可，例如改為 <IMyService, MyNewServiceV2>
    class MyTest(IMyService service)
    {
        public void Run()
        {
            service.DoWork();
        }
    }
}
