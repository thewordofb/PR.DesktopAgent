using DesktopAgent.Scan.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DesktopAgent.Scan.API
{
    public static class ScanningProvider
    {
        public static IServiceCollection AddScanningServices(this IServiceCollection services)
        {
            services.AddSingleton<IDocumentService, DocumentService>();
            services.AddSingleton<IScannerService, ScannerService>();
            services.AddSingleton<IScanJobService, ScanJobService>();

            return services;
        }

        public static IMvcCoreBuilder AddScanningControllers(this IMvcCoreBuilder builder)
        {
            builder.AddApplicationPart(typeof(ScanningProvider).Assembly);

            return builder;
        }
    }
}
