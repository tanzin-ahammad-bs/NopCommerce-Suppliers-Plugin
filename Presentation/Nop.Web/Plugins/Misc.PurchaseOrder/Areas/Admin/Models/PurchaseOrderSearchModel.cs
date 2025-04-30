using Nop.Web.Framework.Models;
using System;

namespace Nop.Plugin.Misc.Suppliers.Areas.Admin.Models
{
    public partial record class PurchaseOrderSearchModel : BaseSearchModel
    {
        public string SupplierName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
