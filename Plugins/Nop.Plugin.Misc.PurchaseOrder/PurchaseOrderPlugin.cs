using Nop.Core;
using Nop.Services.Localization;
using Nop.Web.Framework.Menu;
using Nop.Services.Plugins;
using System.Threading.Tasks;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Services.Events;
using Nop.Web.Framework.Events;
using Nop.Services.Security;

namespace Nop.Plugin.Misc.Purchaseorder;
public class PurchaseOrderPlugin : BasePlugin
{

    protected readonly ILocalizationService _localizationService;

    public PurchaseOrderPlugin(ILocalizationService localizationService)
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

            ["Admin.PurchaseOrder"] = "Purchase Orders",
            ["Admin.PurchaseOrder.Fields.PurchaseOrderId"] = "Purchase Order ID",
            ["Admin.PurchaseOrder.Fields.SupplierName"] = "Supplier Name",
            ["Admin.PurchaseOrder.Fields.CreationDate"] = "Creation Date",
            ["Admin.PurchaseOrder.Fields.TotalAmount"] = "Total Amount",
            ["Admin.PurchaseOrder.Fields.CreatedBy"] = "Created By",
            ["Admin.PurchaseOrder.Fields.StartDate"] = "Start Date",
            ["Admin.PurchaseOrder.Fields.EndDate"] = "End Date",
            ["Admin.PurchaseOrder.Fields.PurchaseOrderStatus"] = "Purchase Order Status",
            ["Admin.PurchaseOrder.AddNew"] = "Add New Purchase Order",
            ["Admin.PurchaseOrder.Edit"] = "Edit Purchase Order",
            ["Admin.PurchaseOrder.Delete"] = "Delete Purchase Order",
            ["Admin.PurchaseOrder.Search"] = "Search Purchase Orders",
            ["Admin.PurchaseOrder.NoResults"] = "No Purchase Orders Found",
            ["Admin.PurchaseOrder.ViewDetails"] = "View Details",
            ["Admin.PurchaseOrder.Details"] = "Purchase Order Details",
            ["Admin.PurchaseOrder.Save"] = "Save Purchase Order",
            ["Admin.PurchaseOrder.Cancel"] = "Cancel",
            ["Admin.PurchaseOrder.Fields.SupplierName"] = "Supplier Name",
            ["Admin.PurchaseOrder.Fields.StartDate"] = "Start Date",
            ["Admin.PurchaseOrder.Fields.EndDate"] = "End Date",
            ["Admin.PurchaseOrder.Fields.TotalAmount"] = "Total Amount",
            ["Admin.PurchaseOrder.Fields.CreatedBy"] = "Created By",
            ["Admin.PurchaseOrder.Action.Save"] = "Save Purchase Order",
            ["Admin.PurchaseOrder.Action.Cancel"] = "Cancel",
            ["Admin.Catalog.Products.AddProducts"] = "Add Products",
            ["Admin.Catalog.Products.SelectProductsForSupplier"] = "Select Products for Supplier",
            ["Admin.Catalog.Products.SelectSupplier"] = "Select Supplier",
            ["Admin.Catalog.Products.Name"] = "Product Name",
            ["Admin.Catalog.Products.SKU"] = "SKU",
            ["Admin.Catalog.Products.CurrentStock"] = "Current Stock",
            ["Admin.Catalog.Products.QuantityToOrder"] = "Quantity to Order",
            ["Admin.Catalog.Products.UnitCost"] = "Unit Cost",
            ["Admin.Catalog.Products.LineTotal"] = "Line Total",
            ["Admin.Catalog.Products.RelatedProducts.AddNew"] = "Add Related Products",
            ["Admin.Catalog.Products.Fields.Name"] = "Name",
            ["Admin.Catalog.Products.Fields.Published"] = "Published",
            ["Admin.Common.Actions"] = "Actions",
            ["Admin.Common.Save"] = "Save",
            ["Admin.Common.Edit"] = "Edit",
            ["Admin.Common.Delete"] = "Delete",
            ["Admin.Common.Yes"] = "Yes",
            ["Admin.Common.No"] = "No",
            ["Admin.Common.Save"] = "Save",
            ["Admin.Common.Close"] = "Close",
            ["Admin.Common.Add"] = "Select Product",
            ["Admin.Common.Back"] = "Back",
            ["Admin.Common.Supplier"] = "Select a Supplier",
            ["Admin.PurchaseOrder.Fields.ProductName"] = "Product Name",
            ["Admin.PurchaseOrder.Fields.Sku"] = "SKU",
            ["Admin.PurchaseOrder.Fields.CurrentStock"] = "Current Stock",
            ["Admin.PurchaseOrder.Fields.QuantityToOrder"] = "Quantity To Order",
            ["Admin.PurchaseOrder.Fields.UnitCost"] = "Unit Cost",
            ["Admin.Common.Update"] = "Update"


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

            

            eventMessage.RootMenuItem.InsertAfter("Help",
                new AdminMenuItem
                {
                    SystemName = "PurchaseOrder",
                    Title = "Purchase Order",
                    Url = eventMessage.GetMenuItemUrl("PurchaseOrder", "PurchaseList"),
                    IconClass = "far fa-dot-circle",
                    Visible = true
                });





        }
    }


}
