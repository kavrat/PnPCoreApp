using Microsoft.Extensions.Hosting;
using PnPCoreApp.Internal;

public static class Program
{
    public static async Task Main(string[] args)
    {
        IHost host = HostContextBuilder.New(args);
        using HostContext context = host.Build();
        
        await context.PnPService.GetListsCountAsync();

        Console.ReadKey();
    }
}