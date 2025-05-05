using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.Purchaseorder.Areas.Admin.Factories;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories;
using Nop.Plugin.Misc.PurchaseOrder.Services;

namespace Nop.Plugin.Misc.PurchaseOrder.Infrastructure;

public class NopStartup : INopStartup
{
    public int Order => 1;

    public void Configure(IApplicationBuilder application)
    {
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Register services
        services.AddScoped<IPurchaseOrderModelFactory, PurchaseOrderModelFactory>();
        services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
        services.AddScoped<SelectedProductService>();
        services.AddScoped<ISupplierProductMappingService, SupplierProductMappingService>();

        // Register event consumer
        services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
    }
}
