using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PnP.Core.Auth.Services.Builder.Configuration;
using PnP.Core.Services.Builder.Configuration;

namespace PnPCoreApp.Internal;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPnP(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPnPCore(options =>
        {
            options.PnPContext.GraphFirst = true;
            options.Sites.Add("private", new PnPCoreSiteOptions
            {
                SiteUrl = configuration["PnPCore:SiteUrl"],
            });
        });
        services.AddPnPCoreAuthentication(options =>
        {
            var pnpConfiguration = configuration.GetSection("PnPCore");

            options.Credentials.Configurations.Add("x509certificate", new PnPCoreAuthenticationCredentialConfigurationOptions
            {
                ClientId = pnpConfiguration["ClientId"],
                TenantId = pnpConfiguration["TenantId"],
                X509Certificate = new PnPCoreAuthenticationX509CertificateOptions
                {
                    StoreName = StoreName.My,
                    StoreLocation = StoreLocation.CurrentUser,
                    Thumbprint = pnpConfiguration["Thumbprint"]
                }
            });
            options.Credentials.DefaultConfiguration = "x509certificate";
            options.Sites.Add("private",
                new PnPCoreAuthenticationSiteOptions
                {
                    AuthenticationProviderName = "x509certificate"
                });
        });

        return services;
    }
}