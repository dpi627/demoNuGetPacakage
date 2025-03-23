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

        // create host
        var builder = Host.CreateApplicationBuilder(args);

        // add services
        builder.Services.AddSerilog();
        builder.Services.AddSingleton<MyService>();

        var host = builder.Build();
        //host.Run();

        // get service and call method
        var myService = host.Services.GetRequiredService <MyService>();
        myService.DoWork();
    }
}
