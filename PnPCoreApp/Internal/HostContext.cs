using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PnPCoreApp.Services;

namespace PnPCoreApp.Internal;

public class HostContext : IDisposable
{
    private readonly IHost _host;
    private readonly Stopwatch _watch;
    
    public IPnPService PnPService { get; }
    public ILoggerFactory LoggerFactory { get; }
    
    public HostContext(IHost host, IPnPService pnpService, ILoggerFactory LoggerFactory)
    {
        _host = host;
        _watch = Stopwatch.StartNew();
        PnPService = pnpService;
        LoggerFactory = LoggerFactory;
        
        _watch = Stopwatch.StartNew();
    }

    public void Dispose()
    {
        _watch.Stop();
        _host.Dispose();
    }
}