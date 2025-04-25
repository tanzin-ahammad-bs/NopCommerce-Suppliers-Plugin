using Microsoft.AspNetCore.Mvc.Razor;
namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Infrastructure;
public class ViewLocationExpander : IViewLocationExpander
{

    public void PopulateValues(ViewLocationExpanderContext context)
    {

    }
    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        if (context.AreaName == "Admin")
        {
            viewLocations = new string[]
            {
                $"/Plugins/Nop.Plugin.Misc.Suppliers/Areas/Admin/Views/{{0}}.cshtml",
                $"/Plugins/Nop.Plugin.Misc.Suppliers/Areas/Admin/Views/{{1}}/{{0}}.cshtml"
            }.Concat(viewLocations);
        }
        else
        {
            viewLocations = new string[]
            {
                $"/Plugins/Nop.Plugin.Misc.Suppliers/Areas/Admin/Views/{{0}}.cshtml",
                $"/Plugins/Nop.Plugin.Misc.Suppliers/Areas/Admin/Views/{{1}}/{{0}}.cshtml"
            }.Concat(viewLocations);
        }
        return viewLocations;
    }
}