using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PnPCoreApp.Services;

namespace PnPCoreApp.Internal;

public static class HostContextBuilder
{
    public static IHost New(string[] args, string appSettingsPath = null)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                if (!string.IsNullOrEmpty(appSettingsPath))
                {
                    config.AddJsonFile(appSettingsPath, optional: true, reloadOnChange: true);
                }
                else
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                }
            })
            .ConfigureServices((context, services) =>
            {
                services.AddPnP(context.Configuration);
                services.AddSingleton<IPnPService, PnPService>();
                services.AddLogging();
            }).Build();
    }
    
    public static HostContext Build(this IHost host)
    {
        IServiceProvider services = host.Services;

        return new HostContext(
            host,
            services.GetRequiredService<IPnPService>(),
            services.GetRequiredService<ILoggerFactory>()
        );
    }
}