using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Misc.Suppliers.Infrastructure;
public class RouteProvider : IRouteProvider
{
    public int Priority => 0;

    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapControllerRoute(name: SuppliersDefault.ListRouteName,
            pattern: "Admin/Suppliers/List",
            defaults: new { controller = "Suppliers", action = "List", area = AreaNames.ADMIN });
        
        endpointRouteBuilder.MapControllerRoute(name: SuppliersDefault.SupplierToProductRouteName,
            pattern: "/Admin/Suppliers/AssignSupplierToProduct",
            defaults: new { controller = "Suppliers", action = "AssignSupplierToProduct", area = AreaNames.ADMIN });
        
    }
}
