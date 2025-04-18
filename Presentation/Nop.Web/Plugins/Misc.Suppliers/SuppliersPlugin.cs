using DocumentFormat.OpenXml.Spreadsheet;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.Suppliers;
public class SuppliersPlugin : BasePlugin
{

    protected readonly ILocalizationService _localizationService;

    public SuppliersPlugin(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }




    public override async Task InstallAsync()
    {

       
        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await base.UninstallAsync();
    }





    public override async Task UpdateAsync(string currentVersion, string targetVersion)
    {

        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Admin.Suppliers"] = "Suppliers",
            ["Admin.Suppliers.Fields.Name"] = "Name",
            ["Admin.Suppliers.Fields.Email"] = "Email",
            ["Admin.Suppliers.Fields.Active"] = "Active",
            ["Admin.Suppliers.List.SearchName"] = "Supplier Name",
            ["Admin.Suppliers.List.SearchEmail"] = "Supplier Email",
            ["admin.suppliers.addnew"] = "Add a new supplier",
            ["admin.suppliers.backtolist"] = "back to supplier list",
            ["Admin.Suppliers.info"] = "Supplier Info",
            ["Admin.suppliers.editsupplierdetails"] = "Edit supplier details"
        });


        await base.UpdateAsync(currentVersion, targetVersion);
    }







    public class EventConsumer : IConsumer<AdminMenuCreatedEvent>
    {
        private readonly IPermissionService _permissionService;

        public EventConsumer(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermission.Configuration.MANAGE_PLUGINS))
                return;

            eventMessage.RootMenuItem.InsertBefore("Local plugins",
                new AdminMenuItem
                {
                    SystemName = "Misc.Suppliers", 
                    Title = "Suppliers", 
                    Url = eventMessage.GetMenuItemUrl("Suppliers", "List"),
                    IconClass = "far fa-dot-circle", 
                    Visible = true, 
                });
        }
    }
}
