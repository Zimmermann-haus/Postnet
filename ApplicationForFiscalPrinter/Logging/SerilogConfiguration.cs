using Microsoft.Extensions.Configuration;
using Serilog;

namespace ApplicationForFiscalPrinter.Logging;

public static class SerilogConfigurator
{
    public static void Configure()
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(
                new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build())
            .CreateLogger();
    }
}
