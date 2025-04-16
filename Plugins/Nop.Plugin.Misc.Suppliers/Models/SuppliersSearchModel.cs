using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.Suppliers.Models;
public record SuppliersSearchModel : BaseSearchModel
{
    #region Properties

    [NopResourceDisplayName("Admin.Vendors.List.SearchName")]
    public string SearchName { get; set; }

    [NopResourceDisplayName("Admin.Vendors.List.SearchEmail")]
    public string SearchEmail { get; set; }

    #endregion
}