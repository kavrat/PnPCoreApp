using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PnP.Core.Services;

namespace PnPCoreApp.Services;

public class PnPService : IPnPService
{
    private readonly string? _siteUrl;
    private ILogger<PnPService> _logger;
    private IPnPContextFactory _contextFactory;
    private PnPContext _context;
    public PnPService(ILoggerFactory loggerFactory, IPnPContextFactory contextFactory, IConfiguration configuration)
    {
        _contextFactory = contextFactory;
        _logger = loggerFactory.CreateLogger<PnPService>();
        _siteUrl = configuration["SiteUrl"];
    }

    public void InitContext()
    {
        _context = _contextFactory.Create(_siteUrl);
    }

    public async Task<string> GetListsCountAsync()
    {
        InitContext();
        await _context.Web.LoadAsync(s => s.Lists);
        _logger.LogInformation("lists count: {0}", _context.Web.Lists.Length);
        return _context.Web.Lists.Length.ToString();
    }
}

public interface IPnPService
{
    public void InitContext();
    public Task<string> GetListsCountAsync();
}