using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PnP.Core.Model.SharePoint;
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
        _siteUrl = "private";
    }

    public void InitContext()
    {
        _context = _contextFactory.Create(_siteUrl);
    }

    public async Task<string> GetListsCountAsync()
    {
        await _context.Web.LoadAsync(s => s.Lists);
        _logger.LogInformation("lists count: {0}", _context.Web.Lists.Length);
        return _context.Web.Lists.Length.ToString();
    }

    public async Task<IList> GetListAsync(string listTitle)
    {
        var list = await _context.Web.Lists.GetByTitleAsync(listTitle, p => p.Title, p => p.Id, p=>p.Fields);
        var list2 = await _context.Web.Lists.GetByIdAsync(list.Id);   

        foreach (var field in list.Fields)   
        {
            _logger.LogInformation(field.Title);
        }
        return list;
    }
}

public interface IPnPService
{
    public void InitContext();
    public Task<string> GetListsCountAsync();
    public Task<PnP.Core.Model.SharePoint.IList> GetListAsync(string listTitle); 
}