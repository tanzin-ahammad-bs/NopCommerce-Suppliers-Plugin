using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models;
public record SuppliersSearchModel : BaseSearchModel
{
    #region Properties

    [NopResourceDisplayName("Admin.Suppliers.List.SearchName")]
    public string SearchName { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.List.SearchEmail")]
    public string SearchEmail { get; set; }

    #endregion
}