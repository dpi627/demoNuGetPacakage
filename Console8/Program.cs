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

        var builder = Host.CreateApplicationBuilder(args);

    }
}
