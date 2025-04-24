using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
public partial record SupplierProductModel : BaseNopModel
{
    [NopResourceDisplayName("Plugins.Misc.Suppliers.Fields.ProductId")]
    public int ProductId { get; set; }

    [NopResourceDisplayName("Plugins.Misc.Suppliers.Fields.SupplierId")]
    public int SupplierId { get; set; }

    [NopResourceDisplayName("Plugins.Misc.Suppliers.Fields.Suppliers")]
    public IList<SelectListItem> Suppliers { get; set; } = new List<SelectListItem>();
}

