using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Events;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.Purchaseorder.Areas.Admin.Factories;
using Nop.Plugin.Misc.Purchaseorder.Areas.Admin.Services;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domain;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Services;



namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Infrastructure;
public class NopStartup : INopStartup
{
    public int Order => 1;

    public void Configure(IApplicationBuilder application)
    {
        
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {

        
        services.AddScoped<IPurchaseOrderModelFactory, PurchaseOrderModelFactory>();

        // Register the PurchaseOrderService
        services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
        services.AddScoped<SelectedProductService>();
        services.AddScoped<ISupplierProductMappingService, SupplierProductMappingService>();
    }
}
