using Microsoft.Extensions.Hosting;
using PnPCoreApp.Internal;

public static class Program
{
    public static async Task Main(string[] args)
    {
        IHost host = HostContextBuilder.New(args);
        using HostContext context = host.Build();
        
        context.PnPService.InitContext();
        await context.PnPService.GetListsCountAsync();
        await context.PnPService.GetListAsync("Test Root List");

        Console.ReadKey();
    }
}